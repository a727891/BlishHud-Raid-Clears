using Newtonsoft.Json;
using RaidClears.Features.Shared.Models;
using System.Collections.Generic;

namespace RaidClears.Features.Strikes.Models;

public class StrikeMission: EncounterInterface
{
    [JsonProperty("mapIds")]
    public List<int> MapIds = new();

    /// <summary>
    /// Optional GW2 achievement ID for the Daily Raid Bounty when this strike is a bounty (e.g. Kaineng Overlook: 9179).
    /// </summary>
    [JsonProperty("daily_bounty_achievement_id", NullValueHandling = NullValueHandling.Ignore)]
    public int? DailyBountyAchievementId { get; set; }

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
}
