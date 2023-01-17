using Blish_HUD.Controls;
using RaidClears.Localization;
using RaidClears.Utils;

namespace RaidClears.Settings.Views.Tabs;

public class DungeonGeneralView : MenuedSettingsView
{
    protected override void Build(Container buildPanel)
    {
        base.Build(buildPanel);
        ShowSettingWithViewContainer(settingsService.DungeonEnable,rootFlowPanel);
        AddVerticalSpacer(rootFlowPanel);

        var dungeonOffPanel = VisibilityInvertedSettingsFlowPanel(rootFlowPanel, settingsService.DungeonEnable);
        ShowText(Strings.Setting_Dun_DisabledWarning, dungeonOffPanel);

        var dungeonSettings = VisibilitySettingsFlowPanel(rootFlowPanel, settingsService.DungeonEnable);
        ShowSettingWithViewContainer(settingsService.DungeonPanelDragWithMouseIsEnabled, dungeonSettings);
        ShowSettingWithViewContainer(settingsService.DungeonPanelIsVisible, dungeonSettings);
        ShowSettingWithViewContainer(settingsService.DungeonPanelAllowTooltips, dungeonSettings);
        ShowSettingWithViewContainer(settingsService.DungeonCornerIconEnabled, dungeonSettings);
        AddVerticalSpacer(dungeonSettings);
        ShowSettingWithViewContainer(settingsService.DungeonPanelIsVisibleKeyBind,dungeonSettings);
        ShowText(Strings.SharedKeybind, dungeonSettings);
    }
}