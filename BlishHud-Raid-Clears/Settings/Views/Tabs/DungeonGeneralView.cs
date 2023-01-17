using Blish_HUD.Controls;
using RaidClears.Localization;

namespace RaidClears.Settings.Views.Tabs;

public class DungeonGeneralView : MenuedSettingsView
{
    protected override void Build(Container buildPanel)
    {
        base.Build(buildPanel);
        ShowSettingWithViewContainer(_settingsService.DungeonPanelDragWithMouseIsEnabled);
        AddVerticalSpacer();
        ShowSettingWithViewContainer(_settingsService.DungeonPanelIsVisible);
        ShowSettingWithViewContainer(_settingsService.DungeonPanelAllowTooltips);
        ShowSettingWithViewContainer(_settingsService.DungeonCornerIconEnabled);
        AddVerticalSpacer();
        ShowSettingWithViewContainer(_settingsService.DungeonPanelIsVisibleKeyBind);
        ShowText(Strings.SharedKeybind);

    }


}