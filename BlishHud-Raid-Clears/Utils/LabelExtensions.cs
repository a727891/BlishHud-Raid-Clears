using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Settings;

namespace RaidClears.Utils;

public static class LabelExtensions
{
    public static void FontSizeChange(this Label label, SettingEntry<ContentService.FontSize> setting)
    {
        setting.SettingChanged += (_, e) =>
        {
            SetFontSize(e.NewValue, label);
        };
        
        SetFontSize(setting.Value, label);
    }
    
    private static void SetFontSize(ContentService.FontSize fontSize, Label label)
    {
        label.Font = GameService
            .Content
            .GetFont(
                ContentService.FontFace.Menomonia,
                fontSize,
                ContentService.FontStyle.Regular
            );
        
        label.Width = GetLabelWidthForFontSize(fontSize);
    }
    
    private static int GetLabelWidthForFontSize(ContentService.FontSize size)
    {
        // ContentService.FontSize.Size36 => 100,
        // ContentService.FontSize.Size34 or ContentService.FontSize.Size32 => 80,
        // ContentService.FontSize.Size24 or ContentService.FontSize.Size22 or ContentService.FontSize.Size20 => 50,
        // ContentService.FontSize.Size18 or ContentService.FontSize.Size16 or ContentService.FontSize.Size14 => 40,
        // ContentService.FontSize.Size12 or ContentService.FontSize.Size11 => 35,
        // ContentService.FontSize.Size8 => 39,
        
        return (int)size switch
        {
            >= 36 => 100,
            >= 32 => 80,
            >= 20 => 50,
            _ => 40,
        };
    }
}
