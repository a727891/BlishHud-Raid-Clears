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
using System.Runtime.Remoting.Channels;

namespace RaidClears.Features.Fractals.Services;



public static class DailyTierNFractalService
{
    private static int DAILY_ROTATION_MAX_INDEX = 15;


    public static IEnumerable<BoxModel> GetDailyTierN()
    {
        return GetDailyTierNFractals().Select(e => new BoxModel($"{e.Encounter.id}", $"{e.Encounter.name}\n\n{Strings.Strike_Tooltip_tomorrow}\n{e.TomorrowEncounter.Label}", e.Encounter.shortName));
    }

    public static IEnumerable<BoxModel> GetCMFractals()
    {
        var today = DayOfYearIndexService.DayOfYearIndex();
        var CMs = new List<(FractalMap fractal, int scale)> {};
        foreach(var scale in Service.FractalMapData.ChallengeMotes)
        {
            CMs.Add((Service.FractalMapData.GetFractalForScale(scale), scale));
        }

        return CMs.Select( e => new BoxModel(e.fractal.ApiLabel, GetCMTooltip(e.fractal, e.scale,today), e.fractal.ShortLabel));
    }

    private static string GetCMTooltip(FractalMap fractal, int scale, int today)
    {
        var instab = String.Join("\n\t",Service.InstabilitiesData.GetInstabsForLevelOnDay(scale, today).ToArray());
        var tomInstab = String.Join("\n\t",Service.InstabilitiesData.GetInstabsForLevelOnDay(scale, (today + 1) % 366).ToArray());
        return $"{fractal.Label}\n\nInstabilities\n\t{instab}\n\nTomorrow's Instabilities\n\t{tomInstab}";
    }

    public static IEnumerable<BoxModel> GetTomorrowTierN()
    {
        return GetTomorrowTierNFractals().Select(e => new BoxModel($"{e.ApiLabel}", $"{e.Label}", e.ShortLabel));
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

    public static IEnumerable<FractalMap> GetTomorrowTierNFractals()
    {

        var dayIndex = DayOfYearIndexService.DayOfYearIndex();

        var tomorrow = (dayIndex + 1) % DAILY_ROTATION_MAX_INDEX;

        
        var tomorrowsScales = DailyRecsRotation(tomorrow);

        var resultList = new List<FractalMap>();
        for (var i = 0; i < tomorrowsScales.Count(); i++)
        {
            resultList.Add(tomorrowsScales[i]);
        }

        return resultList;

    }

    public static List<FractalMap> DailyRecsRotation(int index)
    {
        List<FractalMap> _list = new();
        if(Service.FractalMapData.DailyTier.Count < index)
        {
            return _list;
        }

        var dayList = Service.FractalMapData.DailyTier[index];
        foreach(var fractalName in dayList)
        {
            _list.Add(Service.FractalMapData.GetFractalByName(fractalName));
        }
        return _list;

        //https://wiki.guildwars2.com/index.php?title=Template:Daily_Fractal_Schedule&action=edit
    }


}