using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace RaidClears.Features.Shared.Models;

/// <summary>
/// Shared model for a single encounter (raid boss or strike mission).
/// Used by both raid_data.json and strike_data.json with type-specific properties defaulting when not present.
/// </summary>
[Serializable]
public class BossEncounter : EncounterInterface, IEncounter
{
    /// <summary>Raid encounters use this in JSON (api_id); strikes use base Id.</summary>
    [JsonProperty("api_id", NullValueHandling = NullValueHandling.Ignore)]
    public string ApiId = "undefined";

    [JsonProperty("mapIds")]
    public List<int> MapIds = new();

    [JsonProperty("powerFavored")]
    public bool PowerFavored { get; set; }

    [JsonProperty("condiFavored")]
    public bool CondiFavored { get; set; }

    [JsonProperty("needsDefianceBreak")]
    public bool NeedsDefianceBreak { get; set; }

    /// <summary>Optional GW2 achievement ID for the boss mentor achievement (raids).</summary>
    [JsonProperty("mentor_achievement_id", NullValueHandling = NullValueHandling.Ignore)]
    public int? MentorAchievementId { get; set; }

    /// <summary>Optional GW2 achievement ID for the Daily Raid Bounty (raids and some strikes).</summary>
    [JsonProperty("daily_bounty_achievement_id", NullValueHandling = NullValueHandling.Ignore)]
    public int? DailyBountyAchievementId { get; set; }

    /// <summary>Stable id for lookups: raids use ApiId, strikes use Id.</summary>
    public string EncounterId => ApiId != null && ApiId != "undefined" ? ApiId : Id;

    /// <summary>True when this encounter is a strike mission (has map IDs).</summary>
    public bool IsStrike => MapIds != null && MapIds.Count > 0;

    string IEncounter.Id => EncounterId;
    int IEncounter.IconAssetId => AssetId;
}
