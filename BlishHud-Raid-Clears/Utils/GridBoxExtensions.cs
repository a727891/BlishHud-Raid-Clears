using System;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Settings;
using RaidClears.Features.Shared.Controls;
using RaidClears.Settings.Enums;

namespace RaidClears.Utils;

public static class GridBoxExtensions
{
    public static void LabelDisplayChange(
        this GridBox box, 
        SettingEntry<LabelDisplay> labelDisplay, 
        string shortText, 
        string longText)
    {
        var settingChangedDelegate = new EventHandler<ValueChangedEventArgs<LabelDisplay>>((_, args) =>
        {
            if(args.NewValue is LabelDisplay.NoLabel)
            {
                box.Hide();
            }
            else
            {
                box.Show();
                box.Text = GetLabelText(args.NewValue, shortText, longText);
            }

            box.Parent.Invalidate();
        });

        labelDisplay.SettingChanged += settingChangedDelegate;
        settingChangedDelegate.Invoke(null, new ValueChangedEventArgs<LabelDisplay>(labelDisplay.Value, labelDisplay.Value));
    }
    
    public static void TextColorSetting(this GridBox box, SettingEntry<string> textColor)
    {
        textColor.SettingChanged += (_, e) => box.TextColor = e.NewValue.HexToXnaColor();
        box.TextColor = textColor.Value.HexToXnaColor();
    }
    
    public static void ConditionalTextColorSetting(
        this GridBox box,
        SettingEntry<bool> condition,
        SettingEntry<string> trueColor, 
        SettingEntry<string> falseColor)
    {
        CalculateConditionalTextColor(box, condition.Value, trueColor.Value, falseColor.Value);
        condition.SettingChanged += (_,e) => CalculateConditionalTextColor(box, e.NewValue, trueColor.Value, falseColor.Value);
        trueColor.SettingChanged += (_, e) => CalculateConditionalTextColor(box, condition.Value, e.NewValue, falseColor.Value);
        falseColor.SettingChanged += (_, e) => CalculateConditionalTextColor(box, condition.Value, trueColor.Value, e.NewValue);
    }
    
    public static void VisiblityChanged(this GridBox panel, SettingEntry<bool> setting)
    {
        setting.SettingChanged += (_, e) =>
        {
            panel.Visible = e.NewValue;
            panel.Parent?.Invalidate();
        };
        
        panel.Visible = setting.Value;
        panel.Parent?.Invalidate();
    }
    
    //
    // Helper Methods
    //

    private static string GetLabelText(LabelDisplay labelDisplay, string shortText, string longText)
    {
        return labelDisplay switch
        {
            LabelDisplay.NoLabel => "",
            LabelDisplay.WingNumber => shortText,
            LabelDisplay.Abbreviation => longText,
            _ => "-"
        };
    }
    
    private static void CalculateConditionalTextColor(Label boxLabel, bool condition, string trueColor, string falseColor) => boxLabel.TextColor = condition ? trueColor.HexToXnaColor() : falseColor.HexToXnaColor();
}
