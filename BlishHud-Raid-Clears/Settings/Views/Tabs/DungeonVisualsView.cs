using Blish_HUD.Controls;
using RaidClears.Localization;
using RaidClears.Settings.Models;

namespace RaidClears.Settings.Views.Tabs;

public class DungeonVisualsView : MenuedSettingsView
{
    private static DungeonSettings Settings => Module.moduleInstance.SettingsService.DungeonSettings;
    
    protected override void Build(Container buildPanel)
    {
        base.Build(buildPanel);
        var dungeonOffPanel = VisibilityInvertedSettingsFlowPanel(rootFlowPanel, Settings.Generic.Enabled);
        ShowText(Strings.Setting_Dun_DisabledWarning, dungeonOffPanel);
        var dungeonSettings = VisibilitySettingsFlowPanel(rootFlowPanel, Settings.Generic.Enabled);
        
        var copyButton = new StandardButton
        {
            Parent = dungeonSettings,
            Text = Strings.Setting_Strike_CopyRaids,
            Width = 200

        };
        
        copyButton.Click += (_, _) =>
        {
            copyButton.Enabled = false;
            settingsService.CopyRaidVisualsToDungeons();
        };
        
        ShowEnumSettingWithViewContainer(Settings.Style.Layout, dungeonSettings);
        ShowEnumSettingWithViewContainer(Settings.Style.FontSize, dungeonSettings);
        ShowEnumSettingWithViewContainer(Settings.Style.LabelDisplay, dungeonSettings);
        ShowSettingWithViewContainer(Settings.Style.LabelOpacity, dungeonSettings);
        ShowSettingWithViewContainer(Settings.Style.GridOpacity, dungeonSettings);
        ShowSettingWithViewContainer(Settings.Style.BgOpacity, dungeonSettings);

        AddVerticalSpacer(dungeonSettings);

        ShowSettingWithViewContainer(Settings.DungeonHighlightFrequenter, dungeonSettings);

        AddVerticalSpacer(dungeonSettings);

        ShowText(Strings.SettingsPanel_Raid_Visual_Colors, dungeonSettings);
        ShowText(Strings.SettingsPanel_Raid_Visual_ColorsTip, dungeonSettings);

        ShowColorSettingWithViewContainer(Settings.Style.Color.NotCleared, dungeonSettings);
        ShowColorSettingWithViewContainer(Settings.Style.Color.Cleared, dungeonSettings);
        ShowColorSettingWithViewContainer(Settings.Style.Color.Text, dungeonSettings);
        ShowColorSettingWithViewContainer(Settings.DungeonPanelColorFreq, dungeonSettings);
    }
}