using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using RaidClears.Localization;
using RaidClears.Utils;

namespace RaidClears.Settings.Views;

public class ModuleMainSettingsView: View
{

    protected override void Build(Container buildPanel)
    {

        StandardButton _openSettingsButton = new StandardButton
        {
            Parent = buildPanel,
            Text = Strings.ModuleSettings_OpenSettings,
            Size = buildPanel.Size.Scale(0.20f),
            Location = buildPanel.Size.Half() - buildPanel.Size.Scale(0.20f).Half(),

        };

        buildPanel.AddControl(_openSettingsButton);


        StandardButton _runSetupWizard = new StandardButton
        {
            Parent = buildPanel,
            Text = "Setup Wizard",
            Size = buildPanel.Size.Scale(0.20f),
            Location = buildPanel.Size.Half() - buildPanel.Size.Scale(0.20f).Half()+new Microsoft.Xna.Framework.Point(0,_openSettingsButton.Height+10),

        };



        _openSettingsButton.Click += (_, _) => Service.SettingsWindow.Show();
        _runSetupWizard.Click += (_, _) => Service.SettingsWindow.Show();
    }
}
