using Blish_HUD.Controls;
using RaidClears.Localization;
using RaidClears.Settings.Models;

namespace RaidClears.Settings.Views.Tabs;

public class RaidVisualsView : MenuedSettingsView
{
    private static RaidSettings Settings => Module.ModuleInstance.SettingsService.RaidSettings;
    
    protected override void Build(Container buildPanel)
    {
        base.Build(buildPanel);
        ShowEnumSettingWithViewContainer(Settings.Style.Layout);
        ShowEnumSettingWithViewContainer(Settings.Style.FontSize);
        ShowEnumSettingWithViewContainer(Settings.Style.LabelDisplay);
        ShowSettingWithViewContainer(Settings.Style.LabelOpacity);
        ShowSettingWithViewContainer(Settings.Style.GridOpacity);
        ShowSettingWithViewContainer(Settings.Style.BgOpacity);

        AddVerticalSpacer();
        ShowSettingWithViewContainer(Settings.RaidPanelHighlightEmbolden);
        ShowSettingWithViewContainer(Settings.RaidPanelHighlightCotM);

        AddVerticalSpacer();

        ShowText(Strings.SettingsPanel_Raid_Visual_Colors);

        ShowText(Strings.SettingsPanel_Raid_Visual_ColorsTip);

        ShowColorSettingWithViewContainer(Settings.Style.Color.NotCleared);
        ShowColorSettingWithViewContainer(Settings.Style.Color.Cleared);
        ShowColorSettingWithViewContainer(Settings.RaidPanelColorCotm);
        ShowColorSettingWithViewContainer(Settings.RaidPanelColorEmbolden);
        ShowColorSettingWithViewContainer(Settings.Style.Color.Text);
    }
}