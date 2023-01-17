using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using RaidClears.Localization;

namespace RaidClears.Settings.Views;

public class ModuleSettingsView: View
{
    protected override void Build(Container buildPanel)
    {
        var button = new StandardButton
        {
            Parent = buildPanel,
            Text = Strings.ModuleSettings_OpenSettings,

        };

        button.Left = (buildPanel.Width /2) - (button.Width/ 2);
        button.Top = (buildPanel.Height / 2) - (button.Height / 2);

        button.Click += (_, _) => Module.ModuleInstance.SettingsWindow.Show();
    }
}
