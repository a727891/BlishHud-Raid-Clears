using Blish_HUD.Controls;
using RaidClears.Localization;

namespace RaidClears.Settings.Views.Tabs
{
    public class DungeonVisualsView : MenuedSettingsView
    {
        protected override void Build(Container buildPanel)
        {
            base.Build(buildPanel);
            StandardButton copyButton = new StandardButton()
            {
                Parent = _rootFlowPanel,
                Text = Strings.Setting_Strike_CopyRaids,
                Width = 200

            };
            copyButton.Click += (s, e) =>
            {
                copyButton.Enabled = false;
                _settingsService.CopyRaidVisualsToDungeons();
            };
            ShowEnumSettingWithViewContainer(_settingsService.DungeonPanelLayout);
            ShowEnumSettingWithViewContainer(_settingsService.DungeonPanelFontSize);
            ShowEnumSettingWithViewContainer(_settingsService.DungeonPanelLabelDisplay);
            ShowSettingWithViewContainer(_settingsService.DungeonPanelLabelOpacity);
            ShowSettingWithViewContainer(_settingsService.DungeonPanelGridOpacity);
            ShowSettingWithViewContainer(_settingsService.DungeonPanelBgOpacity);

            AddVerticalSpacer();

            ShowSettingWithViewContainer(_settingsService.DungeonHighlightFrequenter);

            AddVerticalSpacer();

            ShowText(Strings.SettingsPanel_Raid_Visual_Colors);
            ShowText(Strings.SettingsPanel_Raid_Visual_ColorsTip);

            ShowColorSettingWithViewContainer(_settingsService.DungeonPanelColorNotCleared);
            ShowColorSettingWithViewContainer(_settingsService.DungeonPanelColorCleared);
            ShowColorSettingWithViewContainer(_settingsService.DungeonPanelColorText);
            ShowColorSettingWithViewContainer(_settingsService.DungeonPanelColorFreq);

        }


    }
}