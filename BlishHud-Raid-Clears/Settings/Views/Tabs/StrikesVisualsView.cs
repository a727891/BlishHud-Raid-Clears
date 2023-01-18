using Blish_HUD.Controls;
using RaidClears.Localization;
using RaidClears.Settings.Models;

namespace RaidClears.Settings.Views.Tabs;

public class StrikesVisualsView : MenuedSettingsView
{
    private static StrikeSettings StrikeSettings => Module.ModuleInstance.SettingsService.StrikeSettings;
    
    protected override void Build(Container buildPanel)
    {
        base.Build(buildPanel);
        var copyButton = new StandardButton
        {
            Parent = rootFlowPanel,
            Text = Strings.Setting_Strike_CopyRaids,
            Width = 180
        };
        
        copyButton.Click += (_, _) =>
        {
            copyButton.Enabled = false;
            settingsService.CopyRaidVisualsToStrikes();
        };
        
        var style = StrikeSettings.Style;
        
        ShowEnumSettingWithViewContainer(style.Layout);
        ShowEnumSettingWithViewContainer(style.FontSize);
        ShowEnumSettingWithViewContainer(style.LabelDisplay);
        ShowSettingWithViewContainer(style.LabelOpacity);
        ShowSettingWithViewContainer(style.GridOpacity);
        ShowSettingWithViewContainer(style.BgOpacity);

        AddVerticalSpacer();

        ShowText(Strings.SettingsPanel_Raid_Visual_Colors);
        ShowText(Strings.SettingsPanel_Raid_Visual_ColorsTip);

        var colors = StrikeSettings.Style.Color;
        
        ShowColorSettingWithViewContainer(colors.NotCleared);
        ShowColorSettingWithViewContainer(colors.Cleared);
        ShowColorSettingWithViewContainer(colors.Text);
    }
}