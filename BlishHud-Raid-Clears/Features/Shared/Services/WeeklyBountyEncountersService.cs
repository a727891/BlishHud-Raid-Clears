using System;
using System.Collections.Generic;
using System.Linq;
using RaidClears.Features.Strikes.Services;

namespace RaidClears.Features.Shared.Services;

/// <summary>
/// Builds and exposes the set of raid encounter ApiIds that will be daily bounties
/// for the remainder of the current week (today through the day before weekly reset).
/// </summary>
public class WeeklyBountyEncountersService
{
    private readonly HashSet<string> _weeklyBountyApiIds = new();
    private readonly object _lock = new();

    public WeeklyBountyEncountersService()
    {
        Rebuild();
    }

    public void Rebuild()
    {
        lock (_lock)
        {
            _weeklyBountyApiIds.Clear();

            var bountyData = Service.DailyBountyData;
            if (bountyData == null || !bountyData.Enabled)
                return;

            var nextWeekly = Service.ResetWatcher.NextWeeklyReset;
            var today = DateTime.UtcNow.Date;
            var lastDayOfWeek = nextWeekly.Date.AddDays(-1);

            if (today > lastDayOfWeek)
                return;

            for (var date = today; date <= lastDayOfWeek; date = date.AddDays(1))
            {
                var dayIndex = PriorityRotationService.DayOfYearIndex(date);
                foreach (var apiId in DailyBountyService.GetBountyEncounterApiIdsForDay(dayIndex))
                    _weeklyBountyApiIds.Add(apiId);
            }
        }
    }

    public bool IsWeeklyBounty(string encounterApiId)
    {
        if (string.IsNullOrEmpty(encounterApiId)) return false;
        lock (_lock)
        {
            return _weeklyBountyApiIds.Contains(encounterApiId);
        }
    }
}
