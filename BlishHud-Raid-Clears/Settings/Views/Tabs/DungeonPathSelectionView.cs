using Blish_HUD.Controls;
using RaidClears.Localization;
using RaidClears.Settings.Models;

namespace RaidClears.Settings.Views.Tabs;

public class DungeonPathSelectionView : MenuedSettingsView
{
    private static DungeonSettings Settings => Module.ModuleInstance.SettingsService.DungeonSettings;
    
    protected override void Build(Container buildPanel)
    {
        base.Build(buildPanel);

        var dungeonOffPanel = VisibilityInvertedSettingsFlowPanel(rootFlowPanel, Settings.Generic.Enabled);
        ShowText(Strings.Setting_Dun_DisabledWarning, dungeonOffPanel);

        var dungeonSettings = VisibilitySettingsFlowPanel(rootFlowPanel, Settings.Generic.Enabled);
        foreach (var dungeon in Settings.DungeonPaths)
        {
            ShowSettingWithViewContainer(dungeon, dungeonSettings);
        }
    }
}