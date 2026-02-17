using Newtonsoft.Json;
using RaidClears.Features.Raids.Models;
using RaidClears.Features.Shared.Models;
using System;
using System.Collections.Generic;

namespace RaidClears.Features.Strikes.Models;

[Serializable]
public class ExpansionStrikes : EncounterInterface, IExpansion<BossEncounter>
{
    [JsonProperty("asset")]
    public string asset = "missing.png";

    [JsonIgnore]
    public string Asset => asset;

    [JsonProperty("resets")]
    public string Resets = "weekly";

    [JsonProperty("daily_priority_modulo")]
    public int DailyPriorityModulo = 1;

    [JsonProperty("daily_priority_offset")]
    public int DailyPriorityOffset = 0;

    [JsonProperty("missions")]
    public List<BossEncounter> Missions = new();

    [JsonIgnore]
    public IReadOnlyList<BossEncounter> Children => Missions;

    // IExpansion Id is backed by EncounterInterface.Id
    string IExpansion<BossEncounter>.Id => Id;

    public List<BoxModel> GetEncounters()
    {
        List<BoxModel> missionslist = new();
        foreach (var mission in Missions)
        {
            missionslist.Add(new Encounter(mission, isStrike: true));
        }
        return missionslist;
    }

    public BossEncounter ToBossEncounter()
    {
        return new BossEncounter()
        {
            Name = Name,
            Id = Id,
            Abbriviation = Abbriviation,
        };
    }
}
