using Microsoft.Xna.Framework;
using System;

namespace RaidClears.Features.Strikes.Services;

public class ResetsWatcherService : IDisposable
{

    public event EventHandler<DateTime>? DailyReset;
    public event EventHandler<DateTime>? WeeklyReset;

    public  DateTime NextDailyReset { get; private set; }
    public DateTime LastDailyReset { get; private set; }
    public DateTime NextWeeklyReset { get; private set; }
    public DateTime LastWeeklyReset { get; private set; }

    public ResetsWatcherService()
    {
        CalcNextDailyReset();
        CalcNextWeeklyReset();
    }

    public void CalcNextDailyReset()
    {
        var now = DateTime.UtcNow;

        NextDailyReset = now.AddDays(1).Date;
        LastDailyReset = NextDailyReset.AddDays(-1);
    }

    public void CalcNextWeeklyReset()
    {
        NextWeeklyReset = NextDayOfWeek(DayOfWeek.Monday, 7, 30); //https://wiki.guildwars2.com/wiki/Server_reset#Weekly_reset
        LastWeeklyReset = NextWeeklyReset.AddDays(-7);

    }
    public static DateTime NextDayOfWeek(DayOfWeek weekday, int hour, int minute)
    {
        var today = DateTime.UtcNow;

        if (today.Hour < hour && today.DayOfWeek == weekday)
        {
            return today.Date.AddHours(hour).AddMinutes(minute);
        }
        else
        {
            var nextReset = today.AddDays(1);

            while (nextReset.DayOfWeek != weekday)
            {
                nextReset = nextReset.AddDays(1);
            }

            return nextReset.Date.AddHours(hour).AddMinutes(minute);
        }
    }

    public void Update(GameTime gametime)
    {
        var now = DateTime.UtcNow;
        if( now >= NextDailyReset )
        {
            DailyReset?.Invoke(this, NextDailyReset);
            CalcNextDailyReset();
        }
        if(now >= NextWeeklyReset )
        {
            WeeklyReset?.Invoke(this, NextWeeklyReset);
            CalcNextWeeklyReset();
        }
    }

    public void Dispose()
    {

    }
}