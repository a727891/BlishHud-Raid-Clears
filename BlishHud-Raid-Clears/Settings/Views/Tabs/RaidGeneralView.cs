using Blish_HUD.Controls;
using RaidClears.Localization;

namespace RaidClears.Settings.Views.Tabs;

public class RaidGeneralView : MenuedSettingsView
{
    protected override void Build(Container buildPanel)
    {
        base.Build(buildPanel);
        ShowSettingWithViewContainer(settingsService.RaidPanelDragWithMouseIsEnabled);
        AddVerticalSpacer();
        ShowSettingWithViewContainer(settingsService.RaidPanelIsVisible);
        ShowSettingWithViewContainer(settingsService.RaidPanelAllowTooltips);
        ShowSettingWithViewContainer(settingsService.RaidCornerIconEnabled);
        AddVerticalSpacer();
        ShowSettingWithViewContainer(settingsService.RaidPanelIsVisibleKeyBind);
        ShowText(Strings.SharedKeybind);
    }
}