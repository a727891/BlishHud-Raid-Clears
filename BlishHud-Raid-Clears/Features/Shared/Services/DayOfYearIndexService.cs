using System;

namespace RaidClears.Features.Shared.Services;

public static  class DayOfYearIndexService
{
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
}