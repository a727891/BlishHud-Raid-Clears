using Newtonsoft.Json;
using RaidClears.Features.Shared.Models;
using System;
using System.Collections.Generic;

namespace RaidClears.Features.Shared.Models;

[Serializable]
public class DailyBountyData
{
    [JsonProperty("enabled")]
    public bool Enabled { get; set; } = false;

    /// <summary>
    /// Official GW2 API URL for the Daily Raid Bounties achievement category (e.g. category 475).
    /// Used to fetch achievement IDs for marking bounty clears. Remotely updatable via JSON.
    /// </summary>
    [JsonProperty("dailyBountyCategoryUrl")]
    public string DailyBountyCategoryUrl { get; set; } = "https://api.guildwars2.com/v2/achievements/categories/475";

    [JsonProperty("version")]
    public string Version { get; set; } = "";

    [JsonProperty("name")]
    private string _name { get; set; } = "Daily Bounty";

    public string Name => GetLocalizedName(_name);

    [JsonProperty("abbreviation")]
    private string _abbreviation { get; set; } = "B";

    public string Abbreviation => GetLocalizedAbbreviation(_abbreviation);

    [JsonProperty("localizedNames")]
    public LocalizedStrings? LocalizedNames { get; set; }

    [JsonProperty("localizedAbbreviations")]
    public LocalizedStrings? LocalizedAbbreviations { get; set; }

    [JsonProperty("rotationType")]
    public string RotationType { get; set; } = "dayOfYearIndexed"; // "dayOfYearIndexed", "fixedRotation", "staticList"

    [JsonProperty("modulo")]
    public int Modulo { get; set; } = 365;

    [JsonProperty("offset")]
    public int Offset { get; set; } = 0;

    [JsonProperty("rotation")]
    public List<List<BountyEncounterReference>> Rotation { get; set; } = new();

    [JsonProperty("staticEncounters")]
    public List<BountyEncounterReference> StaticEncounters { get; set; } = new();

    /// <summary>
    /// New per-slot rotation model for daily bounties.
    /// Each slot represents one boss position (1–4) with its own offset and encounter cycle.
    /// When this collection is non-empty, it takes precedence over the legacy Rotation list.
    /// </summary>
    [JsonProperty("bossSlots")]
    public List<BossSlotRotation> BossSlots { get; set; } = new();

    public string GetLocalizedName(string defaultName)
    {
        var locale = GetUserLocale();
        var localizedName = LocalizedNames?.GetValue(locale);
        return localizedName ?? defaultName;
    }

    public string GetLocalizedAbbreviation(string defaultAbbreviation)
    {
        var locale = GetUserLocale();
        var localizedAbbr = LocalizedAbbreviations?.GetValue(locale);
        return localizedAbbr ?? defaultAbbreviation;
    }

    private string GetUserLocale()
    {
        try
        {
            var userLocaleSetting = Blish_HUD.GameService.Overlay.UserLocale;
            if (userLocaleSetting == null) return "en";

            var locale = userLocaleSetting.Value;
            return locale switch
            {
                Gw2Sharp.WebApi.Locale.English => "en",
                Gw2Sharp.WebApi.Locale.French => "fr",
                Gw2Sharp.WebApi.Locale.German => "de",
                Gw2Sharp.WebApi.Locale.Spanish => "es",
                _ => "en"
            };
        }
        catch
        {
            return "en";
        }
    }
}

[Serializable]
public class BountyEncounterReference
{
    [JsonProperty("encounterId")]
    public string EncounterId { get; set; } = "";

    [JsonProperty("type")]
    public string Type { get; set; } = "raid_encounter"; // "raid_encounter" or "raid_wing_encounter"
}

[Serializable]
public class BossSlotRotation
{
    /// <summary>
    /// Boss slot position (1–4).
    /// </summary>
    [JsonProperty("slot")]
    public int Slot { get; set; }

    /// <summary>
    /// Per-slot offset applied to the day-of-year index before modulo.
    /// </summary>
    [JsonProperty("offset")]
    public int Offset { get; set; } = 0;

    /// <summary>
    /// Ordered list of encounter ids for this slot.
    /// Modulo is implicitly Encounters.Count.
    /// </summary>
    [JsonProperty("encounters")]
    public List<string> Encounters { get; set; } = new();
}
