using Newtonsoft.Json;
using RaidClears.Features.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;

namespace RaidClears.Features.Fractals.Services;


[Serializable]
public class FractalPersistance
{
    [JsonIgnore]
    public static string FILENAME = "fractal_clears.json";

    [JsonProperty("version")]
    public string Version { get; set; } = "3.0.0";

    [JsonProperty("accountClears")]
    public Dictionary<string, Dictionary<string, DateTime>> AccountClears { get; set; } = new();

    public Dictionary<string, DateTime> GetEmpty() {
        var empty = new Dictionary<string, DateTime>
                {
                    //{ Encounters.Fractal.MistlockObservatory, new() },
                };
        foreach(var fractal in Service.FractalMapData.Maps)
        {
            empty.Add(fractal.Value.ApiLabel, new());
        }
        return empty;
    }
    public void SaveClear(string account, FractalMap mission)
    {
        Dictionary<string, DateTime> clears;
        if (!AccountClears.TryGetValue(account, out clears))
        {
            clears = GetEmpty();
            AccountClears.Add(account, clears);
            // the key isn't in the dictionary.
        }
        if(!clears.ContainsKey(mission.ApiLabel))
        {
            clears.Add(mission.ApiLabel, new());
        }
        clears[mission.ApiLabel] = DateTime.UtcNow;
        AccountClears[account] = clears;
        Save();

    }

    public void RemoveClear(string account, FractalMap mission)
    {
        Dictionary<string, DateTime> clears;
        if (!AccountClears.TryGetValue(account, out clears))
        {
            clears = GetEmpty();
            AccountClears.Add(account, clears);
            // the key isn't in the dictionary.
        }
        if (!clears.ContainsKey(mission.ApiLabel))
        {
            clears.Add(mission.ApiLabel, new());
        }

        clears[mission.ApiLabel] = new DateTime();
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

    public static FractalPersistance Load()
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

    private static FractalPersistance LoadExistingCharacterConfiguration(string fileText)
    {
        var loadedCharacterConfiguration = JsonConvert.DeserializeObject<FractalPersistance>(fileText);

        if (loadedCharacterConfiguration == null)
        {
            loadedCharacterConfiguration = new FractalPersistance();
        }

        return HandleVersionUpgrade(loadedCharacterConfiguration);
    }

    private static FractalPersistance HandleVersionUpgrade(FractalPersistance data)
    {
        if (data.Version == "2.0.0" || data.Version=="2.2.0")
        {
            FractalPersistance newSaveFile = new FractalPersistance();
            //Rename keys to remove "fractal"
            foreach(var account in data.AccountClears)
            {
                Dictionary<string,DateTime> acctClears = new Dictionary<string,DateTime>();
                foreach(var line in account.Value)
                {
                    var frac = Service.FractalMapData.GetFractalByName(line.Key.Replace("Fractal",""));
                    if(frac.ApiLabel != "undefined")
                        acctClears.Add(frac.ApiLabel, line.Value);
                }
                newSaveFile.AccountClears.Add(account.Key, acctClears);
            }
            newSaveFile.Save();
            return newSaveFile;
        }
        else if(data.Version == "3.0.0")
        {
            return data;
        }
        else
        {
            return new FractalPersistance();
        }

    }

    private static FractalPersistance CreateNewCharacterConfiguration()
    {
        var newCharacterConfiguration = new FractalPersistance();
        newCharacterConfiguration.AccountClears.Add("default", newCharacterConfiguration.GetEmpty());
        newCharacterConfiguration.Save();
        return newCharacterConfiguration;
    }
}