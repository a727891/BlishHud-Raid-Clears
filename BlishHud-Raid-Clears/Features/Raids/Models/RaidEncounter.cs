﻿using Newtonsoft.Json;
using RaidClears.Features.Shared.Models;
using System;


namespace RaidClears.Features.Raids.Models;


[Serializable]
public class RaidEncounter : EncounterInterface
{
    [JsonProperty("api_id")]
    public string ApiId = "undefined";

}

