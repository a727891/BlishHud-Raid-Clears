using Blish_HUD.Controls;
using RaidClears.Localization;
using RaidClears.Utils;

namespace RaidClears.Settings.Views.Tabs;

public class DungeonGeneralView : MenuedSettingsView
{
    protected override void Build(Container buildPanel)
    {
        base.Build(buildPanel);
        ShowSettingWithViewContainer(_settingsService.DungeonEnable,_rootFlowPanel);
        AddVerticalSpacer(_rootFlowPanel);

        FlowPanel dungeonOffPanel = VisibibilityInvertedSettingsFlowPanel(_rootFlowPanel, _settingsService.DungeonEnable);
        ShowText(Strings.Setting_Dun_DisabledWarning, dungeonOffPanel);

        FlowPanel dungeonSettings = VisibibilitySettingsFlowPanel(_rootFlowPanel, _settingsService.DungeonEnable);
        ShowSettingWithViewContainer(_settingsService.DungeonPanelDragWithMouseIsEnabled, dungeonSettings);
        ShowSettingWithViewContainer(_settingsService.DungeonPanelIsVisible, dungeonSettings);
        ShowSettingWithViewContainer(_settingsService.DungeonPanelAllowTooltips, dungeonSettings);
        ShowSettingWithViewContainer(_settingsService.DungeonCornerIconEnabled, dungeonSettings);
        AddVerticalSpacer(dungeonSettings);
        ShowSettingWithViewContainer(_settingsService.DungeonPanelIsVisibleKeyBind,dungeonSettings);
        ShowText(Strings.SharedKeybind, dungeonSettings);

    }


}