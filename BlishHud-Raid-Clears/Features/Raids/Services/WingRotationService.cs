using System;

namespace RaidClears.Features.Raids.Services;

public class WeeklyWings
{
    public int Emboldened { get; }
    public int CallOfTheMist { get; }
    public WeeklyWings(int emboldened, int callOfTheMist)
    {
        Emboldened = emboldened;
        CallOfTheMist = callOfTheMist;
    }
}

public static class WingRotationService
{
    private const int EMBOLDEN_START_TIMESTAMP = 1656315000;
    private const int WEEKLY_SECONDS = 604800;
    private const int NUMBER_OF_WINGS = 7;

    public static WeeklyWings GetWeeklyWings()
    {
        var now = (DateTimeOffset)DateTime.UtcNow;

        var duration = now.ToUnixTimeSeconds() - EMBOLDEN_START_TIMESTAMP;

        var wing = (int)Math.Floor((decimal)duration / WEEKLY_SECONDS) % NUMBER_OF_WINGS;

        return new WeeklyWings(wing, (wing + 1) % NUMBER_OF_WINGS);
    }
}