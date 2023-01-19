using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Microsoft.Xna.Framework;
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
        buildPanel.Location = new Point(300, 5);
        buildPanel.Size = new Point(760, 680);
        
        base.Build(buildPanel);
        
        
        new FlowPanel()
            .BeginFlow(buildPanel)
            .AddSetting(_settings.IbsMissions)
            .AddSpace()
            .AddSetting(_settings.EodMissions);
    }
}