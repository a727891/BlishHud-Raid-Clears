
using Blish_HUD.Controls;
using Blish_HUD.Settings;
using RaidClears.Features.Dungeons;
using RaidClears.Features.Raids.Models;
using RaidClears.Features.Shared.Controls;
using RaidClears.Features.Shared.Models;
using RaidClears.Localization;
using RaidClears.Raids.Services;
using RaidClears.Settings;
using RaidClears.Utils;

namespace RaidClears.Features.Dungeons.Models
{
    
    public static class DungeonFactory
    {
        public static int FREQUENTER_INDEX = 8;
        public static string FREQUENTER_ID = "freq";
        public static Dungeon[] Create(DungeonPanel panel)
        {
            SettingService settings = Module.ModuleInstance.SettingsService;
            Dungeon[] dungeons = GetDungeonMetaData();
            foreach (var dungeon in dungeons)
            {
                GridGroup group = new GridGroup(
                    panel,
                    settings.DungeonPanelLayout
                );
                group.VisiblityChanged(GetDungeonSelectionByIndex(dungeon.index, settings));
                dungeon.SetGridGroupReference(group);


                GridBox labelBox = new GridBox(
                    group,
                    dungeon.shortName, dungeon.name,
                    settings.DungeonPanelLabelOpacity, settings.DungeonPanelFontSize
                );
                labelBox.LayoutChange(settings.DungeonPanelLayout);
                dungeon.SetGroupLabelReference(labelBox);
                //ApplyConditionalTextColoring(labelBox, dungeon.index, weekly, settings);
                labelBox.LabelDisplayChange(settings.DungeonPanelLabelDisplay, (dungeon.index + 1).ToString(), dungeon.shortName);

                foreach (var encounter in dungeon.boxes as Path[])
                {
                    GridBox encounterBox = new GridBox(
                        group,
                        encounter.short_name, encounter.name,
                        settings.DungeonPanelGridOpacity, settings.DungeonPanelFontSize
                    );
                    encounter.SetGridBoxReference(encounterBox);
                    encounter.WatchColorSettings(settings.DungeonPanelColorCleared, settings.DungeonPanelColorNotCleared);
                    encounter.RegisterFrequenterSettings(settings.dungeonHighlightFrequenter, settings.DungeonPanelColorFreq, settings.DungeonPanelColorText);
                    //ApplyConditionalTextColoring(encounterBox, dungeon.index, weekly, settings);

                }

            }

            return dungeons;
        }

        public static SettingEntry<bool> GetDungeonSelectionByIndex(int index, SettingService settings)
        {
            switch (index)
            {
                case 0: return settings.D1IsVisible;
                case 1: return settings.D2IsVisible;
                case 2: return settings.D3IsVisible;
                case 3: return settings.D4IsVisible;
                case 4: return settings.D5IsVisible;
                case 5: return settings.D6IsVisible;
                case 6: return settings.D7IsVisible;
                case 7: return settings.D8IsVisible;
                case 8: return settings.DFIsVisible;
                default: return settings.D1IsVisible;
            }
        }

        public static Dungeon[] GetDungeonMetaData()
        {
            return new Dungeon[]
            {
                new Dungeon(
                    $"Ascalonian Catacombs\nStory {30}, Explore {35}",
                    0,
                    "AC",
                    new Path[]
                    {
                        new Path("ac_story","Story", "S"),
                        new Path("hodgins","hodgins", "E1"),
                        new Path("detha","detha", "E2"),
                        new Path("tzark","tzark", "E3"),
                    }
                ),
                new Dungeon(
                    $"Caudecus Manor\nStory {40}, Explore {45}",
                    1,
                    "CM",
                    new Path[]
                    {
                        new Path("cm_story","Story", "S"),
                        new Path("asura","asura", "E1"),
                        new Path("seraph","seraph", "E2"),
                        new Path("butler","butler", "E3"),
                    }
                ),
                new Dungeon(
                    $"Twilight Arbor\nStory {50}, Explore {55}",
                    2,
                    "TA",
                    new Path[]
                    {
                        new Path("ta_story","Story", "S"),
                        new Path("leurent","leurent (Up)", "Up"),
                        new Path("vevina","vevina (Forward)", "Fwd"),
                        new Path("aetherpath","aetherpath", "Ae"),
                    }
                ),
                new Dungeon(
                    $"Sorrows Embrace\nStory {60}, Explore {65}",
                    3,
                    "SE",
                    new Path[]
                    {
                        new Path("se_story","Story", "S"),
                        new Path("fergg","fergg", "E1"),
                        new Path("rasalov","rasalov", "E2"),
                        new Path("koptev","koptev", "E3"),
                    }
                ),
                new Dungeon(
                    $"Citadel of Flame\nStory {70}, Explore {75}",
                    4,
                    "CoF",
                    new Path[]
                    {
                        new Path("cof_story","Story", "S"),
                        new Path("ferrah","ferrah", "E1"),
                        new Path("magg","magg", "E2"),
                        new Path("rhiannon","rhiannon", "E3"),
                    }
                ),
                new Dungeon(
                    $"Honor of the Waves\nStory {76}, Explore {80}",
                    5,
                    "HW",
                    new Path[]
                    {
                        new Path("hotw_story","Story", "S"),
                        new Path("butcher","butcher", "E1"),
                        new Path("plunderer","plunderer", "E2"),
                        new Path("zealot","zealot", "E3"),
                    }
                ),
                new Dungeon(
                    $"Crucible of Eternity\nStory {78}, Explore {80}",
                    6,
                    "CoE",
                    new Path[]
                    {
                        new Path("coe_story","Story", "S"),
                        new Path("submarine","submarine", "E1"),
                        new Path("teleporter","teleporter", "E2"),
                        new Path("front_door","front_door", "E3"),
                    }
                ),
                new Dungeon(
                    $"Ruined City of Arah\nExplore {80}",
                    7,
                    "Arah",
                    new Path[]
                    {
                        //new Path("arah_story","Story", "S"),
                        new Path("jotun","jotun", "E1"),
                        new Path("mursaat","mursaat", "E2"),
                        new Path("forgotten","forgotten", "E3"),
                        new Path("seer","seer", "E4"),
                    }
                ),
                new Dungeon(
                    $"Frequenter Achievement Summary",
                    FREQUENTER_INDEX,
                    "Freq",
                    new Path[]
                    {
                        //new Path("arah_story","Story", "S"),
                        new Path(FREQUENTER_ID,"Frequenter Achievement Paths Finished", "0/8"),
                    }
                )
            };
        }
    }
    public class Dungeon : GroupModel
    {

        public Dungeon(string name, int index, string shortName, BoxModel[] boxes) : base(name, index, shortName, boxes)
        {
        }


    }
}
