using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using RaidClears.Features.Shared.Controls;
using System;
using Blish_HUD;
using Gw2Sharp.WebApi;

namespace RaidClears.Features.Shared.Models;

[Serializable]
public class EncounterInterface
{
    [JsonProperty("id")]
    public string Id = "undefined";

    [JsonProperty("name")]
    public string Name = "undefined";

    [JsonProperty("abbriviation")]
    public string Abbriviation = "undefined";

    [JsonProperty("assetId")]
    public int AssetId = 0;

    [JsonProperty("localizedNames")]
    protected LocalizedStrings? LocalizedNames { get; set; }

    [JsonProperty("localizedAbbreviations")]
    protected LocalizedStrings? LocalizedAbbreviations { get; set; }

    /// <summary>
    /// Gets the localized name based on user locale, falling back to default Name if localization is not available.
    /// </summary>
    protected string GetLocalizedName(string defaultName)
    {
        var locale = GetUserLocale();
        var localizedName = LocalizedNames?.GetValue(locale);
        return localizedName ?? defaultName;
    }

    /// <summary>
    /// Gets the localized abbreviation based on user locale, falling back to default Abbriviation if localization is not available.
    /// </summary>
    protected string GetLocalizedAbbreviation(string defaultAbbreviation)
    {
        var locale = GetUserLocale();
        var localizedAbbr = LocalizedAbbreviations?.GetValue(locale);
        return localizedAbbr ?? defaultAbbreviation;
    }

    protected string GetUserLocale()
    {
        try
        {
            var userLocaleSetting = GameService.Overlay.UserLocale;
            if (userLocaleSetting == null) return "en";
            
            var locale = userLocaleSetting.Value;
            return locale switch
            {
                Locale.English => "en",
                Locale.French => "fr",
                Locale.German => "de",
                Locale.Spanish => "es",
                Locale.Korean => "en", // Fallback to English if not supported
                Locale.Chinese => "en", // Fallback to English if not supported
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
public class LocalizedStrings
{
    [JsonProperty("en")]
    public string? En { get; set; }

    [JsonProperty("fr")]
    public string? Fr { get; set; }

    [JsonProperty("de")]
    public string? De { get; set; }

    [JsonProperty("es")]
    public string? Es { get; set; }

    public string? GetValue(string locale)
    {
        if (locale == null) return En;
        var lowerLocale = locale.ToLowerInvariant();
        return lowerLocale switch
        {
            "fr" => Fr,
            "de" => De,
            "es" => Es,
            "en" => En,
            _ => En
        };
    }
}
