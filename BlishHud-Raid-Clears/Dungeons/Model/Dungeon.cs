using System.Collections.Generic;
using RaidClears.Dungeons.Controls;
using System.Linq;

namespace RaidClears.Dungeons.Model
{
    public class Dungeon
    {
        public int index;
        public string name;
        public int storyLevel;
        public int exploreLevel;
        public string shortName;
        public Path[] paths;

        private PathsPanel _dungeonPathsPanel;

        public Dungeon(int index, string name, int storyLevel, int expLevel, string shortName, Path[] paths)
        {
            this.index = index;
            this.name = name;
            this.storyLevel = storyLevel;
            this.exploreLevel = expLevel;
            this.shortName = shortName;
            this.paths = paths;
        }

        public string GetTooltip()
        {
            if (this.storyLevel != -1)
                return $"{name}\nStory {storyLevel}, Explore {exploreLevel}";
            else
                return $"{name}\nExplore {exploreLevel}";
        
        }

     
        public void SetPanelReference(PathsPanel panel)
        {
            _dungeonPathsPanel= panel;
        }
        public PathsPanel GetPanelReference()
        {
            return _dungeonPathsPanel;
        }


        public static Dungeon[] GetDungeonMetaData()
        {
            return new Dungeon[]
            {
                new Dungeon(
                    1,
                    "Ascalonian Catacombs",
                    30,
                    35,
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
                    2,
                    "Caudecus Manor",
                   40,
                   45,
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
                    3,
                    "Twilight Arbor",
                    50,
                    55,
                    "TA",
                    new Path[]
                    {
                        new Path("ta_story","Story", "S"),
                        new Path("leurent","leurent (Up)", "Up"),
                        new Path("vevina","vevina (down)", "Fwd"),
                        new Path("aetherpath","aetherpath", "Ae"),
                    }
                ),
                new Dungeon(
                    4,
                    "Sorrows Embrace",
                    60,65,
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
                    5,
                    "Citadel of Flame",
                    70,75,
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
                    6,
                    "Honor of the Waves",
                    76,80,
                    "HotW",
                    new Path[]
                    {
                        new Path("hotw_story","Story", "S"),
                        new Path("butcher","butcher", "E1"),
                        new Path("plunderer","plunderer", "E2"),
                        new Path("zealot","zealot", "E3"),
                    }
                ),
                new Dungeon(
                    7,
                    "Crucible of Eternity",
                    78,80,
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
                    8,
                    "Ruined City of Arah",
                    -1,80,
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
            };

        }
    }
}