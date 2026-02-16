using RaidClears.Features.Raids.Models;
using RaidClears.Features.Shared.Models;
using RaidClears.Features.Strikes.Models;
using RaidClears.Features.Strikes.Services;
using RaidClears;
using System;
using System.Collections.Generic;
using System.Linq;
using RaidClears.Features.Raids.Services;

namespace RaidClears.Features.Shared.Services;

public static class DailyBountyService
{
    public static IEnumerable<Encounter> GetDailyBounties()
    {
        var bountyData = Service.DailyBountyData;
        var raidData = Service.RaidData;
        if (bountyData == null || !bountyData.Enabled)
            return Enumerable.Empty<Encounter>();

        var dayIndex = PriorityRotationService.DayOfYearIndex();

        return GetDayOfYearBounties(dayIndex, bountyData, raidData);
    }

    public static IEnumerable<Encounter> GetTomorrowBounties()
    {
        var bountyData = Service.DailyBountyData;
        var raidData = Service.RaidData;
        var strikeData = Service.StrikeData;
        if (bountyData == null || !bountyData.Enabled)
            return Enumerable.Empty<Encounter>();

        var dayIndex = PriorityRotationService.DayOfYearIndex() + 1;

        return GetDayOfYearBounties(dayIndex, bountyData, raidData, "tomorrow_");
    }

    /// <summary>
    /// Returns raid encounter ApiIds that are daily bounties for the given day-of-year index.
    /// Used to build the weekly bounty set without mutating raid data.
    /// </summary>
    public static IEnumerable<string> GetBountyEncounterApiIdsForDay(int dayIndex)
    {
        var bountyData = Service.DailyBountyData;
        if (bountyData == null || !bountyData.Enabled)
            yield break;

        if (bountyData.BossSlots != null && bountyData.BossSlots.Count > 0)
        {
            foreach (var slot in bountyData.BossSlots)
            {
                if (slot.Encounters == null || slot.Encounters.Count == 0)
                    continue;

                var modulo = slot.Encounters.Count;
                var indexForSlot = (dayIndex + slot.Offset) % modulo;
                if (indexForSlot < 0)
                    indexForSlot += modulo;

                var encounterId = slot.Encounters[indexForSlot];
                if (!string.IsNullOrWhiteSpace(encounterId))
                    yield return encounterId;
            }
            yield break;
        }

        var legacyIndex = (dayIndex + bountyData.Offset) % bountyData.Modulo;
        if (legacyIndex < bountyData.Rotation.Count)
        {
            foreach (var reference in bountyData.Rotation[legacyIndex])
            {
                if (!string.IsNullOrWhiteSpace(reference.EncounterId))
                    yield return reference.EncounterId;
            }
        }
    }

    private static IEnumerable<Encounter> GetDayOfYearBounties(int dayIndex, DailyBountyData bountyData, RaidData raidData, string prefix = "priority_")
    {
        // New per-slot rotation model (bossSlots).
        // When bossSlots is populated, we derive today's encounters by
        // independently indexing into each slot's encounter list.
        if (bountyData.BossSlots != null && bountyData.BossSlots.Count > 0)
        {
            var references = new List<BountyEncounterReference>();

            foreach (var slot in bountyData.BossSlots)
            {
                if (slot.Encounters == null || slot.Encounters.Count == 0)
                    continue;

                var modulo = slot.Encounters.Count;
                var indexForSlot = (dayIndex + slot.Offset) % modulo;
                if (indexForSlot < 0)
                    indexForSlot += modulo;

                var encounterId = slot.Encounters[indexForSlot];
                if (string.IsNullOrWhiteSpace(encounterId))
                    continue;

                references.Add(new BountyEncounterReference
                {
                    EncounterId = encounterId
                });
            }

            return ResolveBountyEncounters(references, raidData, prefix);
        }

        // Legacy rotation model: single list-of-lists indexed by day-of-year.
        var legacyIndex = (dayIndex + bountyData.Offset) % bountyData.Modulo;
        if (legacyIndex < bountyData.Rotation.Count)
        {
            var references = bountyData.Rotation[legacyIndex];
            return ResolveBountyEncounters(references, raidData, prefix);
        }

        return Enumerable.Empty<Encounter>();
    }


    private static IEnumerable<Encounter> ResolveBountyEncounters(List<BountyEncounterReference> references, RaidData raidData, string prefix = "priority_")
    {
        var encounters = new List<Encounter>();

        foreach (var reference in references)
        {
            var raidEncounter = raidData.GetRaidEncounterByApiId(reference.EncounterId);

            Encounter encounter;
            if (raidEncounter != null)
            {
                raidEncounter.Id = $"{prefix}{raidEncounter.Id}";
                raidEncounter.ApiId = $"{prefix}{raidEncounter.ApiId}";
                encounter = new Encounter(raidEncounter);
                encounters.Add(encounter);
            }
            else
            {
                Module.ModuleLogger.Warn($"Could not resolve bounty encounter: {reference.EncounterId}");
                continue;
            }
        }

        return encounters;
    }
}
