﻿using Blish_HUD.Controls;
using Microsoft.Xna.Framework;
using RaidClears.Localization;
using RaidClears.Settings.Models;
using RaidClears.Utils;

namespace RaidClears.Settings.Views.Tabs;

public class GenericGeneralView : MenuedSettingsView
{
    private readonly GenericSettings _settings;
    
    public GenericGeneralView(GenericSettings settings)
    {
        _settings = settings;
    }
    
    protected override void Build(Container buildPanel)
    {
        buildPanel.Location = new Point(300, 5);
        buildPanel.Size = new Point(760, 680);
        
        base.Build(buildPanel);
        
        new FlowPanel()
            .BeginFlow(buildPanel)
            .AddSetting(_settings.PositionLock)
            .AddSpace()
            .AddSetting(_settings.Visible)
            .AddSetting(_settings.Tooltips)
            .AddSetting(_settings.ToolbarIcon)
            .AddSpace()
            .AddSetting(_settings.ShowHideKeyBind)
            .AddSpace()
            .AddString(Strings.SharedKeybind);
    }
}