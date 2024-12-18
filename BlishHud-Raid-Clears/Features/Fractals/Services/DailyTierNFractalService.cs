﻿using RaidClears.Features.Raids.Models;
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
using Blish_HUD.Controls;

namespace RaidClears.Features.Fractals.Services;



public static class DailyTierNFractalService
{
    private static int DAILY_ROTATION_MAX_INDEX = 15;


    public static IEnumerable<BoxModel> GetDailyTierN()
    {
        return GetDailyTierNFractals().Select(e => new BoxModel($"{e.Encounter.id}", $"{e.Encounter.name}\n\n{Strings.Strike_Tooltip_tomorrow}\n{e.TomorrowEncounter.Label}", e.Encounter.shortName));
    }

    public static IEnumerable<(BoxModel box, FractalMap fractalMap, int scale)> GetCMFractals()
    {
        return BuildToolTipData(Service.FractalMapData.ChallengeMotes.Select(Service.FractalMapData.GetFractalForScale));
    }
    public static IEnumerable<(BoxModel box, FractalMap fractalMap, int scale)> GetTomorrowTierNForTooltip()
    {
        return BuildToolTipData(GetTomorrowTierNFractals());
    }

    public static IEnumerable<(BoxModel box, FractalMap fractalMap, int scale)> BuildToolTipData(IEnumerable<FractalMap> fractals)
    {
        var today = DayOfYearIndexService.DayOfYearIndex();
        var CMs = new List<(BoxModel box, FractalMap fractal, int scale)> { };

        foreach (var map in fractals)
        {
            var scales = map.Scales;
            //var tool = GetCMTooltip(map, scales.Last(), today);
            CMs.Add(
                (
                    new BoxModel(map.ApiLabel, "", Service.FractalPersistance.GetEncounterLabel(map.ApiLabel)),
                    map,
                    scales.Last()
                )
            );
        }
        return CMs;
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