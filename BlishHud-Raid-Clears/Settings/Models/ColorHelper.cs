using System.Collections.Generic;
using System.Linq;

using System.Text.RegularExpressions;
using XnaColor = Microsoft.Xna.Framework.Color;

namespace RaidClears.Settings.Models;

public class ColorHelper : Gw2Sharp.WebApi.V2.Models.Color
{
    public ColorHelper()
    {
        BaseRgb = new List<int>(3);
   
        SetRGB(255,255,255);
    }
    
    public ColorHelper(string colorCode)
    {
        BaseRgb = new List<int>(3);

        SetRGB(colorCode);
    }

    public XnaColor XnaColor { 
        get 
        {
            var color = Cloth.Rgb.ToList();
            return new XnaColor(color[0], color[1], color[2]);
        } 
    }

    public void SetName(string name) => Name = name;

    public void SetRGB(string colorCode)
    {
        colorCode = Regex.Replace(colorCode, "[^a-fA-F0-9]", string.Empty);

        if (colorCode.Length == 6)
        {
            var r = System.Convert.ToByte(colorCode.Substring(0, 2), 16);
            var g = System.Convert.ToByte(colorCode.Substring(2, 2), 16);
            var b = System.Convert.ToByte(colorCode.Substring(4, 2), 16);
            SetRGB(r, g, b);
        }
        else
        {
            SetRGB(255, 255, 255);
        }
    }

    public void SetRGB(int r = 0, int g = 0, int b = 0)
    {
        Cloth.Rgb = new List<int> { r, g, b };
        Name = $"RGB: {r} {g} {b}";
    }
}