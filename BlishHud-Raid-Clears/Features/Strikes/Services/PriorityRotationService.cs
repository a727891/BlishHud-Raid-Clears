using RaidClears.Features.Raids.Models;
using RaidClears.Features.Strikes.Models;
using RaidClears.Features.Shared.Enums;
using RaidClears.Features.Shared.Enums.Extensions;
using RaidClears.Features.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using RaidClears.Localization;
using System.Runtime.InteropServices.WindowsRuntime;

namespace RaidClears.Features.Strikes.Services;

public class StrikeInfo
{
    public Encounter Encounter;
    public StrikeMission TomorrowEncounter;
    public List<int> MapIds;

    public StrikeInfo(StrikeMission mission, List<int> maps, StrikeMission tomorrow)
    {
        Encounter = new(mission);
        MapIds = maps;
        TomorrowEncounter = tomorrow;
    }
    public StrikeInfo(StrikeMission mission, StrikeMission tomorrow)
    {
        Encounter = new(mission);
        MapIds = mission.MapIds;
        TomorrowEncounter = tomorrow;
    }
}

public static class PriorityRotationService
{

    public static IEnumerable<BoxModel> GetPriorityEncounters()
    {
        return GetPriorityStrikes()
            .Select(e =>
            new BoxModel(
                 $"priority_{e.Encounter.id}", 
                $"{e.Encounter.name}\n\n{Strings.Strike_Tooltip_tomorrow}\n{e.TomorrowEncounter.Name}",
                e.Encounter.shortName
                )
            );
    }

    /**
     * Reference: https://wiki.guildwars2.com/wiki/Template:Day_of_year_index
     * Return 0-365 ALWAYS INCLUDING Feb29
     **/
    public static int DayOfYearIndex()
    {
        return DayOfYearIndex(DateTime.UtcNow);
    }
    public static int DayOfYearIndex(DateTime date)
    {
        var day = date.DayOfYear - 1;//-1 to make 0 based index
        if (DateTime.IsLeapYear(date.Year))
        {
            return day;
        }
        else
        {
            if (date.Month >= 3)
            {
                return day + 1;
            }
            return day;
        }
    }
    public static IEnumerable<StrikeInfo> GetPriorityStrikes()
    {
        return Service.StrikeData.GetPriorityStrikes(DayOfYearIndex());
    }

}