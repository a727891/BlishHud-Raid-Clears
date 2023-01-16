
using Blish_HUD;
using Blish_HUD.Settings;
using Label = Blish_HUD.Controls.Label;

namespace RaidClears.Utils
{
    internal static class LabelExtensions
    {
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
