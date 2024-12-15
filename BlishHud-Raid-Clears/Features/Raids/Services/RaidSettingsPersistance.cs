using Blish_HUD.Settings;
using Newtonsoft.Json;
using RaidClears.Features.Raids.Models;
using System;
using System.Collections.Generic;
using System.IO;

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

    [JsonIgnore]
    protected Dictionary<string, SettingEntry<bool>> VirtualSettingsEnties = new();

    [JsonProperty("version")]
    public string Version { get; set; } = "1.0.0";


 
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
        if (EncounterLabels.ContainsKey(encounterApiId))
        {
            EncounterLabels.Remove(encounterApiId);
        }
        EncounterLabels.Add(encounterApiId, label);
        Service.RaidWindow.UpdateEncounterLabel(encounterApiId, label);
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
            GetDescriptionFunc = () => $"",
            GetDisplayNameFunc = () => $"Enable {expac.Name}"
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
            GetDescriptionFunc = () => $"Show {raidWing.Name} on the raid overlay",
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
    public SettingEntry<bool> GetEncounterVisible(RaidEncounter encounter)
    {
        if (VirtualSettingsEnties.ContainsKey(encounter.ApiId))
        {
            return VirtualSettingsEnties[encounter.ApiId];
        }
        if (!Encounters.ContainsKey(encounter.ApiId))
        {
            Encounters.Add(encounter.ApiId, true);
            Save();
        }

        var setting = new SettingEntry<bool>()
        {
            Value = Encounters[encounter.ApiId],
            GetDescriptionFunc = () => $"Show {encounter.Name} on the raid overlay",
            GetDisplayNameFunc = () => $"{encounter.Abbriviation}"
        };
        setting.SettingChanged += (_, e) =>
        {
            Encounters[encounter.ApiId] = e.NewValue;
            Save();
        };

        VirtualSettingsEnties.Add(encounter.ApiId, setting);
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

        using var writer = new StreamWriter(configFileInfo.FullName);
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
        if (data.Version == "1.0.0")
        {
            return data;
        }
        else
        {
            return new RaidSettingsPersistance();
        }
    }

    private static RaidSettingsPersistance CreateNewCharacterConfiguration()
    {
        var newCharacterConfiguration = new RaidSettingsPersistance();
        newCharacterConfiguration.DefineEmpty();
        Service.Settings.RaidSettings.ConvertToJsonFile(newCharacterConfiguration, Service.RaidData);
        newCharacterConfiguration.Save();
        return newCharacterConfiguration;
    }
}