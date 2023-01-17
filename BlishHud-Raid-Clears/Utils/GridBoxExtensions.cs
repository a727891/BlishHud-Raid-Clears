using Blish_HUD.Settings;
using RaidClears.Features.Shared.Controls;
using RaidClears.Settings.Enums;

namespace RaidClears.Utils;

internal static class GridBoxExtensions
{
    #region GroupLabel changes
    public static void LabelDisplayChange(
        this GridBox box,
        SettingEntry<LabelDisplay> labelDisplay,
        string shortText, string longText
    )
    {
        labelDisplay.SettingChanged += (s, e) =>
        {
            if(e.NewValue == LabelDisplay.NoLabel)
            {
                box.Hide();
            }
            else
            {
                box.Show();
                box.Text = GetLabelText(e.NewValue, shortText, longText);
            }

            box.Parent.Invalidate();
        };

        if (labelDisplay.Value == LabelDisplay.NoLabel)
        {
            box.Hide();
        }
        else
        {
            box.Show();
            box.Text = GetLabelText(labelDisplay.Value, shortText, longText);
        }
        box.Parent.Invalidate();

    }
    public static string GetLabelText(LabelDisplay labelDisplay, string shortText, string longText)
    {
        switch (labelDisplay)
        {
            case LabelDisplay.NoLabel: return "";
            case LabelDisplay.WingNumber: return shortText;
            case LabelDisplay.Abbreviation: return longText;
            default: return "-";
        }

    }


    #endregion

    #region TextColor changes
    public static void TextColorSetting(this GridBox box, SettingEntry<string> textColor)
    {
        textColor.SettingChanged += (s, e) => box.TextColor = e.NewValue.HexToXNAColor();
        box.TextColor = textColor.Value.HexToXNAColor();
    }

    public static void ConditionalTextColorSetting(
        this GridBox box,
        SettingEntry<bool> condition,
        SettingEntry<string> trueColor, 
        SettingEntry<string> falseColor)
    {
        CalculateConditionalTextcolor(box, condition.Value, trueColor.Value, falseColor.Value);
        condition.SettingChanged += (s,e) => CalculateConditionalTextcolor(box, e.NewValue, trueColor.Value, falseColor.Value);
        trueColor.SettingChanged += (s, e) => CalculateConditionalTextcolor(box, condition.Value, e.NewValue, falseColor.Value);
        falseColor.SettingChanged += (s, e) => CalculateConditionalTextcolor(box, condition.Value, trueColor.Value, e.NewValue);
    }
    private static void CalculateConditionalTextcolor(GridBox box, bool condition, string trueColor, string falseColor)
    {
        if (condition)
        {
            box.TextColor = trueColor.HexToXNAColor();
        }
        else
        {
            box.TextColor = falseColor.HexToXNAColor();
        }

    }

    #endregion

    public static void VisiblityChanged(this GridBox panel, SettingEntry<bool> setting)
    {
        setting.SettingChanged += (s, e) =>
        {
            panel.Visible = e.NewValue;
            panel.Parent?.Invalidate();
        };
        panel.Visible = setting.Value;
        panel.Parent?.Invalidate();
    }
}
