using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blish_HUD;
using Gw2Sharp.WebApi.V2.Models;


namespace RaidClears.Features.Dungeons.Services;

public class DungeonsClearsService
{
    private const int FREQUENTER_ACHIEVEMENT_ID = 2963;
    
    public async Task<List<string>> GetFrequenterPaths()
    {
        var gw2ApiManager = Module.moduleInstance.Gw2ApiManager;
        var logger = Logger.GetLogger<Module>();

        if (gw2ApiManager.HasPermissions(_necessaryApiTokenPermissions) == false)
        {
            logger.Warn("HasPermissions() returned false. Possible reasons: " +
                        "API subToken does not have the necessary permissions: " +
                        $"{string.Join(", ", _necessaryApiTokenPermissions)}. " +
                        "Or module did not get API subToken from Blish yet. Or API key is missing.");

            return new List<string>();
        }

        try
        {
            var f = await gw2ApiManager.Gw2ApiClient.V2.Account.Achievements.GetAsync();
            var frequenter = f.ToList().Find(x => x.Id == FREQUENTER_ACHIEVEMENT_ID);

            var list = new List<string>();
            
            if (frequenter != null)
            {
                list = ConvertFrequenterToPathId(frequenter.Bits).ToList();
            }
            return list;
        }
        catch (Exception e)
        {
            logger.Warn(e, "Could not get current clears from API");
            return new List<string>();
        }
    }

    public async Task<List<string>> GetClearsFromApi()
    {
        var gw2ApiManager = Module.moduleInstance.Gw2ApiManager;
        var logger = Logger.GetLogger<Module>();

        if (gw2ApiManager.HasPermissions(_necessaryApiTokenPermissions) == false)
        {
            logger.Warn("HasPermissions() returned false. Possible reasons: " +
                        "API subToken does not have the necessary permissions: " +
                        $"{string.Join(", ", _necessaryApiTokenPermissions)}. " +
                        $"Or module did not get API subToken from Blish yet. Or API key is missing.");

            return new List<string>();
        }

        try
        {
            var weeklyCleared = await gw2ApiManager.Gw2ApiClient.V2.Account.Dungeons.GetAsync();
            return weeklyCleared.ToList();
        }
        catch (Exception e)
        {
            logger.Warn(e, "Could not get current clears from API");
            return new List<string>();
        }
    }

    private static IEnumerable<string> ConvertFrequenterToPathId(IEnumerable<int>? frequentedPaths)
    {
        return frequentedPaths?.Select(FrequentIdToPathString) ?? new List<string>();
    }
    
    private static string FrequentIdToPathString(int id)
    {
        return id switch
        {
            0 => "coe_story",
            1 => "submarine",
            2 => "teleporter",
            3 => "front_door",
            4 => "ac_story",
            5 => "hodgins",
            6 => "detha",
            7 => "tzark",
            8 => "jotun",
            9 => "mursaat",
            10 => "forgotten",
            11 => "seer",
            12 => "cm_story",
            13 => "asura",
            14 => "seraph",
            15 => "butler",
            16 => "se_story",
            17 => "fergg",
            18 => "rasalov",
            19 => "koptev",
            20 => "ta_story",
            21 => "leurent",
            22 => "vevina",
            23 => "aetherpath",
            24 => "hotw_story",
            25 => "butcher",
            26 => "plunderer",
            27 => "zealot",
            28 => "cof_story",
            29 => "ferrah",
            30 => "magg",
            31 => "rhiannon",
            _ => string.Empty,
        };
    }

    private readonly List<TokenPermission> _necessaryApiTokenPermissions = new()
    {
        TokenPermission.Account,
        TokenPermission.Progression
    };
}