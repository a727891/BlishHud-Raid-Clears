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
    public FractalMap TomorrowEncounter;

    public List<string>? Instabilities;
    public List<string>? TomorrowInstabilities;

    public FractalInfo(FractalMap mission, FractalMap tomorrow)
    {
        Encounter = new(mission);
        TomorrowEncounter = tomorrow;
    }
    public FractalInfo(FractalMap mission, FractalMap tomorrow, List<string>instab, List<string>tomorrowInstab)
    {
        Encounter = new(mission);
        TomorrowEncounter = tomorrow;
        Instabilities = instab;
        TomorrowInstabilities = tomorrowInstab;
    }

}

public static class DailyRecommendedFractalService
{
    private static int DAILY_ROTATION_MAX_INDEX = 15;


    public static IEnumerable<BoxModel> GetRecommendedFractals()
    {
        return GetDailyRecommendedFractals().Select(e => new BoxModel($"{e.Encounter.id}", $"{e.Encounter.name}\n\n{Strings.Strike_Tooltip_tomorrow}\n{e.TomorrowEncounter.Label}", e.Encounter.shortName));
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
            var todayFrac = Service.FractalMapData.GetFractalForScale(todayScales[i]);
            var tomorrowFrac = Service.FractalMapData.GetFractalForScale(tomorrowsScales[i]);
            resultList.Add(new FractalInfo(todayFrac, tomorrowFrac));
        }

        return resultList;

    }

    public static List<int> DailyRecsRotation(int index)
    {
        if(Service.FractalMapData.Recs.Count >= index)
        {
            return Service.FractalMapData.Recs[index];
        }
        else
        {
            return new List<int> {96, 97, 98, 99, 100 };
        }
        //https://wiki.guildwars2.com/index.php?title=Template:Daily_Recommended_Fractal_Schedule&action=edit
    }


}