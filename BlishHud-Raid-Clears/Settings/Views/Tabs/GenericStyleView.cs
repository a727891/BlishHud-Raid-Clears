using Blish_HUD.Controls;
using Microsoft.Xna.Framework;
using RaidClears.Localization;
using RaidClears.Settings.Models;
using RaidClears.Utils;

namespace RaidClears.Settings.Views.Tabs;

public class GenericStyleView : MenuedSettingsView
{
    private readonly DisplayStyle _settings;

    public GenericStyleView(DisplayStyle settings)
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
            .AddSettingColor(_settings.Color.Text);
    }
}