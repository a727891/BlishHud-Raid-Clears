using Blish_HUD.Controls;
using RaidClears.Settings.Models;

namespace RaidClears.Settings.Views.Tabs;

public class RaidWingSelectionView : MenuedSettingsView
{
    private readonly RaidSettings _settings;
    
    public RaidWingSelectionView(RaidSettings settings)
    {
        _settings = settings;
    }
    
    protected override void Build(Container buildPanel)
    {
        base.Build(buildPanel);

        foreach (var setting in _settings.RaidWings)
        {
            ShowSettingWithViewContainer(setting);
        }
    }
}