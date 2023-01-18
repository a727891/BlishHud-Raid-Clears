using Blish_HUD.Controls;
using RaidClears.Localization;

namespace RaidClears.Settings.Views.Tabs;

public class ModuleGeneralSettingView : MenuedSettingsView
{
    protected override void Build(Container buildPanel)
    {
        base.Build(buildPanel);
        
        AddVerticalSpacer();
        AddVerticalSpacer();
        ShowSettingWithViewContainer(settingsService.SettingsPanelKeyBind);
        AddVerticalSpacer();
        ShowEnumSettingWithViewContainer(settingsService.ApiPollingPeriod);
        
        var refreshButton = new StandardButton
        {
            Parent = rootFlowPanel,
            Text = Strings.Settings_RefreshNow

        };
        
        refreshButton.Click += (_, _) =>
        {
            Module.moduleInstance.ApiPollingService.Invoke();
            refreshButton.Enabled = false;
        };
    }
}