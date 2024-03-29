﻿using RaidClears.Features.Shared.Enums.Extensions;
using RaidClears.Features.Shared.Models;
using static RaidClears.Features.Shared.Enums.Encounters;

namespace RaidClears.Features.Raids.Models;

public class Encounter : BoxModel
{
    public Encounter(RaidBosses boss) : base(boss.GetApiLabel(), boss.GetLabel(), boss.GetLabelShort())
    {
    }
    
    public Encounter(StrikeMission boss) : base(boss.GetApiLabel(), boss.GetLabel(), boss.GetLabelShort())
    {
    }

    public Encounter(Fractal fractal) : base(fractal.GetApiLabel(), fractal.GetLabel(), fractal.GetLabelShort())
    {
    }
}
