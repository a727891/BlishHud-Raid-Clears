using Newtonsoft.Json;
using RaidClears.Features.Fractals.Models;
using RaidClears.Features.Shared.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;

namespace RaidClears.Features.Fractals.Services;

public class FractalMap
{
    [JsonProperty("label")]
    public string Label = "undefined";

    [JsonProperty("short")]
    public string ShortLabel = "undefined";

    [JsonProperty("api")]
    public string ApiLabel = "undefined";

    [JsonProperty("scales")]
    public List<int> Scales = new List<int>();

    [JsonProperty("id")]
    public int MapId = 0;

    public EncounterInterface ToEncounterInterface()
    {
        return new EncounterInterface()
        {
            Id = ApiLabel,
            Name = Label,
            Abbriviation = ShortLabel
        };
    }
}

[Serializable]
public class FractalMapData
{
    [JsonIgnore]
    public static string FILENAME = "fractal_maps.json";
    [JsonIgnore]
    public static string FILE_URL = $"{Module.STATIC_HOST_URL}/fractal_maps.json";

    [JsonProperty("DailyTier")]
    public List<List<string>> DailyTier { get; set; } = new();

    [JsonProperty("Recs")]
    public List<List<int>> Recs { get; set; } = new ();

    [JsonProperty("maps")]
    public Dictionary<string, FractalMap> Maps { get; set; } = new();

    [JsonIgnore]
    public List<EncounterInterface> Categories { get; set; } = new(){
        new()
        {
            Name=Fractal.ChallengeMoteLabel,
            Id=Fractal.ChallengeMoteId,
            Abbriviation=Fractal.ChallengeMoteId
        },
        new()
        {
            Name=Fractal.TomorrowLabel,
            Id=Fractal.TomorrowId,
            Abbriviation=Fractal.TomorrowId
        },
        new()
        {
            Name=Fractal.TierNLabel,
            Id=Fractal.TierNId,
            Abbriviation=Fractal.TierNId
        },
        new()
        {
            Name=Fractal.RecLabel,
            Id=Fractal.RecId,
            Abbriviation=Fractal.RecId
        },
    };

        
    [JsonProperty("challengeMotes")]
    public int[] ChallengeMotes { get; set; } = new int[] { };

    [JsonProperty("scales")]
    public Dictionary<string, string> Scales { get; set; } = new();

    [JsonProperty("instabilityAssets")]
    public Dictionary<string, int> InstabilityAssets = new();

    private List<int>? _mapIds = null;


    public int GetInstabilityAssetIdByName(string name)
    {
        if (InstabilityAssets.TryGetValue(name, out var assetId))
        {
            return assetId;
        }
        return 0;
    }
    public List<int> GetInstabilityAssetIdByNames(List<string> names)
    {
        
        return names.Select(GetInstabilityAssetIdByName).ToList();
        
    }

    public FractalMap GetFractalForScale(int scale)
    {
        if (!Scales.ContainsKey(scale.ToString()))
        {
            return new FractalMap();
        }
        var mapName = Scales[scale.ToString()];
        if (Maps.ContainsKey(mapName))
        {
            return Maps[mapName];
        }
        return new FractalMap();

    }

    public FractalMap GetFractalByName(string name)
    {
        if(Maps.ContainsKey(name)) return Maps[name];
        foreach(var map in Maps.Values)
        {
            if(map.ApiLabel== name) return map;
            if(map.Label== name) return map;
        }
        return new FractalMap();
    }
    public FractalMap GetFractalByApiName(string name)
    {
        foreach (var map in Maps.Values)
        {
            if (map.ApiLabel == name) return map;
        }
        return new FractalMap() { 
            Label= name,
            ShortLabel= name,
            ApiLabel = name,
        };

    }

    public FractalMap? GetFractalMapById(int mapId)
    {
        foreach (var map in Maps.Values)
        {
            if (map.MapId == mapId)
            {
                return map;
            }
        }
        return null;
    }
    public List<int> GetFractalMapIds()
    {
        if(_mapIds is null)
        {
            _mapIds = new List<int> ();
            foreach(var map in Maps)
            {
                _mapIds.Add(map.Value.MapId);
            }
        }

        return _mapIds;
    }

    private static FileInfo GetConfigFileInfo()
    {
        var pluginConfigDirectory = Service.DirectoriesManager.GetFullDirectoryPath(Module.DIRECTORY_PATH);

        return new FileInfo($@"{pluginConfigDirectory}\{FILENAME}");
    }

    public void Save()
    {
        //PluginLog.Verbose($"{DateTime.Now} - {CharacterData.Name} Saved");

        var configFileInfo = GetConfigFileInfo();

        var serializedContents = JsonConvert.SerializeObject(this, Formatting.None);

        using var writer = new StreamWriter(configFileInfo.FullName);
        writer.Write(serializedContents);
        writer.Close();

        //PluginLog.Warning("Tried to save a config with invalid LocalContentID, aborting save.");

    }

    public static FractalMapData Load()
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

    private static FractalMapData LoadFileFromCache(string fileText)
    {
        var loadedCharacterConfiguration = JsonConvert.DeserializeObject<FractalMapData>(fileText);

        if (loadedCharacterConfiguration == null)
        {
            loadedCharacterConfiguration = new FractalMapData();
        }

        return loadedCharacterConfiguration;
    }

    public static FractalMapData DownloadFile()
    {
        try
        {
            using (var webClient = new System.Net.WebClient())
            {
                var json = webClient.DownloadString(FILE_URL);

                FractalMapData? data = JsonConvert.DeserializeObject<FractalMapData>(json);

                if (data == null)
                {
                    return new FractalMapData();
                }
                data.Save();
                return data;
            }
        }
        catch (Exception r)
        {
            return new FractalMapData();
        }
    }
}