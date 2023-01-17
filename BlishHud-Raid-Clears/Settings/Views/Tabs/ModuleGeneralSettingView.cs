using Blish_HUD.Controls;
using RaidClears.Localization;

namespace RaidClears.Settings.Views.Tabs
{
    public class ModuleGeneralSettingView : MenuedSettingsView
    {
        protected override void Build(Container buildPanel)
        {
            base.Build(buildPanel);
            AddVerticalSpacer();
            AddVerticalSpacer();
            ShowSettingWithViewContainer(_settingsService.SettingsPanelKeyBind);
            AddVerticalSpacer();
            ShowEnumSettingWithViewContainer(_settingsService.ApiPollingPeriod);
            StandardButton refreshButton = new StandardButton()
            {
                Parent = _rootFlowPanel,
                Text = Strings.Settings_RefreshNow

            };
            refreshButton.Click += (s, e) =>
            {
                Module.ModuleInstance?.ApiPollingService?.Invoke();
                refreshButton.Enabled = false;
            };

        }


    }
}