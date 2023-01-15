using Blish_HUD;
using Blish_HUD.Controls;
using RaidClears.Localization;
using RaidClears.Settings.Views.Tabs;

namespace RaidClears.Settings.Views
{
    public class RaidVisualsView : MenuedSettingsView
    {
        public RaidVisualsView()
        { 
        }

        protected override void Build(Container buildPanel)
        {
            base.Build(buildPanel);
            ShowEnumSettingWithViewContainer(_settingsService.RaidPanelLayout);
            ShowEnumSettingWithViewContainer(_settingsService.RaidPanelFontSize);
            ShowEnumSettingWithViewContainer(_settingsService.RaidPanelLabelDisplay);
            ShowSettingWithViewContainer(_settingsService.RaidPanelLabelOpacity);
            ShowSettingWithViewContainer(_settingsService.RaidPanelGridOpacity);
            AddVerticalSpacer();
            ShowSettingWithViewContainer(_settingsService.RaidPanelHighlightEmbolden);
            ShowSettingWithViewContainer(_settingsService.RaidPanelHighlightCotM);

            AddVerticalSpacer();

            Label helpText = ShowText(Strings.SettingsPanel_Raid_Visual_Colors);
            helpText.AutoSizeHeight = false;
            helpText.WrapText= false;

            ShowText(Strings.SettingsPanel_Raid_Visual_ColorsTip);

            ShowColorSettingWithViewContainer(_settingsService.RaidPanelColorNotCleared);
            ShowColorSettingWithViewContainer(_settingsService.RaidPanelColorCleared);
            ShowColorSettingWithViewContainer(_settingsService.RaidPanelColorCotm);
            ShowColorSettingWithViewContainer(_settingsService.RaidPanelColorEmbolden);
            ShowColorSettingWithViewContainer(_settingsService.RaidPanelColorText);


        }


    }
}