using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using RaidClears.Features.Raids.Models;
using RaidClears.Features.Shared.Models;

namespace RaidClears.Features.Raids.Services;

[Serializable]
public class RaidData
{
    private static readonly HashSet<string> DefaultEventEncounterApiIds = new(StringComparer.OrdinalIgnoreCase)
    {
        "spirit_woods", "bandit_trio", "escort", "twisted_castle",
        "river_of_souls", "statues_of_grenth", "gate", "camp"
    };

    [JsonIgnore]
    public static string FILENAME = "raid_data.json";
    [JsonIgnore]
    public static string FILE_URL = $"{Module.STATIC_HOST_URL}{Module.STATIC_HOST_API_VERSION}{FILENAME}";

    [JsonProperty("version")]
    public string Version { get; set; } = "";

    [JsonProperty("secondsInWeek")]
    public int SecondsInWeek { get; set; } = -1;

    /// <summary>Api_ids of "event" encounters that players often prefer to skip (e.g. Spirit Woods, Escort).</summary>
    [JsonProperty("eventEncounterApiIds")]
    public List<string> EventEncounterApiIds { get; set; } = new();

    [JsonProperty("powerDamageAssetId")]
    public int PowerDamageAssetId { get; set; } = 993687;

    [JsonProperty("condiDamageAssetId")]
    public int CondiDamageAssetId { get; set; } = 156600;

    [JsonProperty("defianceAssetId")]
    public int DefianceAssetId { get; set; } = 0;

    [JsonProperty("mentorAssetId")]
    public int MentorAssetId { get; set; } = 155062;

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
    public bool IsEventEncounter(string apiId)
    {
        if (string.IsNullOrEmpty(apiId)) return false;
        var set = (EventEncounterApiIds != null && EventEncounterApiIds.Count > 0)
            ? new HashSet<string>(EventEncounterApiIds, StringComparer.OrdinalIgnoreCase)
            : DefaultEventEncounterApiIds;
        return set.Contains(apiId);
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
    public BossEncounter GetRaidEncounterByApiId(string apiId)
    {
        foreach (var expansion in Expansions)
        {
            foreach (var wing in expansion.Wings)
            {
                foreach (var enc in wing.Encounters)
                {
                    if (enc.ApiId == apiId) return enc;
                }
                if (wing.Id == apiId)
                    return wing.ToBossEncounter();
            }
        }
        var strike = Service.StrikeData.GetBossEncounterById(apiId);
        if (strike.Name != "undefined")
        {
            return new BossEncounter()
            {
                Abbriviation = strike.Abbriviation,
                ApiId = strike.Id,
                Id = strike.Id,
                AssetId = strike.AssetId,
                Name = strike.Name,
                MapIds = strike.MapIds != null && strike.MapIds.Count > 0 ? new List<int>(strike.MapIds) : new List<int>(),
                DailyBountyAchievementId = strike.DailyBountyAchievementId,
            };
        }
        return new BossEncounter() { Abbriviation = apiId };
    }

    /// <summary>
    /// Gets the raid encounter that has the given mentor achievement ID, or null if not found.
    /// </summary>
    public BossEncounter? GetEncounterByMentorAchievementId(int mentorAchievementId)
    {
        foreach (var expansion in Expansions)
        {
            foreach (var wing in expansion.Wings)
            {
                foreach (var enc in wing.Encounters)
                {
                    if (enc.MentorAchievementId == mentorAchievementId)
                        return enc;
                }
            }
        }
        return null;
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

    public static RaidData Load()
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
            webClient.Encoding = Encoding.UTF8;
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