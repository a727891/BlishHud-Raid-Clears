﻿using System.ComponentModel;

namespace GatheringTools.Settings
{
    public enum WingLabel
    {
        [Description("Show wing numbers")]
        WingNumber,

        [Description("Abbreviate the wing names")]
        Abbreviation,

        [Description("Hide the wing names")]
        NoLabel
    }
}