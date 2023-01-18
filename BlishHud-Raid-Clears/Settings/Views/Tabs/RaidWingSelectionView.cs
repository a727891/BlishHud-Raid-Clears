using Blish_HUD.Controls;

namespace RaidClears.Settings.Views.Tabs;

public class RaidWingSelectionView : MenuedSettingsView
{
    protected override void Build(Container buildPanel)
    {
        base.Build(buildPanel);

        foreach (var setting in settingsService.RaidSettings.RaidWings)
        {
            ShowSettingWithViewContainer(setting);
        }
    }
}