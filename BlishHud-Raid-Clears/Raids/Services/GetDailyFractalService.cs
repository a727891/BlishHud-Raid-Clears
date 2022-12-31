using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Modules.Managers;
using Gw2Sharp.WebApi.V2.Models;
using RaidClears.Raids.Model;

namespace RaidClears.Raids.Services
{
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
                var shortName = "";
                switch (name.Captures[0].Value)
                {
                    case "Aetherblade": shortName = "aeth"; break;
                    case "Aquatic Ruins": shortName = "aqua"; break;
                    case "Captain Mai Trin Boss": shortName = "mai"; break;
                    case "Chaos": shortName = "chaos"; break;
                    case "Cliffside": shortName = "cliff"; break;
                    case "Deepstone": shortName = "deep"; break;
                    case "Molten Boss": shortName = "m boss"; break;
                    case "Molten Furnace": shortName = "m furn"; break;
                    case "Nightmare": shortName = "night"; break;
                    case "Shattered Observatory": shortName = "s-obs"; break;
                    case "Siren's Reef": shortName = "siren"; break;
                    case "Snowblind": shortName = "snow"; break;
                    case "Sunqua Peak": shortName = "sun"; break;
                    case "Solid Ocean": shortName = "solid"; break;
                    case "Swampland": shortName = "swamp";  break;
                    case "Thaumanova Reactor": shortName = "thau"; break;
                    case "Twilight Oasis": shortName = "twili"; break;
                    case "Uncategorized": shortName = "uncat"; break;
                    case "Underground Facility": shortName = "under"; break;
                    case "Urban Battleground": shortName = "urban"; break;
                    case "Volcanic": shortName = "volc"; break;
                    default: shortName = "???";break;
                }
                return (shortName, name.Captures[0].Value);
            }
            else
            {
                return ("???","Unknown error");
            }
        }

    }
}