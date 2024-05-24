using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace RaidClears.Features.Strikes.Services;


[Serializable]
public class StrikePersistance
{
    [JsonIgnore]
    public static string FILENAME = "strike_clears.json";

    [JsonProperty("version")]
    public string Version { get; set; } = "3.0.0";

    [JsonProperty("accountClears")]
    public Dictionary<string, Dictionary<string, DateTime>> AccountClears { get; set; } = new();

    public Dictionary<string, DateTime> GetEmpty()
    {
        Dictionary<string, DateTime> list = new Dictionary<string, DateTime>();
        foreach (var expac in Service.StrikeData.Expansions)
        {
            foreach (var miss in expac.Missions)
            {
                list.Add(miss.Id, new());
            }
        }
        return list;
    }

    public void SaveClear(string account, StrikeMission mission)
    {
        Dictionary<string, DateTime> clears;
        if (!AccountClears.TryGetValue(account, out clears))
        {
            clears = GetEmpty();
            AccountClears.Add(account, clears);
            // the key isn't in the dictionary.
        }
        if (!clears.ContainsKey(mission.Id))
        {
            clears.Add(mission.Id, new());
        }

        clears[mission.Id] = DateTime.UtcNow;
        AccountClears[account]= clears;
        Save();

    }
    public void RemoveClear(string account, StrikeMission mission)
    {
        Dictionary<string, DateTime> clears;
        if (!AccountClears.TryGetValue(account, out clears))
        {
            clears = GetEmpty();
            AccountClears.Add(account, clears);
            // the key isn't in the dictionary.
        }
        if (!clears.ContainsKey(mission.Id))
        {
            clears.Add(mission.Id, new());
        }

        clears[mission.Id] = new DateTime();
        AccountClears[account] = clears;
        Save();
    }


    public void Save()
    {
        //PluginLog.Verbose($"{DateTime.Now} - {CharacterData.Name} Saved");

        var configFileInfo = GetConfigFileInfo();

        var serializedContents = JsonConvert.SerializeObject(this, Formatting.Indented);

        using var writer = new StreamWriter(configFileInfo.FullName);
        writer.Write(serializedContents);
        writer.Close();

        //PluginLog.Warning("Tried to save a config with invalid LocalContentID, aborting save.");

    }

    private static FileInfo GetConfigFileInfo()
    {
        var pluginConfigDirectory = Service.DirectoriesManager.GetFullDirectoryPath(Module.DIRECTORY_PATH);

        return new FileInfo($@"{pluginConfigDirectory}\{FILENAME}");
    }

    public static StrikePersistance Load()
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

    private static StrikePersistance LoadExistingCharacterConfiguration(string fileText)
    {
        var loadedCharacterConfiguration = JsonConvert.DeserializeObject<StrikePersistance>(fileText);

        if (loadedCharacterConfiguration == null)
        {
            loadedCharacterConfiguration = new StrikePersistance();
        }

        return HandleVersionUpgrade(loadedCharacterConfiguration);
    }

    private static StrikePersistance HandleVersionUpgrade(StrikePersistance data)
    {
        if (data.Version == "2.0.0" || data.Version == "2.0.1")
        {
            Dictionary<string, string> keyRename = new()
            {
                { "ColdWar", "cold_war" },
                { "Fraenir", "fraenir_of_jormag" },
                { "ShiverpeaksPass", "shiverpeak_pass" },
                { "VoiceAndClaw", "voice_and_claw" },
                { "Whisper", "whisper_of_jormag" },
                { "Boneskinner", "boneskinner" },
                { "AetherbladeHideout", "aetherblade_hideout" },
                { "Junkyard", "xunlai_jade_junkyard" },
                { "Overlook", "kaineng_overlook" },
                { "HarvestTemple", "harvest_temple" },
                { "OldLionsCourt", "old_lion_court" },
                { "DragonStorm", "dragonstorm" },
                { "CosmicObservatory", "cosmic_observatory" },
                { "TempleOfFebe", "temple_of_febe" },
            };

            var newFile = new StrikePersistance();
            foreach(var account in data.AccountClears)
            {
                var acctclears = newFile.GetEmpty();
                foreach(var clear in account.Value)
                {
                    if(keyRename.TryGetValue(clear.Key, out string renameTo))
                    {
                        if (acctclears.ContainsKey(renameTo))
                        {
                            acctclears[renameTo] = clear.Value;
                        }
                        else
                        {
                            acctclears.Add(renameTo, clear.Value);
                        }
                    }
                }
                newFile.AccountClears.Add(account.Key, acctclears);
            }
            newFile.Save();

            return newFile;
        }
        else if (data.Version == "3.0.0")
        {
            return data;
        }
        else
        {
            return new StrikePersistance();
        }

    }

    private static StrikePersistance CreateNewCharacterConfiguration()
    {
        var newCharacterConfiguration = new StrikePersistance();
        newCharacterConfiguration.AccountClears.Add("default", newCharacterConfiguration.GetEmpty());
        newCharacterConfiguration.Save();
        return newCharacterConfiguration;
    }
}