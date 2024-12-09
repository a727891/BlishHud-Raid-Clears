using Blish_HUD.Settings;
using Newtonsoft.Json;
using RaidClears.Features.Shared.Enums;
using System;
using System.Collections.Generic;
using System.IO;

namespace RaidClears.Features.Strikes.Services;


[Serializable]
public class StrikeSettingsPersistance
{
    public event EventHandler<bool>? StrikeSettingsChanged;

    [JsonIgnore]
    public static string FILENAME = "strike_settings.json";

    [JsonIgnore]
    protected Dictionary<string, SettingEntry<bool>> VirtualSettingsEnties = new();

    [JsonProperty("version")]
    public string Version { get; set; } = "3.0.0";



    [JsonProperty("priority")]
    public bool Priority { get; set; } = true;


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

    public SettingEntry<bool> GetPriorityVisible(ExpansionStrikes priority)
    {
        if (VirtualSettingsEnties.ContainsKey("priority"))
        {
            return VirtualSettingsEnties["priority"];
        }

        var setting = new SettingEntry<bool>()
        {
            Value = Priority,
            GetDescriptionFunc = () => $"",
            GetDisplayNameFunc = () => $"Enable {priority.Name}"
        };
        setting.SettingChanged += (_, e) =>
        {
            Priority = e.NewValue;
            Save();
        };
        VirtualSettingsEnties.Add("priority", setting);
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
            GetDisplayNameFunc = () => $"Enable {expac.Name}"
        };
        setting.SettingChanged += (_, e) =>
        {
            Expansions[expac.Id] = e.NewValue;
            Save();
        };

        VirtualSettingsEnties.Add(expac.Id, setting);
        return setting;
    }
    public SettingEntry<bool> GetMissionVisible(StrikeMission mission)
    {
        if (VirtualSettingsEnties.ContainsKey(mission.Id))
        {
            return VirtualSettingsEnties[mission.Id];
        }
        if (!Missions.ContainsKey(mission.Id))
        {
            Missions.Add(mission.Id, true);
            Save();
        }

        var setting = new SettingEntry<bool>()
        {
            Value = Missions[mission.Id],
            GetDescriptionFunc = () => $"",
            GetDisplayNameFunc = () => $"{mission.Name}"
        };
        setting.SettingChanged += (_, e) =>
        {
            Missions[mission.Id] = e.NewValue;
            Save();
        };

        VirtualSettingsEnties.Add(mission.Id, setting);
        return setting;
    }

    public void Save()
    {
        var configFileInfo = GetConfigFileInfo();

        var serializedContents = JsonConvert.SerializeObject(this, Formatting.Indented);

        using var writer = new StreamWriter(configFileInfo.FullName);
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
            using var reader = new StreamReader(configFileInfo.FullName);
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
        if (data.Version == "3.0.0")
        {
            return data;
        }
        else
        {
            return new StrikeSettingsPersistance();
        }
    }

    private static StrikeSettingsPersistance CreateNewCharacterConfiguration()
    {
        var newCharacterConfiguration = new StrikeSettingsPersistance();
        newCharacterConfiguration.DefineEmpty();
        Service.Settings.StrikeSettings.ConvertToJsonFile(newCharacterConfiguration);
        newCharacterConfiguration.Save();
        return newCharacterConfiguration;
    }
}