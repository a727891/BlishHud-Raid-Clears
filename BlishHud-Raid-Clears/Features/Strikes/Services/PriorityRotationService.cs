using RaidClears.Features.Raids.Models;
using RaidClears.Features.Shared.Enums;
using RaidClears.Features.Shared.Enums.Extensions;
using RaidClears.Features.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using RaidClears.Localization;

namespace RaidClears.Features.Strikes.Services;

public class StrikeInfo
{
    public Encounter Encounter;
    public Encounters.StrikeMission TomorrowEncounter;
    public List<int> MapIds;

    public StrikeInfo(Encounters.StrikeMission mission, List<int> maps, Encounters.StrikeMission tomorrow)
    {
        Encounter = new(mission);
        MapIds = maps;
        TomorrowEncounter = tomorrow;
    }

}

public static class PriorityRotationService
{

    //private static int BOTH_AT_INDEX_0_TIMESTAMP = 1672617600; //Mon Jan 02 2023 00:00:00 GMT+0000
    private static int BOTH_AT_INDEX_0_TIMESTAMP = 1675123200; //Tue Jan 31 2023 00:00:00 GMT+0000
    private static int DAILY_SECONDS = 86400;
    private static int NUMBER_OF_IBS_STRIKES = 6;
    private static int NUMBER_OF_EOD_STRIKES = 5;


    public static IEnumerable<BoxModel> GetPriorityEncounters()
    {
        return GetPriorityStrikes().Select(e => new BoxModel($"priority_{e.Encounter.id}", $"{e.Encounter.name}\n\n{Strings.Strike_Tooltip_tomorrow}\n{e.TomorrowEncounter.GetLabel()}", e.Encounter.shortName));
    }
    public static IEnumerable<StrikeInfo> GetPriorityStrikes()
    {

        DateTimeOffset now = (DateTimeOffset)DateTime.UtcNow;

        var duration = now.ToUnixTimeSeconds() - BOTH_AT_INDEX_0_TIMESTAMP;

        var daysElapsed = (int)Math.Floor((decimal)(duration / DAILY_SECONDS));

        var ibs_index = daysElapsed % NUMBER_OF_IBS_STRIKES;
        var eod_index = daysElapsed % NUMBER_OF_EOD_STRIKES;

        return new List<StrikeInfo>()
        {
            IcebroodStrikeInfo(ibs_index),
            EndOfDragonsStrikeInfo(eod_index),
        };

    }

    public static StrikeInfo IcebroodStrikeInfo(int index)
    {

        /**
         * The Icebrood Saga
         * 0   Cold War  https://api.guildwars2.com/v2/maps?ids=1374,1376&lang=en
         * 1   Fraenir of Jormag https://api.guildwars2.com/v2/maps?ids=1341,1344&lang=en
         * 2   Shiverpeaks Pass https://api.guildwars2.com/v2/maps?ids=1331,1332&lang=en
         * 3   Voice of the Fallen and Claw of the Fallen https://api.guildwars2.com/v2/maps?ids=1340,1346&lang=en
         * 4   Whisper of Jormag https://api.guildwars2.com/v2/maps?ids=1357,1359&lang=en
         * 5   Boneskinner https://api.guildwars2.com/v2/maps?ids=1339,1351&lang=en
         **/
        switch (index) //Daily rotation index
        {
            case 0:
                return new StrikeInfo(Encounters.StrikeMission.ColdWar, new List<int> { 1374, 1376 }, Encounters.StrikeMission.Fraenir);
            case 1:
                return new StrikeInfo(Encounters.StrikeMission.Fraenir, new List<int> { 1341, 1344 }, Encounters.StrikeMission.ShiverpeaksPass);
            case 2:
                return new StrikeInfo(Encounters.StrikeMission.ShiverpeaksPass, new List<int> { 1331, 1332 }, Encounters.StrikeMission.VoiceAndClaw);
            case 3:
                return new StrikeInfo(Encounters.StrikeMission.VoiceAndClaw, new List<int> { 1340, 1346 }, Encounters.StrikeMission.Whisper);
            case 4:
                return new StrikeInfo(Encounters.StrikeMission.Whisper, new List<int> { 1357, 1359 }, Encounters.StrikeMission.Boneskinner);
            case 5:
                return new StrikeInfo(Encounters.StrikeMission.Boneskinner,  new List<int> { 1339, 1351 }, Encounters.StrikeMission.ColdWar);
            default: return new StrikeInfo(Encounters.StrikeMission.ShiverpeaksPass, new List<int> { }, Encounters.StrikeMission.ShiverpeaksPass);
        }
    }

    public static StrikeInfo EndOfDragonsStrikeInfo(int index)
    {
        /**
         * End of Dragons
         * 0	Aetherblade Hideout https://api.guildwars2.com/v2/maps?ids=1432&lang=en
         * 1	Xunlai Jade Junkyard https://api.guildwars2.com/v2/maps?ids=1450&lang=en
         * 2	Kaineng Overlook https://api.guildwars2.com/v2/maps?ids=1451&lang=en
         * 3	Harvest Temple https://api.guildwars2.com/v2/maps?ids=1437&lang=en
         * 4	Old Lion's Court https://api.guildwars2.com/v2/maps?ids=1485&lang=en
         **/

        switch (index)
        {
            case 0:
                return new StrikeInfo(Encounters.StrikeMission.AetherbladeHideout, new List<int> { 1432 }, Encounters.StrikeMission.Junkyard);
            case 1:
                return new StrikeInfo(Encounters.StrikeMission.Junkyard, new List<int> { 1450 }, Encounters.StrikeMission.Overlook);
            case 2:
                return new StrikeInfo(Encounters.StrikeMission.Overlook, new List<int> { 1451 }, Encounters.StrikeMission.HarvestTemple);
            case 3:
                return new StrikeInfo(Encounters.StrikeMission.HarvestTemple, new List<int> { 1437 }, Encounters.StrikeMission.OldLionsCourt);
            case 4:
                return new StrikeInfo(Encounters.StrikeMission.OldLionsCourt, new List<int> { 1485 }, Encounters.StrikeMission.AetherbladeHideout);

            default: return new StrikeInfo(Encounters.StrikeMission.AetherbladeHideout, new List<int> { }, Encounters.StrikeMission.AetherbladeHideout);
        }
    }


}