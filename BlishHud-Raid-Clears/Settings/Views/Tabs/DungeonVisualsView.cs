using Blish_HUD.Controls;
using RaidClears.Localization;

namespace RaidClears.Settings.Views.Tabs;

public class DungeonVisualsView : MenuedSettingsView
{
    protected override void Build(Container buildPanel)
    {
        base.Build(buildPanel);
        var dungeonOffPanel = VisibilityInvertedSettingsFlowPanel(rootFlowPanel, settingsService.DungeonEnable);
        ShowText(Strings.Setting_Dun_DisabledWarning, dungeonOffPanel);
        var dungeonSettings = VisibilitySettingsFlowPanel(rootFlowPanel, settingsService.DungeonEnable);
        
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
        
        ShowEnumSettingWithViewContainer(settingsService.DungeonPanelLayout, dungeonSettings);
        ShowEnumSettingWithViewContainer(settingsService.DungeonPanelFontSize, dungeonSettings);
        ShowEnumSettingWithViewContainer(settingsService.DungeonPanelLabelDisplay, dungeonSettings);
        ShowSettingWithViewContainer(settingsService.DungeonPanelLabelOpacity, dungeonSettings);
        ShowSettingWithViewContainer(settingsService.DungeonPanelGridOpacity, dungeonSettings);
        ShowSettingWithViewContainer(settingsService.DungeonPanelBgOpacity, dungeonSettings);

        AddVerticalSpacer(dungeonSettings);

        ShowSettingWithViewContainer(settingsService.DungeonHighlightFrequenter, dungeonSettings);

        AddVerticalSpacer(dungeonSettings);

        ShowText(Strings.SettingsPanel_Raid_Visual_Colors, dungeonSettings);
        ShowText(Strings.SettingsPanel_Raid_Visual_ColorsTip, dungeonSettings);

        ShowColorSettingWithViewContainer(settingsService.DungeonPanelColorNotCleared, dungeonSettings);
        ShowColorSettingWithViewContainer(settingsService.DungeonPanelColorCleared, dungeonSettings);
        ShowColorSettingWithViewContainer(settingsService.DungeonPanelColorText, dungeonSettings);
        ShowColorSettingWithViewContainer(settingsService.DungeonPanelColorFreq, dungeonSettings);
    }
}