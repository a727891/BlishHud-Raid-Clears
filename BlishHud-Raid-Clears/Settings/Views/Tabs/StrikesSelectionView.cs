using Blish_HUD.Controls;
using RaidClears.Localization;
using RaidClears.Settings.Models;

namespace RaidClears.Settings.Views.Tabs;

public class StrikesSelectionView : MenuedSettingsView
{
    private static StrikeSettings Settings => Service.Settings.StrikeSettings;
    
    protected override void Build(Container buildPanel)
    {
        base.Build(buildPanel);

        ShowSettingWithViewContainer(Settings.StrikeVisiblePriority);
        
        AddVerticalSpacer();
        ShowSettingWithViewContainer(Settings.StrikeVisibleIbs);
        ShowText(Strings.Settings_Strike_IBS_Heading);

        foreach (var ibsStrike in Settings.IbsMissions)
        {
            ShowSettingWithViewContainer(ibsStrike);
        }
        
        AddVerticalSpacer();
        ShowSettingWithViewContainer(Settings.StrikeVisibleEod);
        ShowText(Strings.Settings_Strike_EOD_Heading);
        foreach (var eodStrike in Settings.EodMissions)
        {
            ShowSettingWithViewContainer(eodStrike);
        }
    }
}