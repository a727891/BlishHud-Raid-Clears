﻿using Blish_HUD.Settings;
using Newtonsoft.Json;
using RaidClears.Features.Strikes.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace RaidClears.Features.Strikes.Services;

[Serializable]
public class StrikeData
{
    [JsonIgnore]
    public static string FILENAME = "strike_data.json";
    [JsonIgnore]
    public static string FILE_URL = $"{Module.STATIC_HOST_URL}/strike_data.json";

    [JsonProperty("version")]
    public string Version { get; set; } = "";

    [JsonProperty("priority")]
    public ExpansionStrikes Priority { get; set; } = new();


    [JsonProperty("expansions")]
    public List<ExpansionStrikes> Expansions { get; set; } = new ();

    public ExpansionStrikes GetExpansionStrikesById(string id)
    {
        foreach(var expansion in Expansions)
        {
            if(expansion.Id == id) return expansion;
        }
        return new ExpansionStrikes();
    }
    public ExpansionStrikes GetExpansionStrikesByName(string name)
    {
        foreach (var expansion in Expansions)
        {
            if (expansion.Name == name) return expansion;
        }
        return new ExpansionStrikes()
        {
            Name = name
        };
    }

    public StrikeMission? GetStrikeMisisonByMapId(int id)
    {
        foreach(var expansion in Expansions)
        {
            foreach(var mission in expansion.Missions){
                if (mission.MapIds.Contains(id))
                {
                    return mission;
                }
            }
        }
        return null;
    }

    public StrikeMission GetStrikeMissionById(string name)
    {
        foreach(var expansion in Expansions)
        {
            if (expansion.Id == name)
            {
                return expansion.ToStrikeMission();
            }
            foreach(var mission in expansion.Missions)
            {
                if(mission.Id == name)
                {
                    return mission;
                }
            }
        }
        return new StrikeMission()
        {
            Id = name
        };
    }
    public StrikeMission GetStrikeMissionByName(string name)
    {
        foreach (var expansion in Expansions)
        {
            if (expansion.Id == name)
            {

            }
            foreach (var mission in expansion.Missions)
            {
                if (mission.Name == name)
                {
                    return mission;
                }
            }
        }
        return new StrikeMission()
        {
            Name = name
        };
    }
    public string GetStrikeMissionResetById(string name)
    {
        foreach (var expansion in Expansions)
        {
            foreach (var mission in expansion.Missions)
            {
                if (mission.Id == name)
                {
                    return expansion.Resets;
                }
            }
        }
        return "weekly";
    }

    public List<StrikeInfo> GetPriorityStrikes(int index)
    {
        List<StrikeInfo> list = new ();
        foreach(var expansion in Expansions)
        {
            var todayIndex = (index+expansion.DailyPriorityOffset) % expansion.DailyPriorityModulo;
            var tomorrowIndex = (index+expansion.DailyPriorityOffset+1) % expansion.DailyPriorityModulo;
            if(expansion.Missions.Count() >= todayIndex 
                && expansion.Missions.Count() >= tomorrowIndex)
            {
                list.Add(
                    new StrikeInfo(
                        expansion.Missions[todayIndex],
                        expansion.Missions[tomorrowIndex]
                    )
                ); 
            }
        }

        return list;

    }
    public SettingEntry<bool> GetPriorityVisible()
    {
        return Service.StrikeSettings.GetPriorityVisible(Priority);
    }
    public SettingEntry<bool> GetExpansionVisible(ExpansionStrikes expac)
    {
        return Service.StrikeSettings.GetExpansionVisible(expac);
    }
    public SettingEntry<bool> GetMissionVisible(StrikeMission mission)
    {
        return Service.StrikeSettings.GetMissionVisible(mission);
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

    public static StrikeData Load()
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

    private static StrikeData LoadFileFromCache(string fileText)
    {
        var loadedCharacterConfiguration = JsonConvert.DeserializeObject<StrikeData>(fileText);

        if (loadedCharacterConfiguration == null)
        {
            loadedCharacterConfiguration = new StrikeData();
        }

        return loadedCharacterConfiguration;
    }

    public static StrikeData DownloadFile()
    {
        try
        {
            using (var webClient = new System.Net.WebClient())
            {
                var json = webClient.DownloadString(FILE_URL);

                StrikeData? data = JsonConvert.DeserializeObject<StrikeData>(json);

                if (data == null)
                {
                    return new StrikeData();
                }
                data.Save();
                return data;
            }
        }
        catch (Exception r)
        {
            return new StrikeData();
        }
    }
}