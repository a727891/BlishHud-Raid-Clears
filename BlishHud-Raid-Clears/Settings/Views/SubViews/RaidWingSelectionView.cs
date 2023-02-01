using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Microsoft.Xna.Framework;
using RaidClears.Localization;
using RaidClears.Settings.Models;
using RaidClears.Utils;
using System.Linq;

namespace RaidClears.Settings.Views.SubViews;

public class RaidWingSelectionView : View
{
    private readonly RaidSettings _settings;
    
    public RaidWingSelectionView(RaidSettings settings)
    {
        _settings = settings;
    }
    
    protected override void Build(Container buildPanel)
    {
        base.Build(buildPanel);

        var panel = new FlowPanel()
            .BeginFlow(buildPanel);

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
               BackgroundTexture = Service.TexturesService!.HoTLogo

           }
           .AddSpace()
            .AddString(Strings.Settings_Raid_Hot_Heading)
            .AddSetting(_settings.RaidWings.Take(4))
            .AddSpace()
            .AddSpace()
        );
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
               BackgroundTexture = Service.TexturesService!.PoFLogo

           }
           .AddSpace()
            .AddString(Strings.Settings_Raid_PoF_Heading)
            .AddSetting(_settings.RaidWings.Skip(4).Take(3))
            .AddSpace()
            .AddSpace()
            .AddSpace()
        );
          //  .AddSetting(_settings.RaidWings);
    }
}