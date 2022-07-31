using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Modules.Managers;
using Gw2Sharp.WebApi.V2.Models;
using Gw2Sharp.WebApi.V2.Clients;
using RaidClears.Dungeons.Model;

namespace RaidClears.Dungeons.Services

{
    public static class DungeonsClearsService
    {
        const int FREQUENTER_ACHIEVEMENT_ID = 2963;
        public static async Task<(ApiDungeons, bool apiAccessFailed)> GetDungeonClearsFromApi( Gw2ApiManager gw2ApiManager,
                                                                                     Logger logger)
        {
            if (gw2ApiManager.HasPermissions(NECESSARY_API_TOKEN_PERMISSIONS) == false)
            {
                logger.Warn("HasPermissions() returned false. Possible reasons: " +
                            "API subToken does not have the necessary permissions: " +
                            $"{String.Join(", ", NECESSARY_API_TOKEN_PERMISSIONS)}. " +
                            $"Or module did not get API subToken from Blish yet. Or API key is missing.");

                return (new ApiDungeons(), true);
            }

            try
            {
                return (await  GetCurrentClearsFromApi(gw2ApiManager, logger), false);
            }
            catch (Exception e)
            {
                logger.Error(e, "Could not get current clears from API");
                return (new ApiDungeons(), true);
            }
        }

        private static async Task<ApiDungeons> GetCurrentClearsFromApi(Gw2ApiManager gw2ApiManager,
                                                                    Logger logger)
        {
            var weeklyCleared = await gw2ApiManager.Gw2ApiClient.V2.Account.Dungeons.GetAsync();
            var f = await gw2ApiManager.Gw2ApiClient.V2.Account.Achievements.GetAsync();
            var frequenter = f.ToList().Find(x => x.Id == FREQUENTER_ACHIEVEMENT_ID);

            var list = new List<string> { };
            if(frequenter != null)
            {
                list = ConvertFrequenterToPathId(frequenter.Bits.ToList());
            }
            

            return new ApiDungeons(weeklyCleared.ToList(), list);

        }

        public static readonly List<TokenPermission> NECESSARY_API_TOKEN_PERMISSIONS = new List<TokenPermission>
        {
            TokenPermission.Account,
            TokenPermission.Progression
        };


        public static List<string> ConvertFrequenterToPathId(List<int> frequentedPaths)
        {
            var list = new List<string> { };

            foreach(var path in frequentedPaths)
            {
                list.Add(FrequentIdToPathString(path));
            }

            return list;
        }

        private static string FrequentIdToPathString(int id)
        {
            switch (id)
            {
                case 0: return "coe_story";
                case 1: return "submarine";
                case 2: return "teleporter";
                case 3: return "front_door";
                case 4: return "ac_story";
                case 5: return "hodgins";
                case 6: return "detha";
                case 7: return "tzark";
                case 8: return "jotun";
                case 9: return "mursaat";
                case 10: return "forgotten";
                case 11: return "seer";
                case 12: return "cm_story";
                case 13: return "asura";
                case 14: return "seraph";
                case 15: return "butler";
                case 16: return "se_story";
                case 17: return "fergg";
                case 18: return "rasalov";
                case 19: return "koptev";
                case 20: return "ta_story";
                case 21: return "leurent";
                case 22: return "vevina";
                case 23: return "aetherpath";
                case 24: return "hotw_story";
                case 25: return "butcher";
                case 26: return "plunderer";
                case 27: return "zealot";
                case 28: return "cof_story";
                case 29: return "ferrah";
                case 30: return "magg";
                case 31: return "rhiannon";
                default:return null;
            }
        }
    }
}
