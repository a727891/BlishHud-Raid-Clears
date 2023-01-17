using RaidClears.Localization;
using System;
using static RaidClears.Features.Shared.Enums.Encounters;


namespace RaidClears.Features.Shared.Enums;

public static class Encounters
{
    public enum RaidBosses
    {
        ValeGuardian,

    }
    public enum StrikeMission
    {
        ShiverpeaksPass,
    }

}


public static class RaidBossesExtensions
{
    public static string GetLabel(this RaidBosses value)
    {
        return value switch
        {
            RaidBosses.ValeGuardian => Strings.Raid_Wing_1_1_Name,
            _ => throw new ArgumentOutOfRangeException(nameof(value), value, null)
        };
    }
    public static string GetLabelShort(this RaidBosses value)
    {
        return value switch
        {
            RaidBosses.ValeGuardian => Strings.Raid_Wing_1_1_Short,
            _ => throw new ArgumentOutOfRangeException(nameof(value), value, null)
        };
    }

    public static string GetApiLabel(this RaidBosses value)
    {
        return value switch
        {
            RaidBosses.ValeGuardian => "vale_guardian",
            _ => throw new ArgumentOutOfRangeException(nameof(value), value, null)
        };
    }

}
