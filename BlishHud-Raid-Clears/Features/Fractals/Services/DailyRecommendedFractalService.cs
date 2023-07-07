using RaidClears.Features.Raids.Models;
using RaidClears.Features.Shared.Enums;
using RaidClears.Features.Shared.Enums.Extensions;
using RaidClears.Features.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using RaidClears.Localization;
using RaidClears.Features.Shared.Services;

namespace RaidClears.Features.Fractals.Services;

public class FractalInfo
{
    public Encounter Encounter;
    public Encounters.Fractal TomorrowEncounter;

    public FractalInfo(Encounters.Fractal mission, Encounters.Fractal tomorrow)
    {
        Encounter = new(mission);
        TomorrowEncounter = tomorrow;
    }

}

public static class DailyRecommendedFractalService
{
    private static int DAILY_ROTATION_MAX_INDEX = 15;


    public static IEnumerable<BoxModel> GetRecommendedFractals()
    {
        return GetDailyRecommendedFractals().Select(e => new BoxModel($"{e.Encounter.id}", $"{e.Encounter.name}\n\n{Strings.Strike_Tooltip_tomorrow}\n{e.TomorrowEncounter.GetLabel()}", e.Encounter.shortName));
    }

    public static IEnumerable<FractalInfo> GetDailyRecommendedFractals()
    {

       var dayIndex = DayOfYearIndexService.DayOfYearIndex();

        var today = dayIndex % DAILY_ROTATION_MAX_INDEX;
        var tomorrow = (today + 1) % DAILY_ROTATION_MAX_INDEX;

        var todayScales = DailyRecsRotation(today);
        var tomorrowsScales = DailyRecsRotation(tomorrow);

        var resultList = new List<FractalInfo>();
        for(var i=0; i< todayScales.Count(); i++)
        {
            var todayFrac = FractalExtensions.GetFractalForScale(todayScales[i]);
            var tomorrowFrac = FractalExtensions.GetFractalForScale(tomorrowsScales[i]);
            resultList.Add(new FractalInfo(todayFrac, tomorrowFrac));
        }

        return resultList;

    }

    public static List<int> DailyRecsRotation(int index)
    {
        //https://wiki.guildwars2.com/index.php?title=Template:Daily_Recommended_Fractal_Schedule&action=edit
        switch (index) //Daily rotation index
        {
            case 0:
                return new List<int> { 2, 37, 53 };
            case 1:
                return new List<int> { 6, 28, 61 };
            case 2:
                return new List<int> { 10, 32, 65 };
            case 3:
                return new List<int> { 14, 34, 74 };
            case 4:
                return new List<int> { 19, 37, 66 };
            case 5:
                return new List<int> { 15, 41, 60 };
            case 6:
                return new List<int> { 24, 35, 75 };
            case 7:
                return new List<int> { 25, 36, 69 };
            case 8:
                return new List<int> { 12, 40, 67 };
            case 9:
                return new List<int> { 8, 31, 54 };
            case 10:
                return new List<int> { 11, 39, 59 };
            case 11:
                return new List<int> { 18, 27, 64 };
            case 12:
                return new List<int> { 4, 30, 58 };
            case 13:
                return new List<int> { 16, 42, 62 };
            case 14:
                return new List<int> { 5, 48, 68 };
          
            default: return new List<int> { 98, 99, 100 };
        }
    }


}