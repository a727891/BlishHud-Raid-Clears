using Newtonsoft.Json;
using RaidClears.Features.Raids.Models;
using RaidClears.Features.Shared.Models;
using System;
using System.Collections.Generic;

namespace RaidClears.Features.Strikes.Models;

[Serializable]
public class ExpansionStrikes : EncounterInterface
{
    [JsonProperty("asset")]
    public string asset = "missing.png";

    [JsonProperty("resets")]
    public string Resets = "weekly";

    [JsonProperty("daily_priority_modulo")]
    public int DailyPriorityModulo = 1;

    [JsonProperty("daily_priority_offset")]
    public int DailyPriorityOffset = 0;

    [JsonProperty("missions")]
    public List<StrikeMission> Missions = new();

    [JsonProperty("name")]
    private string _name = "undefined";

    [JsonProperty("abbriviation")]
    private string _abbriviation = "undefined";

    /// <summary>
    /// Returns the localized name based on user locale, falling back to default name if localization is not available.
    /// </summary>
    public new string Name
    {
        get => GetLocalizedName(_name);
        set => _name = value;
    }

    /// <summary>
    /// Returns the localized abbreviation based on user locale, falling back to default abbreviation if localization is not available.
    /// </summary>
    public new string Abbriviation
    {
        get => GetLocalizedAbbreviation(_abbriviation);
        set => _abbriviation = value;
    }

    public List<BoxModel> GetEncounters()
    {
        List<BoxModel> missionslist = new();
        foreach (var mission in Missions)
        {
            missionslist.Add(new Encounter(mission));
        }
        return missionslist;
    }

    public StrikeMission ToStrikeMission()
    {
        return new StrikeMission()
        {
            Name = Name,
            Id = Id,
            Abbriviation = Abbriviation,
        };
    }
}
