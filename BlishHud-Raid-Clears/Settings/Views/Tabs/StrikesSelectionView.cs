using Blish_HUD.Controls;
using RaidClears.Localization;

namespace RaidClears.Settings.Views.Tabs;

public class StrikesSelectionView : MenuedSettingsView
{
    protected override void Build(Container buildPanel)
    {
        base.Build(buildPanel);

        ShowSettingWithViewContainer(settingsService.StrikeVisible_Priority);
        AddVerticalSpacer();
        ShowSettingWithViewContainer(settingsService.StrikeVisible_IBS);
        ShowText(Strings.Settings_Strike_IBS_Heading);
        ShowSettingWithViewContainer(settingsService.StrikeVisible_CW);
        ShowSettingWithViewContainer(settingsService.StrikeVisible_FoJ);
        ShowSettingWithViewContainer(settingsService.StrikeVisible_SP);
        ShowSettingWithViewContainer(settingsService.StrikeVisible_VandC);
        ShowSettingWithViewContainer(settingsService.StrikeVisible_WoJ);
        ShowSettingWithViewContainer(settingsService.StrikeVisible_BS);
        AddVerticalSpacer();
        ShowSettingWithViewContainer(settingsService.StrikeVisible_EOD);
        ShowText(Strings.Settings_Strike_EOD_Heading);
        ShowSettingWithViewContainer(settingsService.StrikeVisible_AH);
        ShowSettingWithViewContainer(settingsService.StrikeVisible_XJJ);
        ShowSettingWithViewContainer(settingsService.StrikeVisible_KO);
        ShowSettingWithViewContainer(settingsService.StrikeVisible_HT);
        ShowSettingWithViewContainer(settingsService.StrikeVisible_OLC);
    }
}