﻿using System.Collections.Generic;
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
    private bool _showCopyRaids;

    public GenericStyleView(DisplayStyle settings, IEnumerable<SettingEntry<string>>? extraSettings = null, bool showCopyRaids=false)
    {
        _settings = settings;
        _extraSettings = extraSettings;
        _showCopyRaids = showCopyRaids;
    }

    protected override void Build(Container buildPanel)
    {
        base.Build(buildPanel);
        _settings.BgOpacity.SetRange(0.0f, 1.0f);
        _settings.LabelOpacity.SetRange(0.1f, 1.0f);
        _settings.GridOpacity.SetRange(0.1f, 1.0f);
        var panel = new FlowPanel()
            .BeginFlow(buildPanel)
            .AddSettingEnum(_settings.Layout)
            .AddSettingEnum(_settings.FontSize)
            .AddSettingEnum(_settings.LabelDisplay)
            .AddSpace()
            .AddSetting(_settings.LabelOpacity)
            .AddSetting(_settings.GridOpacity)
            .AddSetting(_settings.BgOpacity)
            .AddSpace()
            .AddString(Strings.SettingsPanel_Raid_Visual_Colors)
            .AddString(Strings.SettingsPanel_Raid_Visual_ColorsTip)
            .AddSettingColor(_settings.Color.NotCleared)
            .AddSettingColor(_settings.Color.Cleared)
            .AddSettingColor(_settings.Color.Text)
            .AddSettingColor(_extraSettings);

        if (_showCopyRaids)
        {
            panel
                .AddSpace()
                .AddFlowControl(new StandardButton
                {
                    Text = Strings.Setting_CopyRaids,
                    BasicTooltipText = Strings.Settngs_CopyRaidTooltip
                }, out var CopySettingsButton);

            CopySettingsButton.Click += (_, _) =>
            {
                Service.Settings.CopyRaidSettings(_settings);
                CopySettingsButton.Enabled = false;
            };
        }
    }
}