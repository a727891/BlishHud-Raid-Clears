using Blish_HUD.Settings;
using Newtonsoft.Json;
using RaidClears.Features.Shared.Models;
using RaidClears.Features.Strikes.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace RaidClears.Features.Strikes.Services;

[Serializable]
public class StrikeData
{
    [JsonIgnore]
    public static string FILENAME = "strike_data.json";
    [JsonIgnore]
    public static string FILE_URL = $"{Module.STATIC_HOST_URL}{Module.STATIC_HOST_API_VERSION}{FILENAME}";

    [JsonProperty("version")]
    public string Version { get; set; } = "";

    [JsonProperty("priority")]
    public ExpansionStrikes Priority { get; set; } = new();

    [JsonProperty("priority_tomorrow")]
    public ExpansionStrikes PriorityTomorrow { get; set; } = new();

    /// <summary>GW2 achievement ID for weekly raid encounters (strikes). When set, strike clears are synced from account achievement bits on API refresh.</summary>
    [JsonProperty("weekly_achievement_id")]
    public int WeeklyAchievementId { get; set; }

    /// <summary>Strike encounter ids in achievement bit order (bit 0 = first id, bit 1 = second, etc.). Used to map achievement bits to strike ids.</summary>
    [JsonProperty("weekly_achievement_bit_strike_ids")]
    public List<string> WeeklyAchievementBitStrikeIds { get; set; } = new();

    /// <summary>Strike encounter ids that are tracked by map change only (not in API). e.g. Dragonstorm (daily, no achievement).</summary>
    [JsonProperty("map_tracked_strike_ids")]
    public List<string> MapTrackedStrikeIds { get; set; } = new();

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

    public BossEncounter? GetBossEncounterByMapId(int id)
    {
        foreach (var expansion in Expansions)
        {
            foreach (var mission in expansion.Missions)
            {
                if (mission.MapIds.Contains(id))
                    return mission;
            }
        }
        return null;
    }

    public BossEncounter GetBossEncounterById(string name)
    {
        foreach (var expansion in Expansions)
        {
            if (expansion.Id == name)
                return expansion.ToBossEncounter();
            foreach (var mission in expansion.Missions)
            {
                if (mission.Id == name)
                    return mission;
            }
        }
        if (Priority.Id == name)
            return Priority.ToBossEncounter();
        if (PriorityTomorrow.Id == name)
            return PriorityTomorrow.ToBossEncounter();
        return new BossEncounter() { Id = name, Name = "GetBossEncounterById Failed" };
    }

    public BossEncounter GetBossEncounterByName(string name)
    {
        foreach (var expansion in Expansions)
        {
            foreach (var mission in expansion.Missions)
            {
                if (mission.Name == name)
                    return mission;
            }
        }
        return new BossEncounter() { Name = name };
    }
    public string GetStrikeMissionResetById(string name)
    {
        foreach (var expansion in Expansions)
        {
            foreach (var mission in expansion.Missions)
            {
                if (mission.Id == name)
                {
                    return !string.IsNullOrEmpty(mission.Resets) ? mission.Resets : expansion.Resets;
                }
            }
        }
        return "weekly";
    }

    /// <summary>True if this strike is tracked by map change only (not in weekly achievement API).</summary>
    public bool IsMapTracked(string encounterId)
    {
        return MapTrackedStrikeIds != null && MapTrackedStrikeIds.Contains(encounterId);
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
    public SettingEntry<bool> GetTomorrowBountiesVisible()
    {
        return Service.StrikeSettings.GetTomorrowBountiesVisible(PriorityTomorrow);
    }
    public SettingEntry<bool> GetExpansionVisible(ExpansionStrikes expac)
    {
        return Service.StrikeSettings.GetExpansionVisible(expac);
    }
    public SettingEntry<bool> GetMissionVisible(BossEncounter mission)
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

        using var writer = new StreamWriter(configFileInfo.FullName, false, Encoding.UTF8);
        writer.Write(serializedContents);
        writer.Close();

    }

    public static StrikeData Load()
    {
        if (GetConfigFileInfo() is { Exists: true } configFileInfo)
        {
            using var reader = new StreamReader(configFileInfo.FullName, Encoding.UTF8);
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
                webClient.Encoding = Encoding.UTF8;
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