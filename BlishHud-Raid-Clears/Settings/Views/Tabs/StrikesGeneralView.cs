using Blish_HUD.Controls;
using RaidClears.Localization;
using RaidClears.Settings.Views.Tabs;

namespace RaidClears.Settings.Views
{
    public class StrikesGeneralView : MenuedSettingsView
    {
        public StrikesGeneralView()
        { 
        }

        protected override void Build(Container buildPanel)
        {
            base.Build(buildPanel);
            ShowSettingWithViewContainer(_settingsService.StrikePanelDragWithMouseIsEnabled);
            AddVerticalSpacer();
            ShowSettingWithViewContainer(_settingsService.StrikePanelIsVisible);
            ShowSettingWithViewContainer(_settingsService.StrikePanelAllowTooltips);
            ShowSettingWithViewContainer(_settingsService.StrikeCornerIconEnabled);
            AddVerticalSpacer();
            ShowSettingWithViewContainer(_settingsService.StrikePanelIsVisibleKeyBind);
            ShowText(Strings.SharedKeybind);

        }


    }
}