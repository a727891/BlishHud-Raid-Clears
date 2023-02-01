using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Microsoft.Xna.Framework;
using RaidClears.Localization;
using RaidClears.Utils;

namespace RaidClears.Settings.Views.SubViews;

public class MainSettingsView : View
{
    protected override void Build(Container buildPanel)
    {
        base.Build(buildPanel);

        var panel = new FlowPanel()
            .BeginFlow(buildPanel, new Point(-95, 0), new Point(0, 5))
            .AddSetting(Service.Settings.SettingsPanelKeyBind)
            .AddSpace()
            .AddSetting(Service.Settings.ApiPollingPeriod)
            .AddSpace()
            .AddFlowControl(new StandardButton
            {
                Text = Strings.Settings_RefreshNow,
            }, out var refreshButton)
            .AddSpace()
            .AddSpace()
            .AddSetting(Service.Settings.GlobalCornerIconEnabled);

        panel.AddChildPanel(
            new FlowPanel()
            {
                FlowDirection = ControlFlowDirection.SingleTopToBottom,
                OuterControlPadding = new Vector2(20, 5),
                Parent = panel,
                ShowTint = false,
                ShowBorder = false,
                HeightSizingMode = SizingMode.AutoSize,
                Width = panel.Width - 40,
                //BackgroundTexture = Service.TexturesService.IBSLogo

            }
            .AddString(Strings.Setting_cornerIconHelpText)
            .AddSetting(Service.Settings.RaidSettings.Generic.ToolbarIcon)
            .AddSetting(Service.Settings.DungeonSettings.Generic.ToolbarIcon)
            .AddSetting(Service.Settings.StrikeSettings.Generic.ToolbarIcon)
            );
           
        refreshButton.Click += (_, _) =>
        {
            Service.ApiPollingService?.Invoke();
            refreshButton.Enabled = false;
        };
    }
}