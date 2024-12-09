using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Blish_HUD.Settings;
using RaidClears.Features.Raids.Services;
using RaidClears.Features.Shared.Controls;
using RaidClears.Features.Shared.Enums;
using RaidClears.Features.Shared.Models;
using RaidClears.Features.Strikes.Models;
using RaidClears.Localization;
using RaidClears.Settings.Models;
using RaidClears.Utils;

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
