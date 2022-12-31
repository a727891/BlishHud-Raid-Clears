using Blish_HUD.Controls;
using Microsoft.Xna.Framework;
using System.Text.RegularExpressions;

namespace Blish_HUD.Settings.UI.Views
{
    public class ColorSettingView : SettingView<string>
    {
        private const int CONTROL_PADDING = 5;

        private const int TEXTBOX_WIDTH = 250;

        private const int TEXTBOX_HEIGHT = 27;

        private SettingEntry<string> _setting;

        private Label _displayNameLabel;

        private TextBox _stringTextbox;

        private ColorBox _colorBox;
        private RaidClears.Raids.Model.ColorHelper _colorHelper;

        public ColorSettingView(SettingEntry<string> setting, int definedWidth = -1) : base(setting, definedWidth)
        {
            _setting = setting;
        }
        public override bool HandleComplianceRequisite(IComplianceRequisite complianceRequisite)
        {
            if (complianceRequisite is SettingDisabledComplianceRequisite)
            {
                SettingDisabledComplianceRequisite settingDisabledComplianceRequisite = (SettingDisabledComplianceRequisite)(object)complianceRequisite;
                _displayNameLabel.Enabled = !settingDisabledComplianceRequisite.Disabled;
                _stringTextbox.Enabled = !settingDisabledComplianceRequisite.Disabled;
                return true;
            }

            return false;
        }

        protected override void BuildSetting(Container buildPanel)
        {

            _stringTextbox = new TextBox
            {
                Size = new Point(80, 27),
                Location = new Point(5, 3),
                Parent = buildPanel,
                MaxLength = 7
            };

            _colorHelper = new RaidClears.Raids.Model.ColorHelper();
            _colorHelper.setRGB(_setting.Value);
            _colorBox = new ColorBox
            {
                Location = new Point(90, 0),
                Color = _colorHelper,
                Parent = buildPanel,
                Enabled = false
            };

            _displayNameLabel = new Label
            {
                AutoSizeWidth = true,
                Location = new Point(130, 0),
                Parent = buildPanel
            };
           
            _stringTextbox.InputFocusChanged += StringTextboxOnInputFocusChanged;
            _stringTextbox.TextChanged += TextChangedEventHandler;
        }

        private void StringTextboxOnInputFocusChanged(object sender, ValueEventArgs<bool> e)
        {
            if (!e.Value)
            {
                OnValueChanged(new ValueEventArgs<string>(_stringTextbox.Text));
                updateColorBox(_stringTextbox.Text);
            }
        }
        private void TextChangedEventHandler(object sender, System.EventArgs e)
        {

            updateColorBox(_stringTextbox.Text);
        }

        private void updateColorBox(string text)
        {
            if (Regex.Match(text, "([a-fA-F0-9]{6})").Success)
            {
                _colorHelper.setRGB(text);
                _stringTextbox.BackgroundColor = new Color(0, 0, 0);

            }
            else
            {
                _colorHelper.setRGB(255, 255, 255);
                _colorHelper.setName("Invalid Color");
                _stringTextbox.BackgroundColor = new Color(128, 0, 0);
            }
        }
        private void UpdateSizeAndLayout()
        {
            base.ViewTarget.Height = _stringTextbox.Bottom;
            _displayNameLabel.Height = _stringTextbox.Bottom;
            /*if (base.DefinedWidth > 0)
            {
                _stringTextbox.Left = _displayNameLabel.Right + 5;
                base.ViewTarget.Width = _stringTextbox.Right + 5;
            }
            else
            {
                _stringTextbox.Location = new Point(base.ViewTarget.Width - 5 - 250, 0);
            }*/
        }

        protected override void RefreshDisplayName(string displayName)
        {
            _displayNameLabel.Text = displayName;
            UpdateSizeAndLayout();
        }

        protected override void RefreshDescription(string description)
        {
            _stringTextbox.BasicTooltipText = description;
            _displayNameLabel.BasicTooltipText = description;
        }

        protected override void RefreshValue(string value)
        {
            _stringTextbox.Text = value;
            updateColorBox(value);
        }

        protected override void Unload()
        {
            if (_stringTextbox != null)
            {
                _stringTextbox.InputFocusChanged -= StringTextboxOnInputFocusChanged;
                _stringTextbox.TextChanged -= TextChangedEventHandler;
            }
        }
    }
}