using Newtonsoft.Json;
using RaidClears.Features.Shared.Models;
using System.Collections.Generic;

namespace RaidClears.Features.Strikes.Models;

public class StrikeMission: EncounterInterface
{
    [JsonProperty("mapIds")]
    public List<int> MapIds = new();

}
