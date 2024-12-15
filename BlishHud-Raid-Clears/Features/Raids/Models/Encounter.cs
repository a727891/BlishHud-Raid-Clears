using Blish_HUD.Controls;
using Gw2Sharp.WebApi.V2.Models;
using RaidClears.Features.Fractals.Models;
using RaidClears.Features.Fractals;
using RaidClears.Features.Fractals.Services;
using RaidClears.Features.Raids.Services;
using RaidClears.Features.Shared.Enums.Extensions;
using RaidClears.Features.Shared.Models;
using RaidClears.Features.Shared.Services;
using RaidClears.Features.Strikes.Models;
using SharpDX.Direct2D1.Effects;
using static RaidClears.Features.Shared.Enums.Encounters;

namespace RaidClears.Features.Raids.Models;

public class Encounter : BoxModel
{
    public Encounter(RaidEncounter enc) : base(enc.ApiId, enc.Name, Service.RaidSettings.GetEncounterLabel(enc))
    {
        _tooltip = new RaidTooltipView
        {
            Encoutner = enc
        };
    }

    public Encounter(StrikeMission boss) : base(boss.Id, boss.Name, boss.Abbriviation)
    {
        _tooltip = new RaidTooltipView
        {
            StrikeMission = boss
        };
    }

    public Encounter(FractalMap fractal) : base(fractal.ApiLabel, fractal.Label, fractal.ShortLabel)
    {
    }

}
