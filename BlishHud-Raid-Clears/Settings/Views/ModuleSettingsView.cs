
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;

namespace RaidClears.Settings.Views
{
    public class ModuleSettingsView: View
    {

        public ModuleSettingsView()
        {

        }

        protected override void Build(Container buildPanel)
        {

            StandardButton button = new StandardButton()
            {
                Parent = buildPanel,
                Text = "Open Settings",

            };

            button.Left = (buildPanel.Width /2) - (button.Width/ 2);
            button.Top = (buildPanel.Height / 2) - (button.Height / 2);

            button.Click += (s, e) => Module.ModuleInstance.SettingsWindow.Show();


        }
    }
}
