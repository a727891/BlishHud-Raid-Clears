using Blish_HUD;
using Blish_HUD.Controls;
using RaidClears.Localization;
using RaidClears.Settings.Views.Tabs;

namespace RaidClears.Settings.Views
{
    public class StrikesVisualsView : MenuedSettingsView
    {
        public StrikesVisualsView()
        { 
        }

        protected override void Build(Container buildPanel)
        {
            base.Build(buildPanel);
            StandardButton copyButton = new StandardButton()
            {
                Parent = _rootFlowPanel,
                Text = Strings.Setting_Strike_CopyRaids,
                Width = 180

            };
            copyButton.Click += (s, e) =>
            {
                copyButton.Enabled = false;
                _settingsService.CopyRaidVisualsToStrikes();
            };
            ShowEnumSettingWithViewContainer(_settingsService.StrikePanelLayout);
            ShowEnumSettingWithViewContainer(_settingsService.StrikePanelFontSize);
            ShowEnumSettingWithViewContainer(_settingsService.StrikePanelLabelDisplay);
            ShowSettingWithViewContainer(_settingsService.StrikePanelLabelOpacity);
            ShowSettingWithViewContainer(_settingsService.StrikePanelGridOpacity);
            ShowSettingWithViewContainer(_settingsService.StrikePanelBgOpacity);

            AddVerticalSpacer();

            ShowText(Strings.SettingsPanel_Raid_Visual_Colors);
            ShowText(Strings.SettingsPanel_Raid_Visual_ColorsTip);

            ShowColorSettingWithViewContainer(_settingsService.StrikePanelColorNotCleared);
            ShowColorSettingWithViewContainer(_settingsService.StrikePanelColorCleared);
            ShowColorSettingWithViewContainer(_settingsService.StrikePanelColorText);

        }


    }
}