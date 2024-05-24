using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Settings;
using Microsoft.Xna.Framework;
using RaidClears.Features.Strikes.Services;
using RaidClears.Localization;
using RaidClears.Settings.Models;
using RaidClears.Utils;
using System.Collections.Generic;

namespace RaidClears.Settings.Views.SubViews;

public class DynamicStrikeSelectionView : View
{
    private readonly StrikeSettingsPersistance _setting = Service.StrikeSettings;
    private readonly StrikeData _data = Service.StrikeData;
    public DynamicStrikeSelectionView()
    {
        
    }

    protected override void Build(Container buildPanel)
    {
        base.Build(buildPanel);

        var panel = new FlowPanel()
            .BeginFlow(buildPanel);

        panel.CanScroll= true;

        BuildPrioriyPanel(panel, _data.Priority);

        foreach(var expac in _data.Expansions)
        {
            BuildExpansionPanel(panel, expac);
        }
    }
    private void BuildPrioriyPanel(FlowPanel panel, ExpansionStrikes expac)
    {
        panel
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
                BackgroundTexture = Service.Textures!.GetDynamicTexture(expac.asset)

            }
            .AddString($"Display {expac.Name} strike missions")
            .AddSetting(_data.GetPriorityVisible())
            .AddSpace()
        );
    }

    private void BuildExpansionPanel(FlowPanel panel, ExpansionStrikes expac)
    {
        List<SettingEntry<bool>> expansionMissions = new();
        foreach (var mission in expac.Missions)
        {
           expansionMissions.Add(_data.GetMissionVisible(mission));
        }
        panel
        .AddSetting(_data.GetExpansionVisible(expac))
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
            .AddString($"Display individual {expac.Name} strike missions")
            .AddSetting(expansionMissions)
            .AddSpace(), out var childPanel
        ) ;
        for(var i=expansionMissions.Count;i<=5;i++)
        {
            ((FlowPanel)childPanel).AddSpace();
        }
    }
}