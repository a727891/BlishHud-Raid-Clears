using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Settings;
using Blish_HUD.Settings.UI.Views;
using Microsoft.Xna.Framework;
using RaidClears.Settings.Models;
using System.Text.RegularExpressions;

namespace RaidClears.Settings.Views;

public class ColorSettingView : SettingView<string>
{
    private readonly SettingEntry<string> _setting;

    private Label _displayNameLabel;
    private TextBox _stringTextBox;
    private ColorHelper _colorHelper;

    public ColorSettingView(SettingEntry<string> setting, int definedWidth = -1) : base(setting, definedWidth)
    {
        _setting = setting;
    }
    
    protected override void BuildSetting(Container buildPanel)
    {
        _stringTextBox = new TextBox
        {
            Size = new Point(80, 27),
            Location = new Point(5, 3),
            Parent = buildPanel,
            MaxLength = 7
        };

        _colorHelper = new ColorHelper();
        _colorHelper.SetRGB(_setting.Value);
        
        buildPanel.AddChild(new ColorBox
        {
            Location = new Point(90, 0),
            Color = _colorHelper,
            Enabled = false
        });
        
        _displayNameLabel = new Label
        {
            AutoSizeWidth = true,
            Location = new Point(130, 0),
            Parent = buildPanel
        };

        _stringTextBox.InputFocusChanged += StringTextBoxOnInputFocusChanged;
        _stringTextBox.TextChanged += TextChangedEventHandler;
    }

    private void StringTextBoxOnInputFocusChanged(object sender, ValueEventArgs<bool> e)
    {
        if (!e.Value)
        {
            OnValueChanged(new ValueEventArgs<string>(_stringTextBox.Text));
            UpdateColorBox(_stringTextBox.Text);
        }
    }
    
    private void TextChangedEventHandler(object sender, System.EventArgs e)
    {
        UpdateColorBox(_stringTextBox.Text);
    }

    private void UpdateColorBox(string text)
    {
        if (Regex.Match(text, "([a-fA-F0-9]{6})").Success)
        {
            _colorHelper.SetRGB(text);
            _stringTextBox.BackgroundColor = new Color(0, 0, 0);

        }
        else
        {
            _colorHelper.SetRGB(255, 255, 255);
            _colorHelper.SetName("Invalid Color");
            _stringTextBox.BackgroundColor = new Color(128, 0, 0);
        }
    }
    
    private void UpdateSizeAndLayout()
    {
        ViewTarget.Height = _stringTextBox.Bottom;
        _displayNameLabel.Height = _stringTextBox.Bottom;
    }

    protected override void RefreshDisplayName(string displayName)
    {
        _displayNameLabel.Text = displayName;
        UpdateSizeAndLayout();
    }

    protected override void RefreshDescription(string description)
    {
        _stringTextBox.BasicTooltipText = description;
        _displayNameLabel.BasicTooltipText = description;
    }

    protected override void RefreshValue(string value)
    {
        _stringTextBox.Text = value;
        UpdateColorBox(value);
    }

    protected override void Unload()
    {
        _stringTextBox.InputFocusChanged -= StringTextBoxOnInputFocusChanged;
        _stringTextBox.TextChanged -= TextChangedEventHandler;
    }
}