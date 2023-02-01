using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Microsoft.Xna.Framework;
using RaidClears.Localization;
using RaidClears.Settings.Models;
using RaidClears.Utils;

namespace RaidClears.Settings.Views.SubViews;

public class StrikeSelectionView : View
{
    private readonly StrikeSettings _settings;

    public StrikeSelectionView(StrikeSettings settings)
    {
        _settings = settings;
    }

    protected override void Build(Container buildPanel)
    {
        base.Build(buildPanel);

        var panel = new FlowPanel()
            .BeginFlow(buildPanel)
            .AddSetting(_settings.StrikeVisiblePriority)
            .AddSpace();


        panel
            .AddSetting(_settings.StrikeVisibleIbs)
            .AddChildPanel(
                new FlowPanel()
                {
                    FlowDirection = ControlFlowDirection.SingleTopToBottom,
                    OuterControlPadding = new Vector2(20, 5),
                    Parent = panel,
                    ShowTint = false,
                    ShowBorder = false,
                    HeightSizingMode = SizingMode.AutoSize,
                    Width = panel.Width - 40,
                    BackgroundTexture = Service.TexturesService!.IBSLogo

                }
                    .AddSpace()
                    .AddString(Strings.Settings_Strike_IBS_Heading)
                    .AddSetting(_settings.IbsMissions)
                    .AddSpace()
            )
            .AddSpace()
            .AddSetting(_settings.StrikeVisibleEod)
            .AddChildPanel(
                new FlowPanel()
                {
                    FlowDirection = ControlFlowDirection.SingleTopToBottom,
                    OuterControlPadding = new Vector2(20, 5),
                    Parent = panel,
                    ShowTint = false,
                    ShowBorder = false,
                    HeightSizingMode = SizingMode.AutoSize,
                    Width = panel.Width - 40,
                    BackgroundTexture = Service.TexturesService!.EoDLogo

                }
                .AddSpace()
                .AddString(Strings.Settings_Strike_EOD_Heading)
                .AddSetting(_settings.EodMissions)
                .AddSpace()
                .AddSpace()
            );

      /*  var ibsLogo = new Image(Service.TexturesService!.IBSLogo)
        {
            Size = new Point(175, 175),
            Location = new Point(250, 50),
            Parent = buildPanel,

        };*/
       /* var eodLogo = new Image(Service.TexturesService!.EoDLogo)
        {
            Size = new Point(175, 175),
            Location = new Point(250,250),
            Parent = buildPanel,
           
        };*/
    }
}