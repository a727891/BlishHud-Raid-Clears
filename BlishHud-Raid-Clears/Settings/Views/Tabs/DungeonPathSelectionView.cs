using Blish_HUD.Controls;
using RaidClears.Localization;

namespace RaidClears.Settings.Views.Tabs;

public class DungeonPathSelectionView : MenuedSettingsView
{
    protected override void Build(Container buildPanel)
    {
        base.Build(buildPanel);

        FlowPanel dungeonOffPanel = VisibibilityInvertedSettingsFlowPanel(_rootFlowPanel, _settingsService.DungeonEnable);
        ShowText(Strings.Setting_Dun_DisabledWarning, dungeonOffPanel);

        FlowPanel dungeonSettings = VisibibilitySettingsFlowPanel(_rootFlowPanel, _settingsService.DungeonEnable);
        ShowSettingWithViewContainer(_settingsService.D1IsVisible, dungeonSettings);
        ShowSettingWithViewContainer(_settingsService.D2IsVisible, dungeonSettings);
        ShowSettingWithViewContainer(_settingsService.D3IsVisible, dungeonSettings);
        ShowSettingWithViewContainer(_settingsService.D4IsVisible, dungeonSettings);
        ShowSettingWithViewContainer(_settingsService.D5IsVisible, dungeonSettings);
        ShowSettingWithViewContainer(_settingsService.D6IsVisible, dungeonSettings);
        ShowSettingWithViewContainer(_settingsService.D7IsVisible, dungeonSettings);
        ShowSettingWithViewContainer(_settingsService.D8IsVisible, dungeonSettings);
        ShowSettingWithViewContainer(_settingsService.DFIsVisible, dungeonSettings);


    }


}