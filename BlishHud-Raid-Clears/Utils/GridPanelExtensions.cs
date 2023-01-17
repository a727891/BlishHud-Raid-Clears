using Blish_HUD.Settings;
using Microsoft.Xna.Framework;
using RaidClears.Features.Shared.Controls;

namespace RaidClears.Utils;

public static class GridPanelExtensions
{
    public static void BackgroundColorChange(this GridPanel panel, SettingEntry<float> opacity, SettingEntry<string> bgColor)
    {
        opacity.SettingChanged += (_, e) => panel.BackgroundColor = AddAlphaToColor(bgColor.Value.HexToXnaColor(), e.NewValue);
        bgColor.SettingChanged += (_, e) => panel.BackgroundColor = AddAlphaToColor(e.NewValue.HexToXnaColor(), opacity.Value);
        
        panel.BackgroundColor = AddAlphaToColor(bgColor.Value.HexToXnaColor(), opacity.Value);            
    }
    
    private static Color AddAlphaToColor(Color color, float opacity) => new(color, byte.MaxValue * opacity);
}
