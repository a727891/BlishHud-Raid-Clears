using System;
using System.Collections.Generic;
using Blish_HUD.Settings;
using RaidClears.Settings.Services;

namespace RaidClears.Features.Strikes.Services
{
    public class StrikeSettingService : IDisposable
    {

        private SettingService settings;
        
        public StrikeSettingService()
        {
            settings = Module.ModuleInstance.SettingsService;

        }

        public static SettingEntry<bool> GetStrikeVisibleFromEncounterId(string id)
        {
            var setting = Module.ModuleInstance.SettingsService;
            switch (id) {
                case "cold_war":
                    return setting.StrikeVisible_CW;
                case "fraenir_of_jormag":
                    return setting.StrikeVisible_FoJ;
                case "shiverpeak_pass":
                    return setting.StrikeVisible_SP;
                case "voice_and_claw":
                    return setting.StrikeVisible_VandC;
                case "whisper_of_jormag":
                    return setting.StrikeVisible_WoJ;
                case "boneskinner":
                    return setting.StrikeVisible_BS;
                case "aetherblade_hideout":
                    return setting.StrikeVisible_AH;
                case "xunlai_jade_junkyard":
                    return setting.StrikeVisible_XJJ;
                case "kaineng_overlook":
                    return setting.StrikeVisible_KO;
                case "harvest_temple":
                    return setting.StrikeVisible_HT;
                case "old_lion_court":
                    return setting.StrikeVisible_OLC;
                default: return setting.StrikeVisible_CW; 
            }
        }

        public static SettingEntry<bool> GetStrikeGroupVisibleSettingByIndex(int id)
        {
            var setting = Module.ModuleInstance.SettingsService;
            switch (id)
            {
                case 8: return setting.StrikeVisible_IBS;
                case 9: return setting.StrikeVisible_EOD;
                case 10: return setting.StrikeVisible_Priority;
                default: return setting.StrikeVisible_Priority;
            }
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
}