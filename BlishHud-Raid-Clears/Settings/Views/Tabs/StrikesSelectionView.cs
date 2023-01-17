using Blish_HUD.Controls;
using RaidClears.Localization;

namespace RaidClears.Settings.Views.Tabs;

public class StrikesSelectionView : MenuedSettingsView
{
    public StrikesSelectionView()
    {
    }

    protected override void Build(Container buildPanel)
    {
        base.Build(buildPanel);

        ShowSettingWithViewContainer(_settingsService.StrikeVisible_Priority);
        AddVerticalSpacer();
        ShowSettingWithViewContainer(_settingsService.StrikeVisible_IBS);
        ShowText(Strings.Settings_Strike_IBS_Heading);
        ShowSettingWithViewContainer(_settingsService.StrikeVisible_CW);
        ShowSettingWithViewContainer(_settingsService.StrikeVisible_FoJ);
        ShowSettingWithViewContainer(_settingsService.StrikeVisible_SP);
        ShowSettingWithViewContainer(_settingsService.StrikeVisible_VandC);
        ShowSettingWithViewContainer(_settingsService.StrikeVisible_WoJ);
        ShowSettingWithViewContainer(_settingsService.StrikeVisible_BS);
        AddVerticalSpacer();
        ShowSettingWithViewContainer(_settingsService.StrikeVisible_EOD);
        ShowText(Strings.Settings_Strike_EOD_Heading);
        ShowSettingWithViewContainer(_settingsService.StrikeVisible_AH);
        ShowSettingWithViewContainer(_settingsService.StrikeVisible_XJJ);
        ShowSettingWithViewContainer(_settingsService.StrikeVisible_KO);
        ShowSettingWithViewContainer(_settingsService.StrikeVisible_HT);
        ShowSettingWithViewContainer(_settingsService.StrikeVisible_OLC);


    }


}