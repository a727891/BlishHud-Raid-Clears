using System;

namespace RaidClears.Features.Raids.Models;

public class WeeklyModifier
{
    public bool Emboldened { get; } = false;
    public bool CallOfTheMist { get; } = false;
 
    public WeeklyModifier(RaidWing raidWing)
    {
        Emboldened = GetModifierActive(raidWing.EmboldenedTimestamp, raidWing.EmboldendedWeeks, Service.RaidData.SecondsInWeek);
        CallOfTheMist = GetModifierActive(raidWing.CallOfTheMistsTimestamp, raidWing.CallOfTheMistsWeeks, Service.RaidData.SecondsInWeek);
    }

    public bool GetModifierActive(int timestamp, int weeksBetween, int weeklySeconds)
    {
        if(weeksBetween <= 0)
        {
            return false;
        }
        var now = (DateTimeOffset)DateTime.UtcNow;
        var duration = now.ToUnixTimeSeconds() - timestamp;
        var wing = (int)Math.Floor((decimal)duration / weeklySeconds) % weeksBetween;
        return wing == 0;
    }
}
