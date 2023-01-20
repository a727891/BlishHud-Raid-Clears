using System.Collections;
using System.Collections.Generic;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Settings;
using Microsoft.Xna.Framework;
using RaidClears.Localization;
using RaidClears.Settings.Models;
using RaidClears.Utils;

namespace RaidClears.Settings.Views.SubViews.Generics;

public class GenericGeneralView : View
{
    private readonly GenericSettings _settings;
    private readonly IEnumerable<SettingEntry>? _extraSettings;

    public GenericGeneralView(GenericSettings settings, IEnumerable<SettingEntry>? extraSettings = null)
    {
        _settings = settings;
        _extraSettings = extraSettings;
    }
    
    protected override void Build(Container buildPanel)
    {
        // This sets the size and position for the main build panel each time it is built
        // this propagates to all children after this one so this view must be built first
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
            .AddString(Strings.SharedKeybind)
            .AddSpace()
            .AddSetting(_extraSettings);
    }
}