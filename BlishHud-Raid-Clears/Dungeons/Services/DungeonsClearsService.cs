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
            /*IAccountAchievementsClient achClient = gw2ApiManager.Gw2ApiClient.V2.Account.Achievements;
            var frequenter = await achClient.GetAsync(2963,null);*/
      
            return new ApiDungeons(weeklyCleared.ToList());

        }

        public static readonly List<TokenPermission> NECESSARY_API_TOKEN_PERMISSIONS = new List<TokenPermission>
        {
            TokenPermission.Account,
            TokenPermission.Progression
        };
    }
}
