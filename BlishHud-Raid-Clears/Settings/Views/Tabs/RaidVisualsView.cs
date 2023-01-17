using Blish_HUD.Controls;
using RaidClears.Localization;

namespace RaidClears.Settings.Views.Tabs;

public class RaidVisualsView : MenuedSettingsView
{
    protected override void Build(Container buildPanel)
    {
        base.Build(buildPanel);
        ShowEnumSettingWithViewContainer(settingsService.RaidPanelLayout);
        ShowEnumSettingWithViewContainer(settingsService.RaidPanelFontSize);
        ShowEnumSettingWithViewContainer(settingsService.RaidPanelLabelDisplay);
        ShowSettingWithViewContainer(settingsService.RaidPanelLabelOpacity);
        ShowSettingWithViewContainer(settingsService.RaidPanelGridOpacity);
        ShowSettingWithViewContainer(settingsService.RaidPanelBgOpacity);

        AddVerticalSpacer();
        ShowSettingWithViewContainer(settingsService.RaidPanelHighlightEmbolden);
        ShowSettingWithViewContainer(settingsService.RaidPanelHighlightCotM);

        AddVerticalSpacer();

        ShowText(Strings.SettingsPanel_Raid_Visual_Colors);

        ShowText(Strings.SettingsPanel_Raid_Visual_ColorsTip);

        ShowColorSettingWithViewContainer(settingsService.RaidPanelColorNotCleared);
        ShowColorSettingWithViewContainer(settingsService.RaidPanelColorCleared);
        ShowColorSettingWithViewContainer(settingsService.RaidPanelColorCotm);
        ShowColorSettingWithViewContainer(settingsService.RaidPanelColorEmbolden);
        ShowColorSettingWithViewContainer(settingsService.RaidPanelColorText);
    }
}