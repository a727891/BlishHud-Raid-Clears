using Blish_HUD.Controls;
using RaidClears.Localization;
using RaidClears.Settings.Views.Tabs;

namespace RaidClears.Settings.Views
{
    public class RaidGeneralView : MenuedSettingsView
    {
        public RaidGeneralView()
        { 
        }

        protected override void Build(Container buildPanel)
        {
            base.Build(buildPanel);
            ShowSettingWithViewContainer(_settingsService.RaidPanelDragWithMouseIsEnabled);
            AddVerticalSpacer();
            ShowSettingWithViewContainer(_settingsService.RaidPanelIsVisible);
            ShowSettingWithViewContainer(_settingsService.RaidPanelAllowTooltips);
            AddVerticalSpacer();
            ShowSettingWithViewContainer(_settingsService.RaidCornerIconEnabled);
            AddVerticalSpacer();
            ShowSettingWithViewContainer(_settingsService.RaidPanelIsVisibleKeyBind);
            ShowText(Strings.SharedKeybind);

        }


    }
}