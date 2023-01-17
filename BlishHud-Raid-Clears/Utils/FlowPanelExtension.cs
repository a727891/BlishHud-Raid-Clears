using Blish_HUD.Controls;
using Blish_HUD.Settings;
using RaidClears.Settings.Enums;

namespace RaidClears.Utils;

public static class FlowPanelExtensions
{
    public static void VisiblityChanged(this FlowPanel panel, SettingEntry<bool> setting)
    {
        setting.SettingChanged += (_, e) =>
        {
            panel.Visible = e.NewValue;
            panel.Parent?.Invalidate();
        };
        
        panel.Visible = setting.Value;
        panel.Parent?.Invalidate();
    }
    
    public static void InvertedVisiblityChanged(this FlowPanel panel, SettingEntry<bool> setting)
    {
        setting.SettingChanged += (_, e) =>
        {
            panel.Visible = !e.NewValue;
            panel.Parent?.Invalidate();
        };
        
        panel.Visible = !setting.Value;
        panel.Parent?.Invalidate();
    }

    public static void LayoutChange(this FlowPanel panel, SettingEntry<Layout> setting, int nestingLevel = 0)
    {
        setting.SettingChanged += (_, e) =>
        {
            panel.FlowDirection = GetFlowDirection(e.NewValue, nestingLevel);
        };
        
        panel.FlowDirection = GetFlowDirection(setting.Value, nestingLevel);
    }

    private static ControlFlowDirection GetFlowDirection(Layout orientation, int nestingLevel = 0)
    {
        // FlowDirection based on Orientation 
        // V             H 1  2  3 L>R
        // T  1 B1 B2 B3   B1 B1 B1
        // v  2 B1 B2 B3   B2 B2 B2
        // B  3 B1 B2 B3   B3 B3 B3

        var isChild = nestingLevel % 2 is not 0;

        return orientation switch
        {
            Layout.Horizontal when isChild => ControlFlowDirection.SingleTopToBottom,
            Layout.Vertical when isChild => ControlFlowDirection.SingleLeftToRight,
            Layout.SingleRow when isChild => ControlFlowDirection.SingleLeftToRight,
            Layout.SingleColumn when isChild => ControlFlowDirection.SingleTopToBottom,
            _ when isChild => ControlFlowDirection.SingleLeftToRight,
                
            Layout.Horizontal => ControlFlowDirection.SingleLeftToRight,
            Layout.Vertical => ControlFlowDirection.SingleTopToBottom,
            Layout.SingleRow => ControlFlowDirection.SingleLeftToRight,
            Layout.SingleColumn => ControlFlowDirection.SingleTopToBottom,
            _ => ControlFlowDirection.SingleTopToBottom,
        };
    }
}
