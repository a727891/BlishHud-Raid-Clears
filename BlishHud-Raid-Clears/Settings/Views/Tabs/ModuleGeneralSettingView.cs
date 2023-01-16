using Blish_HUD;
using Blish_HUD.Controls;
using RaidClears.Localization;
using RaidClears.Settings.Views.Tabs;

namespace RaidClears.Settings.Views
{
    public class ModuleGeneralSettingView : MenuedSettingsView
    {
        public ModuleGeneralSettingView()
        { 
        }

        protected override void Build(Container buildPanel)
        {
            base.Build(buildPanel);


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