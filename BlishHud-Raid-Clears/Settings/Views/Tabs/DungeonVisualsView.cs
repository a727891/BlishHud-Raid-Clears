using Blish_HUD;
using Blish_HUD.Controls;
using RaidClears.Localization;
using RaidClears.Settings.Views.Tabs;

namespace RaidClears.Settings.Views
{
    public class DungeonVisualsView : MenuedSettingsView
    {
        public DungeonVisualsView()
        { 
        }

        protected override void Build(Container buildPanel)
        {
            base.Build(buildPanel);
            ShowEnumSettingWithViewContainer(_settingsService.DungeonPanelLayout);
            ShowEnumSettingWithViewContainer(_settingsService.DungeonPanelFontSize);
            ShowEnumSettingWithViewContainer(_settingsService.DungeonPanelLabelDisplay);
            ShowSettingWithViewContainer(_settingsService.DungeonPanelLabelOpacity);
            ShowSettingWithViewContainer(_settingsService.DungeonPanelGridOpacity);
            ShowSettingWithViewContainer(_settingsService.DungeonPanelBgOpacity);

            AddVerticalSpacer();

            ShowSettingWithViewContainer(_settingsService.dungeonHighlightFrequenter);

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