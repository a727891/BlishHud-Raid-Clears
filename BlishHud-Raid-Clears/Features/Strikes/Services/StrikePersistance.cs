using Newtonsoft.Json;
using RaidClears.Features.Shared.Enums;
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
    public string Version { get; set; } = "2.0.1";

    [JsonProperty("accountClears")]
    public Dictionary<string, Dictionary<Encounters.StrikeMission, DateTime>> AccountClears { get; set; } = new();

    public Dictionary<Encounters.StrikeMission, DateTime> GetEmpty() => new Dictionary<Encounters.StrikeMission, DateTime>
                {
                    { Encounters.StrikeMission.ColdWar,new()},
                    { Encounters.StrikeMission.Fraenir, new()},
                    { Encounters.StrikeMission.ShiverpeaksPass, new() },
                    { Encounters.StrikeMission.VoiceAndClaw, new() },
                    { Encounters.StrikeMission.Whisper, new() },
                    { Encounters.StrikeMission.Boneskinner, new() },
                    { Encounters.StrikeMission.AetherbladeHideout, new() },
                    { Encounters.StrikeMission.Junkyard, new() },
                    { Encounters.StrikeMission.Overlook, new() },
                    { Encounters.StrikeMission.HarvestTemple, new() },
                    { Encounters.StrikeMission.OldLionsCourt, new() },
                    { Encounters.StrikeMission.DragonStorm, new() },
                };

    public void SaveClear(string account, Encounters.StrikeMission mission)
    {
        Dictionary<Encounters.StrikeMission, DateTime> clears;
        if (!AccountClears.TryGetValue(account, out clears))
        {
            clears = GetEmpty();
            AccountClears.Add(account, clears);
            // the key isn't in the dictionary.
        }

        clears[mission] = DateTime.UtcNow;
        AccountClears[account]= clears;
        Save();

    }
    public void RemoveClear(string account, Encounters.StrikeMission mission)
    {
        Dictionary<Encounters.StrikeMission, DateTime> clears;
        if (!AccountClears.TryGetValue(account, out clears))
        {
            clears = GetEmpty();
            AccountClears.Add(account, clears);
            // the key isn't in the dictionary.
        }

        clears[mission] = new DateTime();
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

        return loadedCharacterConfiguration;
    }

    private static StrikePersistance CreateNewCharacterConfiguration()
    {
        var newCharacterConfiguration = new StrikePersistance();
        newCharacterConfiguration.AccountClears.Add("test", newCharacterConfiguration.GetEmpty());
        newCharacterConfiguration.Save();
        return newCharacterConfiguration;
    }
}