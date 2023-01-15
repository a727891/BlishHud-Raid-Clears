using System.ComponentModel;

namespace Settings.Enums
{
    public enum Layout
    {
        [Description("Vertical")]
        Vertical,
        [Description("Horizontal")]
        Horizontal,
        [Description("A single row")]
        SingleRow,
        [Description("A single column")]
        SingleColumn

    }
}