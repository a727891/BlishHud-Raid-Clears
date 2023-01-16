using Blish_HUD.Controls;
using RaidClears.Localization;
using RaidClears.Settings.Views.Tabs;

namespace RaidClears.Settings.Views
{
    public class DungeonGeneralView : MenuedSettingsView
    {
        public DungeonGeneralView()
        { 
        }

        protected override void Build(Container buildPanel)
        {
            base.Build(buildPanel);
            ShowSettingWithViewContainer(_settingsService.DungeonPanelDragWithMouseIsEnabled);
            AddVerticalSpacer();
            ShowSettingWithViewContainer(_settingsService.DungeonPanelIsVisible);
            ShowSettingWithViewContainer(_settingsService.DungeonPanelAllowTooltips);
            AddVerticalSpacer();
            ShowSettingWithViewContainer(_settingsService.DungeonCornerIconEnabled);
            AddVerticalSpacer();
            ShowSettingWithViewContainer(_settingsService.DungeonPanelIsVisibleKeyBind);
            ShowText(Strings.SharedKeybind);

        }


    }
}