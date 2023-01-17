using System;

namespace RaidClears.Features.Raids.Services;

public class WeeklyWings
{
    public int Emboldened { get; private set; } = 0;
    public int CallOfTheMist { get; private set; } = 1;
    public WeeklyWings(int emboldened, int callOfTheMist)
    {
        Emboldened = emboldened;
        CallOfTheMist = callOfTheMist;
    }
}

public class WingRotationService
{

    private readonly static int EMBOLDEN_START_TIMESTAMP = 1656315000;
    private readonly static int WEEKLY_SECONDS = 604800;
    private readonly static int NUMBER_OF_WINGS = 7;


    public static WeeklyWings GetWeeklyWings()
    {

        DateTimeOffset now = (DateTimeOffset)DateTime.UtcNow;

        var duration = now.ToUnixTimeSeconds() - EMBOLDEN_START_TIMESTAMP;

        var wing = (int)Math.Floor((decimal)(duration / WEEKLY_SECONDS)) % NUMBER_OF_WINGS;

        return new WeeklyWings(wing, (wing + 1) % NUMBER_OF_WINGS);

    }

}