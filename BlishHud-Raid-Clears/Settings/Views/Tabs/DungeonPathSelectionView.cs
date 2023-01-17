using Blish_HUD.Controls;
using RaidClears.Localization;

namespace RaidClears.Settings.Views.Tabs;

public class DungeonPathSelectionView : MenuedSettingsView
{
    protected override void Build(Container buildPanel)
    {
        base.Build(buildPanel);

        var dungeonOffPanel = VisibilityInvertedSettingsFlowPanel(rootFlowPanel, settingsService.DungeonEnable);
        ShowText(Strings.Setting_Dun_DisabledWarning, dungeonOffPanel);

        var dungeonSettings = VisibilitySettingsFlowPanel(rootFlowPanel, settingsService.DungeonEnable);
        ShowSettingWithViewContainer(settingsService.D1IsVisible, dungeonSettings);
        ShowSettingWithViewContainer(settingsService.D2IsVisible, dungeonSettings);
        ShowSettingWithViewContainer(settingsService.D3IsVisible, dungeonSettings);
        ShowSettingWithViewContainer(settingsService.D4IsVisible, dungeonSettings);
        ShowSettingWithViewContainer(settingsService.D5IsVisible, dungeonSettings);
        ShowSettingWithViewContainer(settingsService.D6IsVisible, dungeonSettings);
        ShowSettingWithViewContainer(settingsService.D7IsVisible, dungeonSettings);
        ShowSettingWithViewContainer(settingsService.D8IsVisible, dungeonSettings);
        ShowSettingWithViewContainer(settingsService.DFIsVisible, dungeonSettings);
    }
}