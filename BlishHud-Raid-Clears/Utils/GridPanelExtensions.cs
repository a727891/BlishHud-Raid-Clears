
using Blish_HUD.Settings;
using Microsoft.Xna.Framework;
using RaidClears.Features.Shared.Controls;

namespace RaidClears.Utils;

internal static class GridPanelExtensions
{


    public static void BackgroundColorChange(this GridPanel panel, SettingEntry<float> opacity, SettingEntry<string> bgColor)
    {
        opacity.SettingChanged += (s, e) => panel.BackgroundColor = AddAlphaToColor(bgColor.Value.HexToXnaColor(), e.NewValue);
        bgColor.SettingChanged += (s, e) => panel.BackgroundColor = AddAlphaToColor(e.NewValue.HexToXnaColor(), opacity.Value);
        panel.BackgroundColor = AddAlphaToColor(bgColor.Value.HexToXnaColor(), opacity.Value);            
    }
    public static Color AddAlphaToColor(Color color, float opacity)
    {
        var a = (int)(byte.MaxValue * opacity);
        return new Color(color, a);
        
    }

}
