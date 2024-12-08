using RaidClears.Features.Fractals.Services;
using RaidClears.Features.Raids.Services;
using RaidClears.Features.Shared.Enums.Extensions;
using RaidClears.Features.Shared.Models;
using RaidClears.Features.Strikes.Services;
using static RaidClears.Features.Shared.Enums.Encounters;

namespace RaidClears.Features.Raids.Models;

public class Encounter : BoxModel
{
    public Encounter(RaidBosses boss) : base(boss.GetApiLabel(), boss.GetLabel(), boss.GetLabelShort())
    {
    }

    public Encounter(RaidEncounter enc) : base(enc.ApiId, enc.Name, enc.Abbriviation)
    { 
    }


    public Encounter(StrikeMission boss) : base(boss.Id, boss.Name, boss.Abbriviation)
    {
    }

    public Encounter(FractalMap fractal) : base(fractal.ApiLabel, fractal.Label, fractal.ShortLabel)
    {
    }

}
