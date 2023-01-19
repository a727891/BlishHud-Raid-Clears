using Blish_HUD.Controls;
using Microsoft.Xna.Framework;
using RaidClears.Localization;
using RaidClears.Utils;

namespace RaidClears.Settings.Views.Tabs;

public class ModuleGeneralSettingView : MenuedSettingsView
{
    protected override void Build(Container buildPanel)
    {
        base.Build(buildPanel);

        new FlowPanel()
            .BeginFlow(buildPanel, new Point(-95, 0), new Point(0, 5))
            .AddSetting(Service.Settings.SettingsPanelKeyBind)
            .AddSpace()
            .AddSetting(Service.Settings.ApiPollingPeriod)
            .AddControl( new StandardButton
            {
                Parent = buildPanel,
                Text = Strings.Settings_RefreshNow,
            }, out var refreshButton);
        
        refreshButton.Click += (_, _) =>
        {
            Service.ApiPollingService?.Invoke();
            refreshButton.Enabled = false;
        };
    }
}