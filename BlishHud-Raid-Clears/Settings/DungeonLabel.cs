using System.ComponentModel;

namespace RaidClears.Settings
{
    public enum DungeonLabel
    {

        [Description("Abbreviate the dungeon names")]
        Abbreviation,

        [Description("Hide the dungeon names")]
        NoLabel
    }
}