using Newtonsoft.Json;
using System;


namespace RaidClears.Features.Raids.Models;


[Serializable]
public class RaidEncounter
{
    [JsonProperty("name")]
    public string Name = "undefined";

    [JsonProperty("api_id")]
    public string ApiId = "undefined";

    [JsonProperty("abbriviation")]
    public string Abbriviation = "undefined";
}

