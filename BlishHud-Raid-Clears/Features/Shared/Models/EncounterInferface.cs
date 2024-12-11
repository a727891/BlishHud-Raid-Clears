using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using RaidClears.Features.Shared.Controls;
using System;

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
    public int? AssetId = 0;
}
