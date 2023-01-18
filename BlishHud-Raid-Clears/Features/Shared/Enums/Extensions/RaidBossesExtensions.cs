using System;
using RaidClears.Localization;

namespace RaidClears.Features.Shared.Enums.Extensions;

public static class RaidBossesExtensions
{
    public static string GetLabel(this Encounters.RaidBosses value)
    {
        return value switch
        {
            Encounters.RaidBosses.ValeGuardian => Strings.Raid_Wing_1_1_Name,
            _ => throw new ArgumentOutOfRangeException(nameof(value), value, null)
        };
    }
    public static string GetLabelShort(this Encounters.RaidBosses value)
    {
        return value switch
        {
            Encounters.RaidBosses.ValeGuardian => Strings.Raid_Wing_1_1_Short,
            _ => throw new ArgumentOutOfRangeException(nameof(value), value, null)
        };
    }

    public static string GetApiLabel(this Encounters.RaidBosses value)
    {
        return value switch
        {
            Encounters.RaidBosses.ValeGuardian => "vale_guardian",
            _ => throw new ArgumentOutOfRangeException(nameof(value), value, null)
        };
    }
}