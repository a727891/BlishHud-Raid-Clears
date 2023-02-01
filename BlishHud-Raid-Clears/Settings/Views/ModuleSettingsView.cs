using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using RaidClears.Localization;
using RaidClears.Utils;

namespace RaidClears.Settings.Views;

public class ModuleMainSettingsView: View
{
    protected override void Build(Container buildPanel)
    {
        buildPanel.AddControl(new StandardButton 
        {
            Parent = buildPanel,
            Text = Strings.ModuleSettings_OpenSettings,
            Size = buildPanel.Size.Scale(0.20f),
            Location = buildPanel.Size.Half() - buildPanel.Size.Scale(0.20f).Half(),
            
        }).Click += (_, _) => Service.SettingsWindow.Show();
    }
}
