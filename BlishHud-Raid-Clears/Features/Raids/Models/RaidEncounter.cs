using Newtonsoft.Json;
using RaidClears.Features.Shared.Models;
using System;


namespace RaidClears.Features.Raids.Models;


[Serializable]
public class RaidEncounter : EncounterInterface
{
    [JsonProperty("api_id")]
    public string ApiId = "undefined";

    [JsonProperty("name")]
    private string _name = "undefined";

    [JsonProperty("abbriviation")]
    private string _abbriviation = "undefined";

    [JsonProperty("powerFavored")]
    public bool PowerFavored { get; set; } = false;

    [JsonProperty("condiFavored")]
    public bool CondiFavored { get; set; } = false;

    [JsonProperty("needsDefianceBreak")]
    public bool NeedsDefianceBreak { get; set; } = false;

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

