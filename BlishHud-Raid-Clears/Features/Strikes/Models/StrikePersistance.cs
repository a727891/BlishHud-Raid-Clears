using Newtonsoft.Json;
using RaidClears.Features.Shared.Enums;
using SharpDX.MediaFoundation;
using System;
using System.Collections.Generic;

namespace RaidClears.Features.Strikes.Models;


[Serializable]
public class StrikePersistance
{
    [JsonProperty("version")]
    public string Version { get; set; } = "2.0.0";

    [JsonProperty("accountClears")]
    public Dictionary<string, Dictionary<Encounters.StrikeMission, DateTime>> AccountClears { get; set; } = new();


    public string Save()
    {
        var serializedContents = JsonConvert.SerializeObject(this, Formatting.Indented);
        return serializedContents;
    }
}