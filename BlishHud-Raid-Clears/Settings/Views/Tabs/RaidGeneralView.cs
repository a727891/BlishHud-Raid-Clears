using Blish_HUD.Controls;
using RaidClears.Localization;

namespace RaidClears.Settings.Views.Tabs;

public class RaidGeneralView : MenuedSettingsView
{
    protected override void Build(Container buildPanel)
    {
        base.Build(buildPanel);
        ShowSettingWithViewContainer(_settingsService.RaidPanelDragWithMouseIsEnabled);
        AddVerticalSpacer();
        ShowSettingWithViewContainer(_settingsService.RaidPanelIsVisible);
        ShowSettingWithViewContainer(_settingsService.RaidPanelAllowTooltips);
        ShowSettingWithViewContainer(_settingsService.RaidCornerIconEnabled);
        AddVerticalSpacer();
        ShowSettingWithViewContainer(_settingsService.RaidPanelIsVisibleKeyBind);
        ShowText(Strings.SharedKeybind);

    }


}