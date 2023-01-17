using Blish_HUD.Controls;
using RaidClears.Localization;

namespace RaidClears.Settings.Views.Tabs;

public class RaidVisualsView : MenuedSettingsView
{
    public RaidVisualsView()
    {
    }

    protected override void Build(Container buildPanel)
    {
        base.Build(buildPanel);
        ShowEnumSettingWithViewContainer(_settingsService.RaidPanelLayout);
        ShowEnumSettingWithViewContainer(_settingsService.RaidPanelFontSize);
        ShowEnumSettingWithViewContainer(_settingsService.RaidPanelLabelDisplay);
        ShowSettingWithViewContainer(_settingsService.RaidPanelLabelOpacity);
        ShowSettingWithViewContainer(_settingsService.RaidPanelGridOpacity);
        ShowSettingWithViewContainer(_settingsService.RaidPanelBgOpacity);

        AddVerticalSpacer();
        ShowSettingWithViewContainer(_settingsService.RaidPanelHighlightEmbolden);
        ShowSettingWithViewContainer(_settingsService.RaidPanelHighlightCotM);

        AddVerticalSpacer();

        ShowText(Strings.SettingsPanel_Raid_Visual_Colors);


        ShowText(Strings.SettingsPanel_Raid_Visual_ColorsTip);

        ShowColorSettingWithViewContainer(_settingsService.RaidPanelColorNotCleared);
        ShowColorSettingWithViewContainer(_settingsService.RaidPanelColorCleared);
        ShowColorSettingWithViewContainer(_settingsService.RaidPanelColorCotm);
        ShowColorSettingWithViewContainer(_settingsService.RaidPanelColorEmbolden);
        ShowColorSettingWithViewContainer(_settingsService.RaidPanelColorText);

    }


}