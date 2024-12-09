using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Settings;
using Microsoft.Xna.Framework;
using RaidClears.Features.Raids.Models;
using RaidClears.Features.Raids.Services;
using RaidClears.Utils;
using System.Collections.Generic;

namespace RaidClears.Settings.Views.SubViews;

public class DynamicRaidSelectionView : View
{
    private readonly RaidSettingsPersistance _setting = Service.RaidSettings;
    private readonly RaidData _data = Service.RaidData;
    public DynamicRaidSelectionView()
    {
        
    }

    protected override void Build(Container buildPanel)
    {
        base.Build(buildPanel);

        var panel = new FlowPanel()
            .BeginFlow(buildPanel);

        panel.CanScroll= true;


        foreach(var expac in _data.Expansions)
        {
            BuildExpansionPanel(panel, expac);
        }
    }
    private void BuildExpansionPanel(FlowPanel panel, ExpansionRaid expac)
    {
        List<SettingEntry<bool>> wings = new();
        foreach (var wing in expac.Wings)
        {
           wings.Add(_setting.GetWingVisible(wing));
        }
        panel
        .AddSetting(_setting.GetExpansionVisible(expac))
        .AddFlowControl(
            new FlowPanel()
            {
                FlowDirection = ControlFlowDirection.SingleTopToBottom,
                OuterControlPadding = new Vector2(20, 5),
                Parent = panel,
                ShowTint = false,
                ShowBorder = false,
                HeightSizingMode = SizingMode.AutoSize,
                Width = panel.Width - 40,
                BackgroundTexture = Service.Textures!.GetDynamicTexture(expac.asset)

            }
            //.AddString($"Display {expac.Name} raid wings")
            , out var childPanel
        ) ;
        foreach(var wing in expac.Wings)
        {
            BuildWingPanel((FlowPanel)childPanel!, wing);
        }
        for(var i=wings.Count;i<=4;i++)
        {
            ((FlowPanel)childPanel).AddSpace();
        }
    }

    private void BuildWingPanel(FlowPanel panel, RaidWing raidWing)
    {
        List<SettingEntry<bool>> encounters = new();
        foreach (var enc in raidWing.Encounters)
        {
            encounters.Add(_setting.GetEncounterVisible(enc));
        }
        panel
            .AddSetting(_setting.GetWingVisible(raidWing))
            .AddFlowControl(new FlowPanel()
            {
                FlowDirection = ControlFlowDirection.SingleLeftToRight,
                Width = panel.Width - 40,
                HeightSizingMode = SizingMode.AutoSize,
            }
            .AddHorizontalSpace(20)
            .AddSetting(encounters,panel.Width/8)
            ) ;
    }
}