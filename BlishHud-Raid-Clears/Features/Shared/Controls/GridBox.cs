using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Settings;
using Microsoft.Xna.Framework;
using RaidClears.Features.Raids.Models;
using RaidClears.Utils;
using Settings.Enums;
using System;


namespace RaidClears.Features.Shared.Controls
{
    public class GridBox : Label
    {
        protected BoxModel Model;
        public GridBox(
            Container parent,
            string title,
            string tooltip,
            SettingEntry<float> opacity,
            SettingEntry<Layout> layout,
            SettingEntry<ContentService.FontSize> fontSize
        )
        {

            Parent = parent;

            Text = title;
            BasicTooltipText = tooltip;

            AutoSizeHeight = true;

            OpacityChange(opacity);
            //LayoutChange(layout);
            FontSizeChange(fontSize);


        }

        public void SetFontColor(Color color)
        {
            TextColor=color;
        }
        public void SetBackgroundColor(Color color) { 
            BackgroundColor=color;
        }

        protected void OpacityChange(SettingEntry<float> opacity )
        {
            opacity.SettingChanged += (s, e) =>
            {
                this.Opacity = e.NewValue;
            };
            this.Opacity = opacity.Value;
        }
        protected void LayoutChange(SettingEntry<Layout> layout)
        {
            layout.SettingChanged += (s, e) =>
            {
                HorizontalAlignment = LabelAlignment(e.NewValue);
            };
            HorizontalAlignment = LabelAlignment(layout.Value);
        }

        protected HorizontalAlignment LabelAlignment(Layout layout)
        {
            switch (layout)
            {
                case Layout.Vertical:
                case Layout.SingleRow:
                    return HorizontalAlignment.Right;
                case Layout.Horizontal:
                case Layout.SingleColumn:
                default:
                    return HorizontalAlignment.Center;
            }
        }


        protected void FontSizeChange(SettingEntry<ContentService.FontSize> fontSize)
        {
            fontSize.SettingChanged += (s, e) =>
            {
                SetFontSize(e.NewValue, this);
            };
            SetFontSize(fontSize.Value, this);

        }
        public void SetFontSize(ContentService.FontSize fontSize, Label label)
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

    }
}
