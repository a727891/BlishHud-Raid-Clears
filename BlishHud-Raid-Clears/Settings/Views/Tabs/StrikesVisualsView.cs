using Blish_HUD.Controls;
using RaidClears.Localization;

namespace RaidClears.Settings.Views.Tabs;

public class StrikesVisualsView : MenuedSettingsView
{
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

        ShowEnumSettingWithViewContainer(settingsService.StrikePanelLayout);
        ShowEnumSettingWithViewContainer(settingsService.StrikePanelFontSize);
        ShowEnumSettingWithViewContainer(settingsService.StrikePanelLabelDisplay);
        ShowSettingWithViewContainer(settingsService.StrikePanelLabelOpacity);
        ShowSettingWithViewContainer(settingsService.StrikePanelGridOpacity);
        ShowSettingWithViewContainer(settingsService.StrikePanelBgOpacity);

        AddVerticalSpacer();

        ShowText(Strings.SettingsPanel_Raid_Visual_Colors);
        ShowText(Strings.SettingsPanel_Raid_Visual_ColorsTip);

        ShowColorSettingWithViewContainer(settingsService.StrikePanelColorNotCleared);
        ShowColorSettingWithViewContainer(settingsService.StrikePanelColorCleared);
        ShowColorSettingWithViewContainer(settingsService.StrikePanelColorText);
    }
}