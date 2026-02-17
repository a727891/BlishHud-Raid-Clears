using Newtonsoft.Json;
using RaidClears.Features.Shared.Models;
using System;
using System.Collections.Generic;

namespace RaidClears.Features.Raids.Models;

[Serializable]
public class ExpansionRaid : EncounterInterface, IExpansion<RaidWing>
{
    [JsonProperty("asset")]
    public string asset = "missing.png";

    [JsonIgnore]
    public string Asset => asset;

    [JsonProperty("wings")]
    public List<RaidWing> Wings = new();

    [JsonIgnore]
    public IReadOnlyList<RaidWing> Children => Wings;

    // IExpansion Id is backed by EncounterInterface.Id
    string IExpansion<RaidWing>.Id => Id;
}
