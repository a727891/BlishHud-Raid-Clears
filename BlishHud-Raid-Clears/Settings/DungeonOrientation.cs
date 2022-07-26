using System.ComponentModel;

namespace RaidClears.Settings
{
    public enum DungeonOrientation
    {
        [Description("Dungeons in a vertical column")]
        Vertical,
        [Description("Dungeons in a horizontal row")]
        Horizontal,
        [Description("A single row")]
        SingleRow

    }
}