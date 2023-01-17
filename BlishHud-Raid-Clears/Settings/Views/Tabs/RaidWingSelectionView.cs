using Blish_HUD.Controls;

namespace RaidClears.Settings.Views.Tabs;

public class RaidWingSelectionView : MenuedSettingsView
{
    protected override void Build(Container buildPanel)
    {
        base.Build(buildPanel);

        ShowSettingWithViewContainer(settingsService.W1IsVisible);
        ShowSettingWithViewContainer(settingsService.W2IsVisible);
        ShowSettingWithViewContainer(settingsService.W3IsVisible);
        ShowSettingWithViewContainer(settingsService.W4IsVisible);
        ShowSettingWithViewContainer(settingsService.W5IsVisible);
        ShowSettingWithViewContainer(settingsService.W6IsVisible);
        ShowSettingWithViewContainer(settingsService.W7IsVisible);
    }
}