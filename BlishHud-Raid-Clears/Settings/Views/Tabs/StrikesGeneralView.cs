using Blish_HUD.Controls;
using RaidClears.Localization;
using RaidClears.Settings.Models;

namespace RaidClears.Settings.Views.Tabs;

public class StrikesGeneralView : MenuedSettingsView
{
    private static StrikeSettings Settings => Module.ModuleInstance.SettingsService.StrikeSettings;
    
    protected override void Build(Container buildPanel)
    {
        base.Build(buildPanel);
        ShowSettingWithViewContainer(Settings.Generic.PositionLock);
        AddVerticalSpacer();
        ShowSettingWithViewContainer(Settings.Generic.Visible);
        ShowSettingWithViewContainer(Settings.Generic.Tooltips);
        ShowSettingWithViewContainer(Settings.Generic.ToolbarIcon);
        AddVerticalSpacer();
        ShowSettingWithViewContainer(Settings.Generic.ShowHideKeyBind);
        ShowText(Strings.SharedKeybind);
        AddVerticalSpacer();
        AddVerticalSpacer();

        var alignButton = new StandardButton
        {
            Parent = rootFlowPanel,
            Text = Strings.Setting_Strike_AlignWithRaids,
            Width = 200
        };
        
        alignButton.Click += (_, _) =>
        {
            settingsService.AlignStrikesWithRaidPanel();
        };
        
        var copyButton = new StandardButton
        {
            Parent = rootFlowPanel,
            Text = Strings.Setting_Strike_CopyRaids,
            Width = 200

        };
        
        copyButton.Click += (_, _) =>
        {
            copyButton.Enabled = false;
            settingsService.CopyRaidVisualsToStrikes();
        };
    }
}