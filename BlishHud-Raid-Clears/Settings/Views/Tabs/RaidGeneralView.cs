using Blish_HUD.Controls;
using RaidClears.Localization;
using RaidClears.Settings.Models;

namespace RaidClears.Settings.Views.Tabs;

public class RaidGeneralView : MenuedSettingsView
{
    private static RaidSettings Settings => Module.moduleInstance.SettingsService.RaidSettings;
    
    protected override void Build(Container buildPanel)
    {
        base.Build(buildPanel);
        ShowSettingWithViewContainer(Settings.Generic.PositionLock);
        AddVerticalSpacer();
        ShowSettingWithViewContainer(Settings.Generic.Visible);
        ShowSettingWithViewContainer(Settings.Generic.Tooltips);
        ShowSettingWithViewContainer(Settings.Generic.ToolbarIcon);
        AddVerticalSpacer();
        ShowSettingWithViewContainer(Settings.Generic.ShowHideKeyBind);
        ShowText(Strings.SharedKeybind);
    }
}