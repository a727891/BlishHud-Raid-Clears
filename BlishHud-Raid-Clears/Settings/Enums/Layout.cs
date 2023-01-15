using System.ComponentModel;

namespace Settings.Enums
{
    public enum Layout
    {
        [Description("Stacked in a vertical column")]
        Vertical,
        [Description("Listed in a horizontal row")]
        Horizontal,
        [Description("A single row")]
        SingleRow,
        [Description("A single column")]
        SingleColumn

    }
}