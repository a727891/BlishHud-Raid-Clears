using Blish_HUD.Controls;
using RaidClears.Localization;

namespace RaidClears.Settings.Views.Tabs;

public class DungeonVisualsView : MenuedSettingsView
{
    protected override void Build(Container buildPanel)
    {
        base.Build(buildPanel);
        FlowPanel dungeonOffPanel = VisibibilityInvertedSettingsFlowPanel(_rootFlowPanel, _settingsService.DungeonEnable);
        ShowText(Strings.Setting_Dun_DisabledWarning, dungeonOffPanel);
        FlowPanel dungeonSettings = VisibibilitySettingsFlowPanel(_rootFlowPanel, _settingsService.DungeonEnable);
        StandardButton copyButton = new StandardButton()
        {
            Parent = dungeonSettings,
            Text = Strings.Setting_Strike_CopyRaids,
            Width = 200

        };
        copyButton.Click += (s, e) =>
        {
            copyButton.Enabled = false;
            _settingsService.CopyRaidVisualsToDungeons();
        };
        ShowEnumSettingWithViewContainer(_settingsService.DungeonPanelLayout, dungeonSettings);
        ShowEnumSettingWithViewContainer(_settingsService.DungeonPanelFontSize, dungeonSettings);
        ShowEnumSettingWithViewContainer(_settingsService.DungeonPanelLabelDisplay, dungeonSettings);
        ShowSettingWithViewContainer(_settingsService.DungeonPanelLabelOpacity, dungeonSettings);
        ShowSettingWithViewContainer(_settingsService.DungeonPanelGridOpacity, dungeonSettings);
        ShowSettingWithViewContainer(_settingsService.DungeonPanelBgOpacity, dungeonSettings);

        AddVerticalSpacer(dungeonSettings);

        ShowSettingWithViewContainer(_settingsService.DungeonHighlightFrequenter, dungeonSettings);

        AddVerticalSpacer(dungeonSettings);

        ShowText(Strings.SettingsPanel_Raid_Visual_Colors, dungeonSettings);
        ShowText(Strings.SettingsPanel_Raid_Visual_ColorsTip, dungeonSettings);

        ShowColorSettingWithViewContainer(_settingsService.DungeonPanelColorNotCleared, dungeonSettings);
        ShowColorSettingWithViewContainer(_settingsService.DungeonPanelColorCleared, dungeonSettings);
        ShowColorSettingWithViewContainer(_settingsService.DungeonPanelColorText, dungeonSettings);
        ShowColorSettingWithViewContainer(_settingsService.DungeonPanelColorFreq, dungeonSettings);

    }


}