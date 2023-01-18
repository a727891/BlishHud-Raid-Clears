using Blish_HUD.Controls;
using RaidClears.Localization;
using RaidClears.Settings.Models;

namespace RaidClears.Settings.Views.Tabs;

public class DungeonGeneralView : MenuedSettingsView
{
    private static DungeonSettings Settings => Module.moduleInstance.SettingsService.DungeonSettings;
    
    protected override void Build(Container buildPanel)
    {
        base.Build(buildPanel);
        ShowSettingWithViewContainer(Settings.Generic.Enabled, rootFlowPanel);
        AddVerticalSpacer(rootFlowPanel);

        var dungeonOffPanel = VisibilityInvertedSettingsFlowPanel(rootFlowPanel, settingsService.DungeonSettings.Generic.Enabled);
        ShowText(Strings.Setting_Dun_DisabledWarning, dungeonOffPanel);

        var dungeonSettings = VisibilitySettingsFlowPanel(rootFlowPanel, settingsService.DungeonSettings.Generic.Enabled);
        ShowSettingWithViewContainer(Settings.Generic.PositionLock, dungeonSettings);
        ShowSettingWithViewContainer(Settings.Generic.Visible, dungeonSettings);
        ShowSettingWithViewContainer(Settings.Generic.Tooltips, dungeonSettings);
        ShowSettingWithViewContainer(Settings.Generic.ToolbarIcon, dungeonSettings);
        AddVerticalSpacer(dungeonSettings);
        ShowSettingWithViewContainer(Settings.Generic.ShowHideKeyBind,dungeonSettings);
        ShowText(Strings.SharedKeybind, dungeonSettings);
    }
}