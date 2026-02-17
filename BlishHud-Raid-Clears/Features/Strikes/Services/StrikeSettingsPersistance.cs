using Blish_HUD.Settings;
using Newtonsoft.Json;
using RaidClears.Features.Raids.Services;
using RaidClears.Features.Shared;
using RaidClears.Features.Shared.Models;
using RaidClears.Features.Strikes.Models;
using RaidClears.Localization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace RaidClears.Features.Strikes.Services;


[Serializable]
public class StrikeSettingsPersistance : Labelable
{
    public StrikeSettingsPersistance() {
        _isStrike = true;
    }

    public event EventHandler<bool>? StrikeSettingsChanged;

    [JsonIgnore]
    public static string FILENAME = "strike_settings.json";

    private const string CURRENT_VERSION = "3.5.0";

    /// <summary>Versions we accept; unknown versions cause a fresh config.</summary>
    private static readonly HashSet<string> SupportedVersions = new(StringComparer.Ordinal)
    {
        "3.0.0", CURRENT_VERSION
    };

    /// <summary>Legacy versions that need MigratePriorityKeysFromStorage before upgrading to CURRENT_VERSION.</summary>
    private static readonly HashSet<string> VersionsRequiringPriorityMigration = new(StringComparer.Ordinal)
    {
        "3.0.0"
    };

    [JsonIgnore]
    protected Dictionary<string, SettingEntry<bool>> VirtualSettingsEnties = new();

    [JsonProperty("version")]
    public string Version { get; set; } = CURRENT_VERSION;



    [JsonProperty("priority")]
    public bool Priority { get; set; } = true;

    [JsonProperty("tomorrow_bounties")]
    public bool TomorrowBounties { get; set; } = false;


    [JsonProperty("expansions")]
    public Dictionary<string, bool> Expansions { get; set; } = new();

    [JsonProperty("missions")]
    public Dictionary<string, bool> Missions { get; set; } = new();

    public void DefineEmpty()
    {
        foreach (var expac in Service.StrikeData.Expansions)
        {
            Expansions.Add(expac.Id, true);
            foreach (var miss in expac.Missions)
            {
                Missions.Add(miss.Id, true);
            }
        }
    }

    public override void SetEncounterLabel(string encounterApiId, string label)
    {
        var storageKey = StorageKeyPrefixes.NormalizeStorageKey(encounterApiId);
        if (EncounterLabels.ContainsKey(storageKey))
            EncounterLabels.Remove(storageKey);
        EncounterLabels.Add(storageKey, label);

        Service.StrikesWindow.UpdateEncounterLabel(encounterApiId, label);
        Service.StrikesWindow.UpdateEncounterLabel(StorageKeyPrefixes.Priority + storageKey, label);
        Service.StrikesWindow.UpdateEncounterLabel(StorageKeyPrefixes.Tomorrow + storageKey, label);
        Save();
    }

    public SettingEntry<bool> GetPriorityVisible(ExpansionStrikes priority)
    {
        const string key = "priority";
        if (VirtualSettingsEnties.ContainsKey(key))
        {
            return VirtualSettingsEnties[key];
        }

        var setting = new SettingEntry<bool>()
        {
            Value = Priority,
            GetDescriptionFunc = () => $"",
            GetDisplayNameFunc = () => string.Format(Strings.StrikeSettings_EnablePriority, priority.Name)
        };
        setting.SettingChanged += (_, e) =>
        {
            Priority = e.NewValue;
            Save();
        };
        VirtualSettingsEnties.Add(key, setting);
        return setting;
    }
    public SettingEntry<bool> GetTomorrowBountiesVisible(ExpansionStrikes priority)
    {
        const string key = "priority_tomorrow";
        if (VirtualSettingsEnties.ContainsKey(key))
        {
            return VirtualSettingsEnties[key];
        }

        var setting = new SettingEntry<bool>()
        {
            Value = TomorrowBounties,
            GetDescriptionFunc = () => string.Empty,
            GetDisplayNameFunc = () => string.Format(Strings.StrikeSettings_EnablePriority, priority.Name)
        };
        setting.SettingChanged += (_, e) =>
        {
            TomorrowBounties = e.NewValue;
            Save();
        };
        VirtualSettingsEnties.Add(key, setting);
        return setting;
    }
    public SettingEntry<bool> GetExpansionVisible(ExpansionStrikes expac)
    {
        if (VirtualSettingsEnties.ContainsKey(expac.Id))
        {
            return VirtualSettingsEnties[expac.Id];
        }
        if (!Expansions.ContainsKey(expac.Id))
        {
            Expansions.Add(expac.Id, true);
            Save();
        }

        var setting = new SettingEntry<bool>()
        {
            Value = Expansions[expac.Id],
            GetDescriptionFunc = () => $"",
            GetDisplayNameFunc = () => string.Format(Strings.StrikeSettings_EnableExpansion, expac.Name)
        };
        setting.SettingChanged += (_, e) =>
        {
            Expansions[expac.Id] = e.NewValue;
            Save();
        };

        VirtualSettingsEnties.Add(expac.Id, setting);
        return setting;
    }
    public SettingEntry<bool> GetMissionVisible(BossEncounter mission)
    {
        var id = StorageKeyPrefixes.NormalizeStorageKey(mission.EncounterId);
        if (VirtualSettingsEnties.ContainsKey(id))
            return VirtualSettingsEnties[id];
        if (!Missions.ContainsKey(id))
        {
            Missions.Add(id, true);
            Save();
        }

        var setting = new SettingEntry<bool>()
        {
            Value = Missions[id],
            GetDescriptionFunc = () => "",
            GetDisplayNameFunc = () => mission.Name
        };
        setting.SettingChanged += (_, e) =>
        {
            Missions[id] = e.NewValue;
            Save();
        };
        VirtualSettingsEnties.Add(id, setting);
        return setting;
    }

    public override void Save()
    {
        var configFileInfo = GetConfigFileInfo();

        var serializedContents = JsonConvert.SerializeObject(this, Formatting.Indented);

        using var writer = new StreamWriter(configFileInfo.FullName, false, Encoding.UTF8);
        writer.Write(serializedContents);
        writer.Close();
        StrikeSettingsChanged?.Invoke(this, true);

    }

    private static FileInfo GetConfigFileInfo()
    {
        var pluginConfigDirectory = Service.DirectoriesManager.GetFullDirectoryPath(Module.DIRECTORY_PATH);

        return new FileInfo($@"{pluginConfigDirectory}\{FILENAME}");
    }

    public static StrikeSettingsPersistance Load()
    {
        if (GetConfigFileInfo() is { Exists: true } configFileInfo)
        {
            using var reader = new StreamReader(configFileInfo.FullName, Encoding.UTF8);
            var fileText = reader.ReadToEnd();
            reader.Close();

            return LoadExistingCharacterConfiguration(fileText);
        }
        else
        {
            return CreateNewCharacterConfiguration();
        }
    }

    private static StrikeSettingsPersistance LoadExistingCharacterConfiguration(string fileText)
    {
        var loadedCharacterConfiguration = JsonConvert.DeserializeObject<StrikeSettingsPersistance>(fileText);

        if (loadedCharacterConfiguration == null)
        {
            loadedCharacterConfiguration = new StrikeSettingsPersistance();
        }

        return HandleVersionUpgrade(loadedCharacterConfiguration);
    }

    private static StrikeSettingsPersistance HandleVersionUpgrade(StrikeSettingsPersistance data)
    {
        if (!SupportedVersions.Contains(data.Version))
            return new StrikeSettingsPersistance();
        if (data.Version == CURRENT_VERSION)
            return data;

        if (VersionsRequiringPriorityMigration.Contains(data.Version))
            MigratePriorityKeysFromStorage(data);
        data.Version = CURRENT_VERSION;
        data.Save();
        return data;
    }

    /// <summary>Merge priority_ keys into base keys and remove priority_ entries so JSON stays clean. Returns true if any change was made.</summary>
    private static bool MigratePriorityKeysFromStorage(StrikeSettingsPersistance data)
    {
        var changed = false;
        foreach (var key in data.EncounterLabels.Keys.ToList())
        {
            if (key == "priority" || key == "priority_tomorrow") continue;
            if (!key.StartsWith(StorageKeyPrefixes.Priority, StringComparison.Ordinal) && !key.StartsWith(StorageKeyPrefixes.Tomorrow, StringComparison.Ordinal)) continue;
            var baseKey = key.StartsWith(StorageKeyPrefixes.Priority, StringComparison.Ordinal) ? key.Substring(StorageKeyPrefixes.Priority.Length) : key.Substring(StorageKeyPrefixes.Tomorrow.Length);
            if (!data.EncounterLabels.ContainsKey(baseKey))
                data.EncounterLabels[baseKey] = data.EncounterLabels[key];
            data.EncounterLabels.Remove(key);
            changed = true;
        }
        foreach (var key in data.Missions.Keys.ToList())
        {
            if (key == "priority" || key == "priority_tomorrow") continue;
            if (!key.StartsWith(StorageKeyPrefixes.Priority, StringComparison.Ordinal) && !key.StartsWith(StorageKeyPrefixes.Tomorrow, StringComparison.Ordinal)) continue;
            var baseKey = key.StartsWith(StorageKeyPrefixes.Priority, StringComparison.Ordinal) ? key.Substring(StorageKeyPrefixes.Priority.Length) : key.Substring(StorageKeyPrefixes.Tomorrow.Length);
            if (!data.Missions.ContainsKey(baseKey))
                data.Missions[baseKey] = data.Missions[key];
            data.Missions.Remove(key);
            changed = true;
        }
        return changed;
    }

    private static StrikeSettingsPersistance CreateNewCharacterConfiguration()
    {
        var newCharacterConfiguration = new StrikeSettingsPersistance();
        newCharacterConfiguration.DefineEmpty();
        //Service.Settings.StrikeSettings.ConvertToJsonFile(newCharacterConfiguration);
        newCharacterConfiguration.Save();
        return newCharacterConfiguration;
    }
}