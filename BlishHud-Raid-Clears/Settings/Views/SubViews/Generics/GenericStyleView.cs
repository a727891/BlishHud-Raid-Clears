using System.Collections.Generic;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Settings;
using Microsoft.Xna.Framework;
using RaidClears.Localization;
using RaidClears.Settings.Models;
using RaidClears.Utils;

namespace RaidClears.Settings.Views.SubViews.Generics;

public class GenericStyleView : View
{
    private readonly DisplayStyle _settings;
    private readonly IEnumerable<SettingEntry<string>>? _extraSettings;

    public GenericStyleView(DisplayStyle settings, IEnumerable<SettingEntry<string>>? extraSettings = null)
    {
        _settings = settings;
        _extraSettings = extraSettings;
    }

    protected override void Build(Container buildPanel)
    {
        base.Build(buildPanel);

        new FlowPanel()
            .BeginFlow(buildPanel)
            .AddSettingEnum(_settings.Layout)
            .AddSettingEnum(_settings.FontSize)
            .AddSettingEnum(_settings.LabelDisplay)
            .AddSpace()
            .AddSetting(_settings.LabelOpacity)
            .AddSetting(_settings.GridOpacity)
            .AddSetting(_settings.BgOpacity)
            .AddSpace()
            .AddString(Strings.SettingsPanel_Raid_Visual_Colors + " " + Strings.SettingsPanel_Raid_Visual_ColorsTip)
            .AddSettingColor(_settings.Color.NotCleared)
            .AddSettingColor(_settings.Color.Cleared)
            .AddSettingColor(_settings.Color.Text)
            .AddSettingColor(_extraSettings);
    }
}