
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Settings;
using Microsoft.Xna.Framework;
using RaidClears.Raids.Controls;
using Settings.Enums;
using Label = Blish_HUD.Controls.Label;

namespace RaidClears.Utils
{
    internal static class GridPanelExtensions
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

        #region Font Size change
        public static void FontSizeChange(this Label label, SettingEntry<ContentService.FontSize> setting)
        {
            setting.SettingChanged += (s, e) =>
            {
                SetFontSize(e.NewValue, label);
            };
            SetFontSize(setting.Value, label);

        }
        public static void SetFontSize(ContentService.FontSize fontSize, Label label)
        {
            var font = GameService
                .Content
                .GetFont(
                    ContentService.FontFace.Menomonia,
                    fontSize,
                    ContentService.FontStyle.Regular
               );
            var width = GetLabelWidthForFontSize(fontSize);

            label.Font = font;
            label.Width = width;
        }
        public static int GetLabelWidthForFontSize(ContentService.FontSize size)
        {
            switch (size)
            {
                case ContentService.FontSize.Size36:
                    return 100;
                case ContentService.FontSize.Size34:
                case ContentService.FontSize.Size32:
                    return 80;
                case ContentService.FontSize.Size24:
                case ContentService.FontSize.Size22:
                case ContentService.FontSize.Size20:
                    return 50;
                case ContentService.FontSize.Size18:
                case ContentService.FontSize.Size16:
                case ContentService.FontSize.Size14:
                    return 40;
                case ContentService.FontSize.Size12:
                case ContentService.FontSize.Size11:
                    return 35;
                case ContentService.FontSize.Size8:
                    return 39;
                default: return 40;
            }
        }
        #endregion

    }

}
