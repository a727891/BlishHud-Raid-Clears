using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace RaidClears.Features.Raids.Models;


[Serializable]
public class RaidWing
{
    [JsonProperty("id")]
    public string Id = "undefined";

    [JsonProperty("number")]
    public int Number = -1;

    [JsonProperty("name")]
    public string Name = "undefined";

    [JsonProperty("abbriviation")]
    public string Abbriviation = "undefined";

    [JsonProperty("mapId")]
    public int MapId = -1;

    [JsonProperty("call_of_mists_timestamp")]
    public int CallOfTheMistsTimestamp = -1;

    [JsonProperty("call_of_mists_weeks")]
    public int CallOfTheMistsWeeks = -1;

    [JsonProperty("emboldened_timestamp")]
    public int EmboldenedTimestamp = -1;

    [JsonProperty("emboldened_weeks")]
    public int EmboldendedWeeks = -1;

    [JsonProperty("encounters")]
    public List<RaidEncounter> Encounters = new();


    public RaidEncounter ToRaidEncounter()
    {
        return new RaidEncounter()
        {
            Name = Name,
            ApiId = Id,
            Abbriviation = Abbriviation,
        };
    }

}
