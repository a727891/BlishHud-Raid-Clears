using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using RaidClears.Localization;
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

        var panel = new FlowPanel()
            .BeginFlow(buildPanel)
            .AddString(Strings.Settings_Dungeon_Heading)
            .AddSetting(_settings.DungeonPaths)
            .AddSpace()
            .AddSetting(_settings.DungeonFrequenterVisible);

        new Image()
        {
            Texture = Service.TexturesService!.BaseLogo,
            Parent = buildPanel,
            Location = new(300, 30),
            Size = new(200,200)


        };
    }
}