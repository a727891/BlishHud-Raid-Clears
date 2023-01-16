﻿using Blish_HUD;
using Blish_HUD.Controls;
using RaidClears.Localization;
using RaidClears.Settings.Views.Tabs;

namespace RaidClears.Settings.Views
{
    public class RaidWingSelectionView : MenuedSettingsView
    {
        public RaidWingSelectionView()
        { 
        }

        protected override void Build(Container buildPanel)
        {
            base.Build(buildPanel);

            ShowSettingWithViewContainer(_settingsService.W1IsVisible);
            ShowSettingWithViewContainer(_settingsService.W2IsVisible);
            ShowSettingWithViewContainer(_settingsService.W3IsVisible);
            ShowSettingWithViewContainer(_settingsService.W4IsVisible);
            ShowSettingWithViewContainer(_settingsService.W5IsVisible);
            ShowSettingWithViewContainer(_settingsService.W6IsVisible);
            ShowSettingWithViewContainer(_settingsService.W7IsVisible);


        }


    }
}