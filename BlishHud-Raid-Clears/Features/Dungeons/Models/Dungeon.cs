using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Blish_HUD.Settings;
using Gw2Sharp.WebApi.V2.Models;
using RaidClears.Features.Shared.Controls;
using RaidClears.Features.Shared.Models;
using RaidClears.Settings.Models;
using RaidClears.Utils;
using static RaidClears.Features.Shared.Enums.Encounters;

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
            if(dungeon.index == FrequenterIndex)
            {
                group.VisiblityChanged(Settings.DungeonFrequenterVisible);
            }
            else
            {
                group.VisiblityChanged(Settings.DungeonPaths.ElementAt(dungeon.index));
            }
            dungeon.SetGridGroupReference(group);

            var labelBox = new GridBox(
                group,
                dungeon.shortName, dungeon.name,
                Settings.Style.LabelOpacity, Settings.Style.FontSize
            );
            labelBox.LayoutChange(Settings.Style.Layout);
            dungeon.SetGroupLabelReference(labelBox);
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
                    new Path(DungeonPaths.AscalonianCatacombsStory),
                    new Path(DungeonPaths.AscalonianCatacombsHodgins),
                    new Path(DungeonPaths.AscalonianCatacombsDetha),
                    new Path(DungeonPaths.AscalonianCatacombsTzark),
                }
            ),
            new Dungeon(
                $"Caudecus Manor\nStory {40}, Explore {45}",
                1,
                "CM",
                new BoxModel[]
                {
                    new Path(DungeonPaths.CaudecusManorStory),
                    new Path(DungeonPaths.CaudecusManorAsura),
                    new Path(DungeonPaths.CaudecusManorSeraph),
                    new Path(DungeonPaths.CaudecusManorButler),
                }
            ),
            new Dungeon(
                $"Twilight Arbor\nStory {50}, Explore {55}",
                2,
                "TA",
                new BoxModel[]
                {
                    new Path(DungeonPaths.TwilightArborStory),
                    new Path(DungeonPaths.TwilightArborLeurent),
                    new Path(DungeonPaths.TwilightArborVevina),
                    new Path(DungeonPaths.TwilightArborAetherPath),
                }
            ),
            new Dungeon(
                $"Sorrows Embrace\nStory {60}, Explore {65}",
                3,
                "SE",
                new BoxModel[]
                {
                    new Path(DungeonPaths.SorrowsEmbraceStory),
                    new Path(DungeonPaths.SorrowsEmbraceFergg),
                    new Path(DungeonPaths.SorrowsEmbraceRasalov),
                    new Path(DungeonPaths.SorrowsEmbraceKoptev),
                }
            ),
            new Dungeon(
                $"Citadel of Flame\nStory {70}, Explore {75}",
                4,
                "CoF",
                new BoxModel[]
                {
                    new Path(DungeonPaths.CitadelOfFlameStory),
                    new Path(DungeonPaths.CitadelOfFlameFerrah),
                    new Path(DungeonPaths.CitadelOfFlameMagg),
                    new Path(DungeonPaths.CitadelOfFlameRhiannon),
                }
            ),
            new Dungeon(
                $"Honor of the Waves\nStory {76}, Explore {80}",
                5,
                "HW",
                new BoxModel[]
                {
                    new Path(DungeonPaths.HonorOfTheWavesStory),
                    new Path(DungeonPaths.HonorOfTheWavesButcher),
                    new Path(DungeonPaths.HonorOfTheWavesPlunderer),
                    new Path(DungeonPaths.HonorOfTheWavesZealot),
                }
            ),
            new Dungeon(
                $"Crucible of Eternity\nStory {78}, Explore {80}",
                6,
                "CoE",
                new BoxModel[]
                {
                    new Path(DungeonPaths.CrucibleOfEternityStory),
                    new Path(DungeonPaths.CrucibleOfEternitySubmarine),
                    new Path(DungeonPaths.CrucibleOfEternityTeleporter),
                    new Path(DungeonPaths.CrucibleOfEternityFrontDoor),
                }
            ),
            new Dungeon(
                $"Ruined City of Arah\nExplore {80}",
                7,
                "Arah",
                new BoxModel[]
                {
                    //new Path("arah_story","Story", "S"),
                    new Path(DungeonPaths.RuinedCityOfArahJotun),
                    new Path(DungeonPaths.RuinedCityOfArahMursaat),
                    new Path(DungeonPaths.RuinedCityOfArahForgotten),
                    new Path(DungeonPaths.RuinedCityOfArahSeer),
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
