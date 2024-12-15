using System.Collections.Generic;

namespace RaidClears.Features.Fractals.Models;

public static class FractalMetaData
{
    public static IEnumerable<Fractal> Create(FractalsPanel panel)
    {
        var fractals = new List<Fractal>(){};

        fractals.Add(new CMFractals(panel));
        fractals.Add(new TierNTomorrow(panel));
        fractals.Add(new TierNFractals(panel));
        fractals.Add(new DailyFractal(panel));

        return fractals;
    }


}