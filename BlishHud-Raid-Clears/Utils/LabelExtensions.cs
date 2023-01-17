
using Blish_HUD;
using Blish_HUD.Settings;
using Label = Blish_HUD.Controls.Label;

namespace RaidClears.Utils;

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
        return (int)size switch
        {
            >=36 => 100,
            >=32 => 80,
            >=20 => 50,
           /* ContentService.FontSize.Size36 => 100,
            ContentService.FontSize.Size34 or ContentService.FontSize.Size32 => 80,
            ContentService.FontSize.Size24 or ContentService.FontSize.Size22 or ContentService.FontSize.Size20 => 50,
            ContentService.FontSize.Size18 or ContentService.FontSize.Size16 or ContentService.FontSize.Size14 => 40,
            ContentService.FontSize.Size12 or ContentService.FontSize.Size11 => 35,
            ContentService.FontSize.Size8 => 39,*/
            _ => 40,
        };
    }
    #endregion

}
