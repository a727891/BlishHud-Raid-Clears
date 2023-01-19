using Blish_HUD.Controls;
using RaidClears.Localization;
using RaidClears.Settings.Models;

namespace RaidClears.Settings.Views.Tabs;

public class DungeonPathSelectionView : MenuedSettingsView
{
    private readonly DungeonSettings _settings;

    public DungeonPathSelectionView(DungeonSettings settings)
    {
        _settings = settings;
    }
    
    protected override void Build(Container buildPanel)
    {
        base.Build(buildPanel);

        var dungeonOffPanel = VisibilityInvertedSettingsFlowPanel(rootFlowPanel, _settings.Generic.Enabled);
        ShowText(Strings.Setting_Dun_DisabledWarning, dungeonOffPanel);

        var dungeonSettings = VisibilitySettingsFlowPanel(rootFlowPanel, _settings.Generic.Enabled);
        foreach (var dungeon in _settings.DungeonPaths)
        {
            ShowSettingWithViewContainer(dungeon, dungeonSettings);
        }
    }
}