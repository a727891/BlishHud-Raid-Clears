using System;
using System.Linq;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Settings;
using Blish_HUD.Settings.UI.Views;
using Humanizer;
using Microsoft.Xna.Framework;

namespace RaidClears.Settings.Views;

public static class EnumSettingView
{
    public static IView? FromEnum(SettingEntry setting, int definedWidth = -1)
    {
        return Activator.CreateInstance(typeof(EnumSettingView<>).MakeGenericType(setting.SettingType), setting, definedWidth) as IView;
    }
}

public class EnumSettingView<TEnum> : SettingView<TEnum> where TEnum : struct, Enum
{
    private const int CONTROL_PADDING = 5;

    private const int DROPDOWN_WIDTH = 277;
    private const int DROPDOWN_HEIGHT = 27;

    private Label _displayNameLabel;
    private Dropdown _enumDropdown;

    private TEnum[] _enumValues;

    public EnumSettingView(SettingEntry<TEnum> setting, int definedWidth = -1) : base(setting, definedWidth) { /* NOOP */ }

    protected override Task<bool> Load(IProgress<string> progress)
    {
        progress.Report("Loading setting values...");
        _enumValues = EnumUtil.GetCachedValues<TEnum>();
        progress.Report(string.Empty);

        return base.Load(progress);
    }

    protected override void BuildSetting(Container buildPanel)
    {
        _displayNameLabel = new Label
        {
            AutoSizeWidth = false,
            Width = 175,//Aligns with Numeric Trackbar
            Location = new Point(CONTROL_PADDING, 0),
            Parent = buildPanel
        };

        _enumDropdown = new Dropdown
        {
            Size = new Point(DROPDOWN_WIDTH, DROPDOWN_HEIGHT),
            Parent = buildPanel
        };
        _enumValues.Select(e => e.Humanize(LetterCasing.Title)).ToList().ForEach(e => _enumDropdown.Items.Add(e));
        //_enumDropdown.Items.AddRange();

        _enumDropdown.ValueChanged += EnumDropdownOnValueChanged;
    }

    public override bool HandleComplianceRequisite(IComplianceRequisite complianceRequisite)
    {
        switch (complianceRequisite)
        {
            case EnumInclusionComplianceRequisite<TEnum> enumInclusionRequisite:
                var toRemove = _enumValues.Except(enumInclusionRequisite.IncludedValues);

                foreach (var value in toRemove)
                {
                    _enumDropdown.Items.Remove(value.Humanize(LetterCasing.Title));
                }

                break;
            case SettingDisabledComplianceRequisite disabledRequisite:
                _displayNameLabel.Enabled = !disabledRequisite.Disabled;
                _enumDropdown.Enabled = !disabledRequisite.Disabled;
                break;
            default:
                return false;
        }

        return true;
    }

    private void EnumDropdownOnValueChanged(object sender, ValueChangedEventArgs e) => OnValueChanged(new ValueEventArgs<TEnum>(e.CurrentValue.DehumanizeTo<TEnum>()));

    private void UpdateSizeAndLayout()
    {
        ViewTarget.Height = _enumDropdown.Bottom;
        _displayNameLabel.Height = ViewTarget.Height;

        if (DefinedWidth > 0)
        {
            _enumDropdown.Left = _displayNameLabel.Right + CONTROL_PADDING;
            ViewTarget.Width = _enumDropdown.Right + CONTROL_PADDING;
        }
        else
        {
            _enumDropdown.Location = new Point(ViewTarget.Width - CONTROL_PADDING - DROPDOWN_WIDTH, 0);
        }
    }

    protected override void RefreshDisplayName(string displayName)
    {
        _displayNameLabel.Text = displayName;

        UpdateSizeAndLayout();
    }

    protected override void RefreshDescription(string description)
    {
        _displayNameLabel.BasicTooltipText = description;
        _enumDropdown.BasicTooltipText = description;
    }

    protected override void RefreshValue(TEnum value) => _enumDropdown.SelectedItem = value.Humanize(LetterCasing.Title);
    protected override void Unload() => _enumDropdown.ValueChanged -= EnumDropdownOnValueChanged;
}