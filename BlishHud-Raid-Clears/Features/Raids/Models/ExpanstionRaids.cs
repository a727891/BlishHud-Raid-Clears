using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace RaidClears.Features.Raids.Models;

[Serializable]
public class ExpansionRaid
{
    [JsonProperty("id")]
    public string Id = "undefined";

    [JsonProperty("name")]
    public string Name = "undefined";

    [JsonProperty("abbriviation")]
    public string Abbriviation = "undefined";

    [JsonProperty("asset")]
    public string asset = "missing.png";

    [JsonProperty("wings")]
    public List<RaidWing> Wings = new();

}
