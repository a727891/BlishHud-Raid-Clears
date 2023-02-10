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
                    BackgroundTexture = Service.Textures!.IBSLogo

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
                    BackgroundTexture = Service.Textures!.EoDLogo

                }
                .AddSpace()
                .AddString(Strings.Settings_Strike_EOD_Heading)
                .AddSetting(_settings.EodMissions)
                .AddSpace()
                .AddSpace()
            );

    }
}