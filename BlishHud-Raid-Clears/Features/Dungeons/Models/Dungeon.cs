using System.Collections.Generic;
using System.Linq;
using Blish_HUD.Settings;
using RaidClears.Features.Shared.Controls;
using RaidClears.Features.Shared.Models;
using RaidClears.Settings.Models;
using RaidClears.Utils;

namespace RaidClears.Features.Dungeons.Models;

public static class DungeonFactory
{
    private static DungeonSettings Settings => Service.Settings.DungeonSettings;
    
    public const int FrequenterIndex = 8;
    public const string FrequenterID = "freq";
    
    public static IEnumerable<Dungeon> Create(DungeonPanel panel)
    {
        var dungeons = GetDungeonMetaData();
        foreach (var dungeon in dungeons)
        {
            var group = new GridGroup(
                panel,
                Settings.Style.Layout
            );
            group.VisiblityChanged(GetDungeonSelectionByIndex(dungeon.index, Settings));
            dungeon.SetGridGroupReference(group);

            var labelBox = new GridBox(
                group,
                dungeon.shortName, dungeon.name,
                Settings.Style.LabelOpacity, Settings.Style.FontSize
            );
            labelBox.LayoutChange(Settings.Style.Layout);
            dungeon.SetGroupLabelReference(labelBox);
            //ApplyConditionalTextColoring(labelBox, dungeon.index, weekly, settings);
            labelBox.LabelDisplayChange(Settings.Style.LabelDisplay, (dungeon.index + 1).ToString(), dungeon.shortName);

            foreach (var encounter in dungeon.boxes.OfType<Path>())
            {
                var encounterBox = new GridBox(
                    group,
                    encounter.shortName, encounter.name,
                    Settings.Style.GridOpacity, Settings.Style.FontSize
                );
                encounter.SetGridBoxReference(encounterBox);
                encounter.WatchColorSettings(Settings.Style.Color.Cleared, Settings.Style.Color.NotCleared);
                encounter.RegisterFrequenterSettings(Settings.DungeonHighlightFrequenter, Settings.DungeonPanelColorFreq, Settings.Style.Color.Text);
                //ApplyConditionalTextColoring(encounterBox, dungeon.index, weekly, settings);
            }
        }

        return dungeons;
    }

    private static SettingEntry<bool> GetDungeonSelectionByIndex(int index, DungeonSettings settings) => settings.DungeonPaths.ElementAt(index);

    private static Dungeon[] GetDungeonMetaData()
    {
        return new[]
        {
            new Dungeon(
                $"Ascalonian Catacombs\nStory {30}, Explore {35}",
                0,
                "AC",
                new BoxModel[]
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
                new BoxModel[]
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
                new BoxModel[]
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
                new BoxModel[]
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
                new BoxModel[]
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
                new BoxModel[]
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
                new BoxModel[]
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
                new BoxModel[]
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
                FrequenterIndex,
                "Freq",
                new BoxModel[]
                {
                    //new Path("arah_story","Story", "S"),
                    new Path(FrequenterID,"Frequenter Achievement Paths Finished", "0/8"),
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
