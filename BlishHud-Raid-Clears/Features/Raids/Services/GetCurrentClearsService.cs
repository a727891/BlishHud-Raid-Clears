using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Modules.Managers;
using Gw2Sharp.WebApi.V2.Models;


namespace RaidClears.Fearures.Raids.Services
{
    public class GetCurrentClearsService
    {
        public GetCurrentClearsService() { 

        }


        public async Task<List<string>> GetClearsFromApi()
        {
            Gw2ApiManager gw2ApiManager = Module.ModuleInstance.Gw2ApiManager ;
            Logger logger = Logger.GetLogger<Module>();

            if (gw2ApiManager.HasPermissions(NECESSARY_API_TOKEN_PERMISSIONS) == false)
            {
                logger.Warn("HasPermissions() returned false. Possible reasons: " +
                            "API subToken does not have the necessary permissions: " +
                            $"{String.Join(", ", NECESSARY_API_TOKEN_PERMISSIONS)}. " +
                            $"Or module did not get API subToken from Blish yet. Or API key is missing.");

                return new List<string>();
            }

            try
            {
                var weeklyCleared = await gw2ApiManager.Gw2ApiClient.V2.Account.Raids.GetAsync();

                return weeklyCleared.ToList();
            }
            catch (Exception e)
            {
                logger.Warn(e, "Could not get current clears from API");
                return new List<string>();
            }
        }

        public readonly List<TokenPermission> NECESSARY_API_TOKEN_PERMISSIONS = new List<TokenPermission>
        {
            TokenPermission.Account,
            TokenPermission.Progression
        };
    }
}