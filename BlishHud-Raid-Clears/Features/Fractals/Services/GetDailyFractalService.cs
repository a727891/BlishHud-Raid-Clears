using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Modules.Managers;
using Gw2Sharp.WebApi.V2.Models;

namespace RaidClears.Features.Fractals.Services;

public static class GetDailyFractalService
{
    public static async Task<(List<Achievement> t4s, List<Achievement> recs)> GetDailyFractals( Gw2ApiManager gw2ApiManager,
                                                                                 Logger logger)
    {
        var recs = new List<Achievement>();
        var t4s = new List<Achievement>();

        try
        {
            var dailies = await gw2ApiManager.Gw2ApiClient.V2.Achievements.Daily.GetAsync();

            var fractal_achievement_list = dailies.Fractals.Select(x => x.Id).ToArray();

            var fractals = await gw2ApiManager.Gw2ApiClient.V2.Achievements.ManyAsync(fractal_achievement_list);

            

            foreach(var fractal in fractals)
            {
                if( Regex.Match(fractal.Name, "Daily Recommended").Success)
                {
                    recs.Add(fractal);
                }else if( Regex.Match(fractal.Name, "Daily Tier 4").Success)
                {
                    t4s.Add(fractal);
                }
                
            }

            recs.Sort((x, y) => x.Name.CompareTo(y.Name));
            t4s.Sort((x, y) => x.Name.CompareTo(y.Name));

           
        }
        catch (Exception e)
        {
            logger.Warn(e, "Could not get fractals from API");

        }
        return (t4s, recs);
    }


    public static (string shortName, string longName) TierFourDisplayName(string achievementName)
    {
        var name = Regex.Match(achievementName, "Daily Tier 4 (.+)");
        if (name.Captures.Count > 0)
        {
            var shortName = name.Captures[0].Value switch
            {
                "Aetherblade" => "aeth",
                "Aquatic Ruins" => "aqua",
                "Captain Mai Trin Boss" => "mai",
                "Chaos" => "chaos",
                "Cliffside" => "cliff",
                "Deepstone" => "deep",
                "Molten Boss" => "m boss",
                "Molten Furnace" => "m furn",
                "Nightmare" => "night",
                "Shattered Observatory" => "s-obs",
                "Siren's Reef" => "siren",
                "Snowblind" => "snow",
                "Sunqua Peak" => "sun",
                "Solid Ocean" => "solid",
                "Swampland" => "swamp",
                "Thaumanova Reactor" => "thau",
                "Twilight Oasis" => "twili",
                "Uncategorized" => "uncat",
                "Underground Facility" => "under",
                "Urban Battleground" => "urban",
                "Volcanic" => "volc",
                _ => "???",
            };
            return (shortName, name.Captures[0].Value);
        }
        else
        {
            return ("???","Unknown error");
        }
    }

}