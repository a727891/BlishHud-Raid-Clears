using System.ComponentModel;

namespace Settings.Enums
{
    public enum LabelDisplay
    {
        [Description("Show numbers")]
        WingNumber,

        [Description("Abbreviate the names")]
        Abbreviation,

        [Description("Hide the names")]
        NoLabel
    }
}