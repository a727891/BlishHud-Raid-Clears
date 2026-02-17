using Blish_HUD.Settings;
using Newtonsoft.Json;
using RaidClears.Features.Raids.Models;
using RaidClears.Features.Shared;
using RaidClears.Features.Shared.Models;
using RaidClears.Localization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace RaidClears.Features.Raids.Services;


[Serializable]
public class RaidSettingsPersistance : Labelable
{
    public RaidSettingsPersistance()
    {
        _isRaid = true;
    }

    public event EventHandler<bool>? RaidSettingsChanged;

    [JsonIgnore]
    public static string FILENAME = "raid_settings.json";

    private const string CURRENT_VERSION = "3.5.0";

    /// <summary>Versions we accept; unknown versions cause a fresh config.</summary>
    private static readonly HashSet<string> SupportedVersions = new(StringComparer.Ordinal)
    {
        "1.0.0", CURRENT_VERSION
    };

    /// <summary>Legacy versions that need MigratePriorityKeysFromStorage before upgrading to CURRENT_VERSION.</summary>
    private static readonly HashSet<string> VersionsRequiringPriorityMigration = new(StringComparer.Ordinal)
    {
        "1.0.0"
    };

    [JsonIgnore]
    protected Dictionary<string, SettingEntry<bool>> VirtualSettingsEnties = new();

    [JsonProperty("version")]
    public string Version { get; set; } = CURRENT_VERSION;


 
    [JsonProperty("expansions")]
    public Dictionary<string, bool> Expansions { get; set; } = new();

    [JsonProperty("wings")]
    public Dictionary<string, bool> Wings { get; set; } = new();

    [JsonProperty("encounters")]
    public Dictionary<string, bool> Encounters { get; set; } = new();



    public void DefineEmpty()
    {
        foreach (var expac in Service.RaidData.Expansions)
        {
            Expansions.Add(expac.Id, true);
            foreach (var wing in expac.Wings)
            {
                Wings.Add(wing.Id, true);
                foreach(var encounter in wing.Encounters)
                {
                    Encounters.Add(encounter.ApiId, true);
                }
            }
        }
    }

    public override void SetEncounterLabel(string encounterApiId, string label)
    {
        var storageKey = StorageKeyPrefixes.NormalizeStorageKey(encounterApiId);
        if (EncounterLabels.ContainsKey(storageKey))
            EncounterLabels.Remove(storageKey);
        EncounterLabels.Add(storageKey, label);

        Service.RaidWindow.UpdateEncounterLabel(encounterApiId, label);
        Service.StrikesWindow.UpdateEncounterLabel(StorageKeyPrefixes.Priority + storageKey, label);
        Service.StrikesWindow.UpdateEncounterLabel(StorageKeyPrefixes.Tomorrow + storageKey, label);
        Save();
    }

    public SettingEntry<bool> GetExpansionVisible(ExpansionRaid expac)
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
            GetDescriptionFunc = () => string.Format(Strings.Settings_Raid_ExpansionVisible_Description, expac.Name),
            GetDisplayNameFunc = () => string.Format(Strings.Settings_Raid_EnableExpansion, expac.Name)
        };
        setting.SettingChanged += (_, e) =>
        {
            Expansions[expac.Id] = e.NewValue;
            foreach(var wing in expac.Wings)
            {
                GetWingVisible(wing).Value = e.NewValue;
            }
            Save();
        };

        VirtualSettingsEnties.Add(expac.Id, setting);
        return setting;
    }
    public SettingEntry<bool> GetWingVisible(RaidWing raidWing)
    {
        if (VirtualSettingsEnties.ContainsKey(raidWing.Id))
        {
            return VirtualSettingsEnties[raidWing.Id];
        }
        if (!Wings.ContainsKey(raidWing.Id))
        {
            Wings.Add(raidWing.Id, true);
            Save();
        }

        var setting = new SettingEntry<bool>()
        {
            Value = Wings[raidWing.Id],
            GetDescriptionFunc = () => string.Format(Strings.Settings_Raid_WingVisible_Description, raidWing.Name),
            GetDisplayNameFunc = () => $"{raidWing.Name}"
        };
        setting.SettingChanged += (_, e) =>
        {
            Wings[raidWing.Id] = e.NewValue;
            Save();
        };

        VirtualSettingsEnties.Add(raidWing.Id, setting);
        return setting;
    }
    public SettingEntry<bool> GetEncounterVisible(BossEncounter encounter)
    {
        var id = StorageKeyPrefixes.NormalizeStorageKey(encounter.EncounterId);
        if (VirtualSettingsEnties.ContainsKey(id))
            return VirtualSettingsEnties[id];
        if (!Encounters.ContainsKey(id))
        {
            Encounters.Add(id, true);
            Save();
        }

        var setting = new SettingEntry<bool>()
        {
            Value = Encounters[id],
            GetDescriptionFunc = () => string.Format(Strings.Settings_Raid_EncounterVisible_Description, encounter.Name),
            GetDisplayNameFunc = () => encounter.Abbriviation
        };
        setting.SettingChanged += (_, e) =>
        {
            Encounters[id] = e.NewValue;
            Save();
        };
        VirtualSettingsEnties.Add(id, setting);
        return setting;
    }

    public SettingEntry<bool>? GetEncounterVisibleByApiId(string apiId)
    {
       foreach(var expansion in Service.RaidData.Expansions)
       {
            foreach(var wing in expansion.Wings)
            {
                foreach(var encounter in wing.Encounters)
                {
                    if(encounter.ApiId == apiId)
                    {
                        return GetEncounterVisible(encounter);
                    }
                }
            }
        }
        return null;

    }


    

    public override void Save()
    {
        var configFileInfo = GetConfigFileInfo();

        var serializedContents = JsonConvert.SerializeObject(this, Formatting.Indented);

        using var writer = new StreamWriter(configFileInfo.FullName, false, Encoding.UTF8);
        writer.Write(serializedContents);
        writer.Close();
        RaidSettingsChanged?.Invoke(this, true);

    }

    private static FileInfo GetConfigFileInfo()
    {
        var pluginConfigDirectory = Service.DirectoriesManager.GetFullDirectoryPath(Module.DIRECTORY_PATH);

        return new FileInfo($@"{pluginConfigDirectory}\{FILENAME}");
    }

    public static RaidSettingsPersistance Load()
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

    private static RaidSettingsPersistance LoadExistingCharacterConfiguration(string fileText)
    {
        var loadedCharacterConfiguration = JsonConvert.DeserializeObject<RaidSettingsPersistance>(fileText);

        if (loadedCharacterConfiguration == null)
        {
            loadedCharacterConfiguration = new RaidSettingsPersistance();
        }

        return HandleVersionUpgrade(loadedCharacterConfiguration);
    }

    private static RaidSettingsPersistance HandleVersionUpgrade(RaidSettingsPersistance data)
    {
        if (!SupportedVersions.Contains(data.Version))
            return new RaidSettingsPersistance();
        if (data.Version == CURRENT_VERSION)
            return data;

        if (VersionsRequiringPriorityMigration.Contains(data.Version))
            MigratePriorityKeysFromStorage(data);
        data.Version = CURRENT_VERSION;
        data.Save();
        return data;
    }

    /// <summary>Merge priority_ keys into base keys and remove priority_ entries so JSON stays clean. Returns true if any change was made.</summary>
    private static bool MigratePriorityKeysFromStorage(RaidSettingsPersistance data)
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
        foreach (var key in data.Encounters.Keys.ToList())
        {
            if (key == "priority" || key == "priority_tomorrow") continue;
            if (!key.StartsWith(StorageKeyPrefixes.Priority, StringComparison.Ordinal) && !key.StartsWith(StorageKeyPrefixes.Tomorrow, StringComparison.Ordinal)) continue;
            var baseKey = key.StartsWith(StorageKeyPrefixes.Priority, StringComparison.Ordinal) ? key.Substring(StorageKeyPrefixes.Priority.Length) : key.Substring(StorageKeyPrefixes.Tomorrow.Length);
            if (!data.Encounters.ContainsKey(baseKey))
                data.Encounters[baseKey] = data.Encounters[key];
            data.Encounters.Remove(key);
            changed = true;
        }
        return changed;
    }

    private static RaidSettingsPersistance CreateNewCharacterConfiguration()
    {
        var newCharacterConfiguration = new RaidSettingsPersistance();
        newCharacterConfiguration.DefineEmpty();
        //Service.Settings.RaidSettings.ConvertToJsonFile(newCharacterConfiguration, Service.RaidData);
        newCharacterConfiguration.Save();
        return newCharacterConfiguration;
    }
}