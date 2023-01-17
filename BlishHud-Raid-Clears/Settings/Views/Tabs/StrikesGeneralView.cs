using Blish_HUD.Controls;
using RaidClears.Localization;

namespace RaidClears.Settings.Views.Tabs
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
            AddVerticalSpacer();
            AddVerticalSpacer();

            StandardButton alignButton = new StandardButton()
            {
                Parent = _rootFlowPanel,
                Text = Strings.Setting_Strike_AlignWithRaids,
                Width = 200

            };
            alignButton.Click += (s, e) =>
            {
                _settingsService.AlignStrikesWithRaidPanel();
            };
            StandardButton copyButton = new StandardButton()
            {
                Parent = _rootFlowPanel,
                Text = Strings.Setting_Strike_CopyRaids,
                Width = 200

            };
            copyButton.Click += (s, e) =>
            {
                copyButton.Enabled = false;
                _settingsService.CopyRaidVisualsToStrikes();
            };

        }


    }
}