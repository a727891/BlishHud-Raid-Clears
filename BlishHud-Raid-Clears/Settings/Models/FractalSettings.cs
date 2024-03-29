﻿using System;
using System.Collections.Generic;
using System.Linq;
using Blish_HUD.Settings;
using RaidClears.Features.Shared.Enums;
using RaidClears.Settings.Enums;

namespace RaidClears.Settings.Models;

public class FractalSettings
{

    public SettingEntry<bool> ChallengeMotes { get; set; }
    public SettingEntry<bool> DailyTierN { get; set; }
    public SettingEntry<bool> DailyRecs { get; set; }
    public SettingEntry<bool> TomorrowTierN { get; set; }

    public SettingEntry<StrikeComplete> CompletionMethod { get; set; }

    public DisplayStyle Style { get; set; }
    public GenericSettings Generic { get; set; }

    public FractalSettings(SettingCollection settings)
    {
        Style = new DisplayStyle
        {
            Color = new DisplayColor
            {
                Background = settings.DefineSetting(Settings.Fractal.Style.Color.background),
                NotCleared = settings.DefineSetting(Settings.Fractal.Style.Color.uncleared),
                Cleared = settings.DefineSetting(Settings.Fractal.Style.Color.cleared),
                Text = settings.DefineSetting(Settings.Fractal.Style.Color.text),
            },
            
            FontSize = settings.DefineSetting(Settings.Fractal.Style.fontSize),
            BgOpacity = settings.DefineSetting(Settings.Fractal.Style.backgroundOpacity),
            GridOpacity = settings.DefineSetting(Settings.Fractal.Style.gridOpacity),
            LabelDisplay = settings.DefineSetting(Settings.Fractal.Style.labelDisplay),
            LabelOpacity = settings.DefineSetting(Settings.Fractal.Style.labelOpacity),
            Layout = settings.DefineSetting(Settings.Fractal.Style.layout),
        };
        Style.GridOpacity.SetRange(0.1f, 1.0f);
        Style.LabelOpacity.SetRange(0.1f, 1.0f);
        Style.BgOpacity.SetRange(0.0f, 1.0f);
        Style.LabelDisplay.SetExcluded(Enums.LabelDisplay.WingNumber);

        Generic = new GenericSettings
        {
            Location = settings.DefineSetting(Settings.Fractal.General.location),
            Enabled = settings.DefineSetting(Settings.Fractal.General.enabled),
            PositionLock = settings.DefineSetting(Settings.Fractal.General.positionLock),
            ShowHideKeyBind = settings.DefineSetting(Settings.Fractal.General.keyBind),
            ToolbarIcon = settings.DefineSetting(Settings.Fractal.General.toolbarIcon),
            Visible = settings.DefineSetting(Settings.Fractal.General.visible),
            Tooltips = settings.DefineSetting(Settings.Fractal.General.tooltips),
        };

        ChallengeMotes = settings.DefineSetting(Settings.Fractal.Module.showCMs);
        DailyTierN = settings.DefineSetting(Settings.Fractal.Module.showTierN);
        DailyRecs = settings.DefineSetting(Settings.Fractal.Module.showRecs);
        TomorrowTierN = settings.DefineSetting(Settings.Fractal.Module.tomorrow);
        

        CompletionMethod = settings.DefineSetting(Settings.Fractal.Module.completionMethod);
    }
}