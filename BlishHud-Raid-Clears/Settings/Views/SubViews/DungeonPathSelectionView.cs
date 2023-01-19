using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using RaidClears.Settings.Models;
using RaidClears.Utils;

namespace RaidClears.Settings.Views.SubViews;

public class DungeonPathSelectionView : View
{
    private readonly DungeonSettings _settings;

    public DungeonPathSelectionView(DungeonSettings settings)
    {
        _settings = settings;
    }
    
    protected override void Build(Container buildPanel)
    {
        base.Build(buildPanel);

        new FlowPanel()
            .BeginFlow(buildPanel)
            .AddSetting(_settings.DungeonPaths);
    }
}