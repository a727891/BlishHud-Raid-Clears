using RaidClears.Features.Shared.Models;
using RaidClears.Settings.Models;
using System.Collections.Generic;
using System.Linq;

namespace RaidClears.Features.Fractals.Models;

public static class FractalMetaData
{
    private static FractalSettings Settings => Service.Settings.FractalSettings;

    public static IEnumerable<Fractal> Create(FractalsPanel panel)
    {
        var fractals = new List<Fractal>(){};

        fractals.Add(new CMFractals("Challenge Motes", 4, "CM", new List<BoxModel>() { }, panel));
        fractals.Add(new TierNFractals("Tier #", 0, "T#", new List<BoxModel>() { }, panel));
        fractals.Add(new DailyFractal("Daily Recommended", 1, "Rec", new List<BoxModel>() { }, panel));
        fractals.Add(new TierNTomorrow("Tomorrow T#", 2, "Tom", new List<BoxModel>() { }, panel));


        return fractals;
    }


}