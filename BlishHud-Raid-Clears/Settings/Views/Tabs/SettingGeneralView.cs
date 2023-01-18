using Blish_HUD.Controls;
using RaidClears.Localization;
using RaidClears.Settings.Models;

namespace RaidClears.Settings.Views.Tabs;

public class SettingGeneralView : MenuedSettingsView
{
    private readonly GenericSettings _settings;

    public SettingGeneralView(GenericSettings settings)
    {
        _settings = settings;
    }
    
    protected override void Build(Container buildPanel)
    {
        base.Build(buildPanel);
        
        ShowSettingWithViewContainer(_settings.Enabled, rootFlowPanel);
        AddVerticalSpacer(rootFlowPanel);

        var dungeonOffPanel = VisibilityInvertedSettingsFlowPanel(rootFlowPanel, settingsService.DungeonSettings.Generic.Enabled);
        ShowText(Strings.Setting_Dun_DisabledWarning, dungeonOffPanel);

        var dungeonSettings = VisibilitySettingsFlowPanel(rootFlowPanel, settingsService.DungeonSettings.Generic.Enabled);
        ShowSettingWithViewContainer(_settings.PositionLock, dungeonSettings);
        ShowSettingWithViewContainer(_settings.Visible, dungeonSettings);
        ShowSettingWithViewContainer(_settings.Tooltips, dungeonSettings);
        ShowSettingWithViewContainer(_settings.ToolbarIcon, dungeonSettings);
        AddVerticalSpacer(dungeonSettings);
        ShowSettingWithViewContainer(_settings.ShowHideKeyBind,dungeonSettings);
        ShowText(Strings.SharedKeybind, dungeonSettings);
    }
}