using Newtonsoft.Json;
using RaidClears.Features.Shared.Models;
using System;
using System.Collections.Generic;

namespace RaidClears.Features.Raids.Models;

[Serializable]
public class ExpansionRaid : EncounterInterface
{
    [JsonProperty("asset")]
    public string asset = "missing.png";

    [JsonProperty("wings")]
    public List<RaidWing> Wings = new();

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
