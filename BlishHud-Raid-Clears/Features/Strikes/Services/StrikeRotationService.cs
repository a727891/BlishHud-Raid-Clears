using System;
using System.Collections.Generic;

namespace RaidClears.Features.Strikes.Services;

public class StrikeInfo
{
    public string name;
    public string shorwName;
    public string expansion;
    public List<int> mapIds;

    public StrikeInfo(string name, string shortName, string expansion, List<int> maps)
    {
        this.name = name;
        this.expansion = expansion;
        this.shorwName = shortName;
        this.mapIds = maps;
    }


}
public class StrikeRotationService : IDisposable
{

    private const int BOTH_AT_INDEX_0_TIMESTAMP = 1672617600; //Mon Jan 02 2023 00:00:00 GMT+0000
    private const int DAILY_SECONDS = 86400;
    private const int NUMBER_OF_IBS_STRIKES = 6;
    private const int NUMBER_OF_EOD_STRIKES = 5;


    public StrikeRotationService()
    {


    }

    public (int IBS_INDEX, int EOD_INDEX) GetPriorityStikeIndex()
    {

        DateTimeOffset now = (DateTimeOffset)DateTime.UtcNow;

        var duration = now.ToUnixTimeSeconds() - BOTH_AT_INDEX_0_TIMESTAMP;

        var daysElapsed = (int)Math.Floor((decimal)(duration / DAILY_SECONDS));

        var ibs_index = daysElapsed % NUMBER_OF_IBS_STRIKES;
        var eod_index = daysElapsed % NUMBER_OF_EOD_STRIKES;

        return (ibs_index, eod_index);

    }

    public StrikeInfo IcebroodStrikeInfo(int index)
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
        switch (index)
        {
            case 0:
                return new StrikeInfo("Cold War", "CW", "IcebroodSaga", new List<int> { 1374, 1376 });
            case 1:
                return new StrikeInfo("Fraenir of Jormag", "Fr", "IcebroodSaga", new List<int> { 1341, 1344 });
            case 2:
                return new StrikeInfo("Shiverpeaks Pass", "SP", "IcebroodSaga", new List<int> { 1331, 1332 });
            case 3:
                return new StrikeInfo("Voice of the Fallen and Claw of the Fallen", "V&C", "IcebroodSaga", new List<int> { 1340, 1346 });
            case 4:
                return new StrikeInfo("Whisper of Jormag", "WoJ", "IcebroodSaga", new List<int> { 1357, 1359 });
            case 5:
                return new StrikeInfo("Boneskinner", "BS", "IcebroodSaga", new List<int> { 1339, 1351 });
            default: return new StrikeInfo("N/A", "?", "IcebroodSaga", new List<int> { });
        }
    }

    public StrikeInfo EndOfDragonsStrikeInfo(int index)
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
                return new StrikeInfo("Aetherblade Hideout", "AH", "End of Dragons", new List<int> { 1432 });
            case 1:
                return new StrikeInfo("Xunlai Jade Junkyard", "XJJ", "End of Dragons", new List<int> { 1450 });
            case 2:
                return new StrikeInfo("Kaineng Overlook", "KO", "End of Dragons", new List<int> { 1451 });
            case 3:
                return new StrikeInfo("Harvest Temple", "HT", "End of Dragons", new List<int> { 1437 });
            case 4:
                return new StrikeInfo("Old Lion's Court", "OLC", "End of Dragons", new List<int> { 1485 });

            default: return new StrikeInfo("N/A", "?", "End of Dragons", new List<int> { });
        }
    }

    public void Dispose()
    {


    }

}