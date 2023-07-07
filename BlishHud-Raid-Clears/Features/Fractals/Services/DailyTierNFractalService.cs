using RaidClears.Features.Raids.Models;
using RaidClears.Features.Shared.Enums;
using RaidClears.Features.Shared.Enums.Extensions;
using RaidClears.Features.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using RaidClears.Localization;
using RaidClears.Features.Shared.Services;
using Gw2Sharp.WebApi.V2.Models;
using SharpDX.Direct3D9;
using static RaidClears.Settings.Models.Settings;
using System.Numerics;

namespace RaidClears.Features.Fractals.Services;



public static class DailyTierNFractalService
{
    private static int DAILY_ROTATION_MAX_INDEX = 15;


    public static IEnumerable<BoxModel> GetDailyTierN()
    {
        return GetDailyTierNFractals().Select(e => new BoxModel($"{e.Encounter.id}", $"{e.Encounter.name}\n\n{Strings.Strike_Tooltip_tomorrow}\n{e.TomorrowEncounter.GetLabel()}", e.Encounter.shortName));
    }

    public static IEnumerable<BoxModel> GetCMFractals()
    {
        var CMs = new List<Encounters.Fractal> {
                Encounters.Fractal.NightmareFractal,
                Encounters.Fractal.ShatteredObservatoryFractal,
                Encounters.Fractal.SunquaPeakFractal,
                Encounters.Fractal.SilentSurfFractal
        };
        return CMs.Select(e => new BoxModel(e.GetApiLabel(), e.GetLabel(), e.GetLabelShort()));
    }

    public static IEnumerable<BoxModel> GetTomorrowTierN()
    {
        return GetTomorrowTierNFractals().Select(e => new BoxModel($"{e.GetApiLabel()}", $"{e.GetLabel()}", e.GetLabelShort()));
    }

    public static IEnumerable<FractalInfo> GetDailyTierNFractals()
    {

       var dayIndex = DayOfYearIndexService.DayOfYearIndex();

        var today = dayIndex % DAILY_ROTATION_MAX_INDEX;
        var tomorrow = (today + 1) % DAILY_ROTATION_MAX_INDEX;

        var todayScales = DailyRecsRotation(today);
        var tomorrowsScales = DailyRecsRotation(tomorrow);

        var resultList = new List<FractalInfo>();
        for(var i=0; i< todayScales.Count(); i++)
        {
            resultList.Add(new FractalInfo(todayScales[i], tomorrowsScales[i]));
        }

        return resultList;

    }

    public static IEnumerable<Encounters.Fractal> GetTomorrowTierNFractals()
    {

        var dayIndex = DayOfYearIndexService.DayOfYearIndex();

        var tomorrow = (dayIndex + 1) % DAILY_ROTATION_MAX_INDEX;

        
        var tomorrowsScales = DailyRecsRotation(tomorrow);

        var resultList = new List<Encounters.Fractal>();
        for (var i = 0; i < tomorrowsScales.Count(); i++)
        {
            resultList.Add(tomorrowsScales[i]);
        }

        return resultList;

    }

    public static List<Encounters.Fractal> DailyRecsRotation(int index)
    {
        //https://wiki.guildwars2.com/index.php?title=Template:Daily_Fractal_Schedule&action=edit
        switch (index) //Daily rotation index
        {
            case 0:
                return new List<Encounters.Fractal> { Encounters.Fractal.NightmareFractal, Encounters.Fractal.SnowblindFractal, Encounters.Fractal.VolcanicFractal };
            case 1:
                return new List<Encounters.Fractal> { Encounters.Fractal.AetherbladeFractal, Encounters.Fractal.UncategorizedFractal, Encounters.Fractal.ThaumanovaReactorFractal };
            case 2:
                return new List<Encounters.Fractal> { Encounters.Fractal.TwilightOasisFractal, Encounters.Fractal.CliffsideFractal, Encounters.Fractal.ChaosFractal };
            case 3:
                return new List<Encounters.Fractal> { Encounters.Fractal.DeepstoneFractal, Encounters.Fractal.CaptainMaiTrinBossFractal, Encounters.Fractal.SilentSurfFractal };
            case 4:
                return new List<Encounters.Fractal> { Encounters.Fractal.SnowblindFractal, Encounters.Fractal.SolidOceanFractal, Encounters.Fractal.NightmareFractal };
            case 5:
                return new List<Encounters.Fractal> { Encounters.Fractal.ChaosFractal, Encounters.Fractal.UncategorizedFractal, Encounters.Fractal.UrbanBattlegroundFractal };
            case 6:
                return new List<Encounters.Fractal> { Encounters.Fractal.SirensReefFractal, Encounters.Fractal.MoltenFurnaceFractal, Encounters.Fractal.DeepstoneFractal };
            case 7:
                return new List<Encounters.Fractal> { Encounters.Fractal.MoltenBossFractal, Encounters.Fractal.TwilightOasisFractal, Encounters.Fractal.UndergroundFacilityFractal };
            case 8:
                return new List<Encounters.Fractal> { Encounters.Fractal.VolcanicFractal, Encounters.Fractal.SwamplandFractal, Encounters.Fractal.SilentSurfFractal };
            case 9:
                return new List<Encounters.Fractal> { Encounters.Fractal.SnowblindFractal, Encounters.Fractal.ThaumanovaReactorFractal, Encounters.Fractal.AquaticRuinsFractal };
            case 10:
                return new List<Encounters.Fractal> { Encounters.Fractal.UndergroundFacilityFractal, Encounters.Fractal.UrbanBattlegroundFractal, Encounters.Fractal.SunquaPeakFractal };
            case 11:
                return new List<Encounters.Fractal> { Encounters.Fractal.AetherbladeFractal, Encounters.Fractal.ChaosFractal, Encounters.Fractal.NightmareFractal };
            case 12:
                return new List<Encounters.Fractal> { Encounters.Fractal.CliffsideFractal, Encounters.Fractal.MoltenBossFractal, Encounters.Fractal.SirensReefFractal };
            case 13:
                return new List<Encounters.Fractal> { Encounters.Fractal.SwamplandFractal, Encounters.Fractal.SolidOceanFractal, Encounters.Fractal.DeepstoneFractal };
            case 14:
                return new List<Encounters.Fractal> { Encounters.Fractal.CaptainMaiTrinBossFractal, Encounters.Fractal.ShatteredObservatoryFractal, Encounters.Fractal.MoltenBossFractal };


            default: return new List<Encounters.Fractal> { Encounters.Fractal.NightmareFractal };
        }
    }


}