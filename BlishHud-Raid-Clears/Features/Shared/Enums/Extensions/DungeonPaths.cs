using System;
using static RaidClears.Features.Shared.Enums.Encounters;

namespace RaidClears.Features.Shared.Enums.Extensions;

public static class DungeonPathsExtensions
{
    public static string GetLabel(this DungeonPaths value)
    {
        return value switch
        {
            DungeonPaths.AscalonianCatacombsStory => "Story",
            DungeonPaths.AscalonianCatacombsHodgins => "hodgins",
            DungeonPaths.AscalonianCatacombsDetha => "detha",
            DungeonPaths.AscalonianCatacombsTzark => "tzark",

            DungeonPaths.CaudecusManorStory => "Story",
            DungeonPaths.CaudecusManorAsura => "asura",
            DungeonPaths.CaudecusManorSeraph => "seraph",
            DungeonPaths.CaudecusManorButler => "butler",

            DungeonPaths.TwilightArborStory => "Story",
            DungeonPaths.TwilightArborLeurent => "leurent (Up)",
            DungeonPaths.TwilightArborVevina => "vevina (Forward)",
            DungeonPaths.TwilightArborAetherPath => "aetherpath",

            DungeonPaths.SorrowsEmbraceStory => "Story",
            DungeonPaths.SorrowsEmbraceFergg => "fergg",
            DungeonPaths.SorrowsEmbraceRasalov => "rasalov",
            DungeonPaths.SorrowsEmbraceKoptev => "koptev",

            DungeonPaths.CitadelOfFlameStory => "Story",
            DungeonPaths.CitadelOfFlameFerrah => "ferrah",
            DungeonPaths.CitadelOfFlameMagg => "magg",
            DungeonPaths.CitadelOfFlameRhiannon => "rhiannon",

            DungeonPaths.HonorOfTheWavesStory => "Story",
            DungeonPaths.HonorOfTheWavesButcher => "butcher",
            DungeonPaths.HonorOfTheWavesPlunderer => "plunderer",
            DungeonPaths.HonorOfTheWavesZealot => "zealot",

            DungeonPaths.CrucibleOfEternityStory => "Story",
            DungeonPaths.CrucibleOfEternitySubmarine => "submarine",
            DungeonPaths.CrucibleOfEternityTeleporter => "teleporter",
            DungeonPaths.CrucibleOfEternityFrontDoor => "front_door",

            DungeonPaths.RuinedCityOfArahStory => "Story",
            DungeonPaths.RuinedCityOfArahJotun => "jotun",
            DungeonPaths.RuinedCityOfArahMursaat => "mursaat",
            DungeonPaths.RuinedCityOfArahForgotten => "forgotten",
            DungeonPaths.RuinedCityOfArahSeer => "seer",

        };
    }
    
    public static string GetLabelShort(this DungeonPaths value)
    {
        return value switch
        {
            DungeonPaths.AscalonianCatacombsStory => "S",
            DungeonPaths.AscalonianCatacombsHodgins => "H",
            DungeonPaths.AscalonianCatacombsDetha => "D",
            DungeonPaths.AscalonianCatacombsTzark => "T",

            DungeonPaths.CaudecusManorStory => "S",
            DungeonPaths.CaudecusManorAsura => "A",
            DungeonPaths.CaudecusManorSeraph => "S",
            DungeonPaths.CaudecusManorButler => "B",

            DungeonPaths.TwilightArborStory => "S",
            DungeonPaths.TwilightArborLeurent => "Up",
            DungeonPaths.TwilightArborVevina => "Fwd",
            DungeonPaths.TwilightArborAetherPath => "Ae",

            DungeonPaths.SorrowsEmbraceStory => "S",
            DungeonPaths.SorrowsEmbraceFergg => "F",
            DungeonPaths.SorrowsEmbraceRasalov => "R",
            DungeonPaths.SorrowsEmbraceKoptev => "K",

            DungeonPaths.CitadelOfFlameStory => "S",
            DungeonPaths.CitadelOfFlameFerrah => "F",
            DungeonPaths.CitadelOfFlameMagg => "M",
            DungeonPaths.CitadelOfFlameRhiannon => "R",

            DungeonPaths.HonorOfTheWavesStory => "S",
            DungeonPaths.HonorOfTheWavesButcher => "B",
            DungeonPaths.HonorOfTheWavesPlunderer => "P",
            DungeonPaths.HonorOfTheWavesZealot => "Z",

            DungeonPaths.CrucibleOfEternityStory => "S",
            DungeonPaths.CrucibleOfEternitySubmarine => "S",
            DungeonPaths.CrucibleOfEternityTeleporter => "T",
            DungeonPaths.CrucibleOfEternityFrontDoor => "F",

            DungeonPaths.RuinedCityOfArahStory => "S",
            DungeonPaths.RuinedCityOfArahJotun => "J",
            DungeonPaths.RuinedCityOfArahMursaat => "M",
            DungeonPaths.RuinedCityOfArahForgotten => "F",
            DungeonPaths.RuinedCityOfArahSeer => "S",

            _ => throw new ArgumentOutOfRangeException(nameof(value), value, null)
        };
    }

    public static string GetApiLabel(this DungeonPaths value)
    {
        return value switch
        {
            DungeonPaths.AscalonianCatacombsStory => "",
            DungeonPaths.AscalonianCatacombsHodgins => "",
            DungeonPaths.AscalonianCatacombsDetha => "",
            DungeonPaths.AscalonianCatacombsTzark => "",
            /*  "AC",
                  new Path("ac_story","Story", "S"),
                  new Path("hodgins","hodgins", "E1"),
                  new Path("detha","detha", "E2"),
                  new Path("tzark","tzark", "E3"),*/
            DungeonPaths.CaudecusManorStory => "",
            DungeonPaths.CaudecusManorAsura => "",
            DungeonPaths.CaudecusManorSeraph => "",
            DungeonPaths.CaudecusManorButler => "",
            /*"CM",
                new Path("cm_story","Story", "S"),
                new Path("asura","asura", "E1"),
                new Path("seraph","seraph", "E2"),
                new Path("butler","butler", "E3"),*/
            DungeonPaths.TwilightArborStory => "",
            DungeonPaths.TwilightArborLeurent => "",
            DungeonPaths.TwilightArborVevina => "",
            DungeonPaths.TwilightArborAetherPath => "",
            /*"TA",
                new Path("ta_story","Story", "S"),
                new Path("leurent","leurent (Up)", "Up"),
                new Path("vevina","vevina (Forward)", "Fwd"),
                new Path("aetherpath","aetherpath", "Ae"),*/
            DungeonPaths.SorrowsEmbraceStory => "",
            DungeonPaths.SorrowsEmbraceFergg => "",
            DungeonPaths.SorrowsEmbraceRasalov => "",
            DungeonPaths.SorrowsEmbraceKoptev => "",
            /*"SE",
                new Path("se_story","Story", "S"),
                new Path("fergg","fergg", "E1"),
                new Path("rasalov","rasalov", "E2"),
                new Path("koptev","koptev", "E3"),*/
            DungeonPaths.CitadelOfFlameStory => "",
            DungeonPaths.CitadelOfFlameFerrah => "",
            DungeonPaths.CitadelOfFlameMagg => "",
            DungeonPaths.CitadelOfFlameRhiannon => "",
            /*"CoF",
                new Path("cof_story","Story", "S"),
                new Path("ferrah","ferrah", "E1"),
                new Path("magg","magg", "E2"),
                new Path("rhiannon","rhiannon", "E3"),*/
            DungeonPaths.HonorOfTheWavesStory => "",
            DungeonPaths.HonorOfTheWavesButcher => "",
            DungeonPaths.HonorOfTheWavesPlunderer => "",
            DungeonPaths.HonorOfTheWavesZealot => "",
            /*"HW",
                new Path("hotw_story","Story", "S"),
                new Path("butcher","butcher", "E1"),
                new Path("plunderer","plunderer", "E2"),
                new Path("zealot","zealot", "E3"),*/
            DungeonPaths.CrucibleOfEternityStory => "",
            DungeonPaths.CrucibleOfEternitySubmarine => "",
            DungeonPaths.CrucibleOfEternityTeleporter => "",
            DungeonPaths.CrucibleOfEternityFrontDoor => "",
            /*"CoE",
                new Path("coe_story","Story", "S"),
                new Path("submarine","submarine", "E1"),
                new Path("teleporter","teleporter", "E2"),
                new Path("front_door","front_door", "E3"),*/
            DungeonPaths.RuinedCityOfArahStory => "",
            DungeonPaths.RuinedCityOfArahJotun => "",
            DungeonPaths.RuinedCityOfArahMursaat => "",
            DungeonPaths.RuinedCityOfArahForgotten => "",
            DungeonPaths.RuinedCityOfArahSeer => "",
            /*"Arah",
                //new Path("arah_story","Story", "S"),
                new Path("jotun","jotun", "E1"),
                new Path("mursaat","mursaat", "E2"),
                new Path("forgotten","forgotten", "E3"),
                new Path("seer","seer", "E4"),*/
            _ => throw new ArgumentOutOfRangeException(nameof(value), value, null)
        };
    }
}