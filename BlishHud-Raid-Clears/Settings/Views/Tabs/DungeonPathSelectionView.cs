using Blish_HUD;
using Blish_HUD.Controls;
using RaidClears.Localization;
using RaidClears.Settings.Views.Tabs;

namespace RaidClears.Settings.Views
{
    public class DungeonPathSelectionView : MenuedSettingsView
    {
        public DungeonPathSelectionView()
        { 
        }

        protected override void Build(Container buildPanel)
        {
            base.Build(buildPanel);

            ShowSettingWithViewContainer(_settingsService.D1IsVisible);
            ShowSettingWithViewContainer(_settingsService.D2IsVisible);
            ShowSettingWithViewContainer(_settingsService.D3IsVisible);
            ShowSettingWithViewContainer(_settingsService.D4IsVisible);
            ShowSettingWithViewContainer(_settingsService.D5IsVisible);
            ShowSettingWithViewContainer(_settingsService.D6IsVisible);
            ShowSettingWithViewContainer(_settingsService.D7IsVisible);
            ShowSettingWithViewContainer(_settingsService.D8IsVisible);
            ShowSettingWithViewContainer(_settingsService.DFIsVisible);


        }


    }
}