using Blish_HUD.Settings;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using RaidClears.Features.Raids.Models;

namespace RaidClears.Features.Raids.Services;

[Serializable]
public class RaidData
{
    [JsonIgnore]
    public static string FILENAME = "raid_data.json";
    [JsonIgnore]
    public static string FILE_URL = "https://bhm.blishhud.com/Soeed.RaidClears/static/raid_data.json";

    [JsonProperty("version")]
    public string Version { get; set; } = "";

    [JsonProperty("secondsInWeek")]
    public int SecondsInWeek { get; set; } = -1;

    [JsonProperty("aerodrome")]
    public Aerodrome AeroDrome { get; set; } = new();

    [JsonProperty("expansions")]
    public List<ExpansionRaid> Expansions { get; set; } = new();


    public ExpansionRaid GetExpansionRaidsById(string id)
    {
        foreach(var expansion in Expansions)
        {
            if(expansion.Id == id) return expansion;
        }
        return new ExpansionRaid();
    }
    public ExpansionRaid GetExpansionRaidByName(string name)
    {
        foreach (var expansion in Expansions)
        {
            if (expansion.Name == name) return expansion;
        }
        return new ExpansionRaid();
    }

    public RaidWing GetRaidWingByMapId(int id)
    {
        foreach(var expansion in Expansions)
        {
            foreach(var wing in expansion.Wings){
                if (wing.MapId == id)  return wing;
            }
        }
        return new RaidWing();
    }

    public RaidWing GetRaidWingById(string id)
    {
        foreach(var expansion in Expansions)
        {
            foreach(var wing in expansion.Wings)
            {
                if(wing.Id == id) return wing;
            }
        }
        return new RaidWing();
    }
    public RaidWing GetRaidWingByZeroIndex(int idx)
    {
        foreach (var expansion in Expansions)
        {
            foreach (var wing in expansion.Wings)
            {
                if (wing.Number == (idx+1)) return wing;
            }
        }
        return new RaidWing();
    }
    public RaidWing GetRaidWingByIndex(int idx)
    {
        foreach (var expansion in Expansions)
        {
            foreach (var wing in expansion.Wings)
            {
                if (wing.Number == (idx)) return wing;
            }
        }
        return new RaidWing();
    }
    public RaidEncounter GetRaidEncounterByApiId(string apiId)
    {
        foreach (var expansion in Expansions)
        {
            foreach (var wing in expansion.Wings)
            {
                foreach(var enc in wing.Encounters)
                {
                    if(enc.ApiId == apiId) return enc;
                }
                if(wing.Id == apiId)
                {
                    return wing.ToRaidEncounter();
                }
            }
        }
        return new RaidEncounter();
    }


    private static FileInfo GetConfigFileInfo()
    {
        var pluginConfigDirectory = Service.DirectoriesManager.GetFullDirectoryPath(Module.DIRECTORY_PATH);

        return new FileInfo($@"{pluginConfigDirectory}\{FILENAME}");
    }

    public void Save()
    {
        var configFileInfo = GetConfigFileInfo();

        var serializedContents = JsonConvert.SerializeObject(this, Formatting.None);

        using var writer = new StreamWriter(configFileInfo.FullName);
        writer.Write(serializedContents);
        writer.Close();

    }

    public static RaidData Load()
    {
        if (GetConfigFileInfo() is { Exists: true } configFileInfo)
        {
            using var reader = new StreamReader(configFileInfo.FullName);
            var fileText = reader.ReadToEnd();
            reader.Close();

            return LoadFileFromCache(fileText);
        }
        else
        {
            return DownloadFile();
        }
    }

    private static RaidData LoadFileFromCache(string fileText)
    {
        var loadedCharacterConfiguration = JsonConvert.DeserializeObject<RaidData>(fileText);

        loadedCharacterConfiguration ??= new RaidData();

        return loadedCharacterConfiguration;
    }

    public static RaidData DownloadFile()
    {
        try
        {
            using var webClient = new System.Net.WebClient();
            var json = webClient.DownloadString(FILE_URL);

            RaidData? data = JsonConvert.DeserializeObject<RaidData>(json);

            if (data == null)
            {
                return new RaidData();
            }
            data.Save();
            return data;
        }
        catch (Exception r)
        {
            return new RaidData();
        }
    }
}