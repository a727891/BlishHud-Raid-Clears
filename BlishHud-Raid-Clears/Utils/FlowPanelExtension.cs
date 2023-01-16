
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Settings;
using Settings.Enums;

namespace RaidClears.Utils
{
    internal static class FlowPanelExtensions
    {

        public static void VisiblityChanged(this FlowPanel panel, SettingEntry<bool> setting)
        {
            setting.SettingChanged += (s, e) =>
            {
                panel.Visible = e.NewValue;
                panel.Parent?.Invalidate();
            };
            panel.Visible = setting.Value;
            panel.Parent?.Invalidate();
        }

        #region Layout change
        public static void LayoutChange(this FlowPanel panel, SettingEntry<Layout> setting, int nestingLevel = 0)
        {
            setting.SettingChanged += (s, e) =>
            {
                panel.FlowDirection = GetFlowDirection(e.NewValue, nestingLevel);
            };
            panel.FlowDirection = GetFlowDirection(setting.Value, nestingLevel);

        }
            
        public static ControlFlowDirection GetFlowDirection(Layout orientation, int nestingLevel = 0)
        {
            /**  FlowDirection based on Orientation 
                * V             H 1  2  3 L>R
                * T  1 B1 B2 B3   B1 B1 B1
                * v  2 B1 B2 B3   B2 B2 B2
                * B  3 B1 B2 B3   B3 B3 B3
                */
            if (nestingLevel % 2 == 0)
            {
                switch (orientation)
                {
                    case Layout.Horizontal: return ControlFlowDirection.SingleLeftToRight;
                    case Layout.Vertical: return ControlFlowDirection.SingleTopToBottom;
                    case Layout.SingleRow: return ControlFlowDirection.SingleLeftToRight;
                    case Layout.SingleColumn: return ControlFlowDirection.SingleTopToBottom;

                    default: return ControlFlowDirection.SingleTopToBottom;
                }
            }
            else //Children flow opposite
            {
                switch (orientation)
                {
                    case Layout.Horizontal: return ControlFlowDirection.SingleTopToBottom;
                    case Layout.Vertical: return ControlFlowDirection.SingleLeftToRight;
                    case Layout.SingleRow: return ControlFlowDirection.SingleLeftToRight;
                    case Layout.SingleColumn: return ControlFlowDirection.SingleTopToBottom;
                    default: return ControlFlowDirection.SingleLeftToRight;
                }

            }
        }

        #endregion


    }

}
