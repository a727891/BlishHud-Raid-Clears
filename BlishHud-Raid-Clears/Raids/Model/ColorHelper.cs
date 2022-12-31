using System.Collections.Generic;
using Gw2Sharp.WebApi.V2.Models;
using System.Linq;
using System.Drawing.Imaging;
using System.Data.SqlTypes;
using System.Text.RegularExpressions;
using XnaColor = Microsoft.Xna.Framework.Color;

namespace RaidClears.Raids.Model
{
    public class ColorHelper : Gw2Sharp.WebApi.V2.Models.Color
    {
        public ColorHelper()
        {
            this.BaseRgb = new List<int>(3);
       
            this.setRGB(255,255,255);
        }
        public ColorHelper(string colorCode)
        {
            this.BaseRgb = new List<int>(3);

            this.setRGB(colorCode);
        }

        public List<int> getRGB()
        {
            return this.Cloth.Rgb.ToList<int>();
        }
        public XnaColor XnaColor { 
            get 
            {
                var color = this.Cloth.Rgb.ToList();
                return new XnaColor(color[0], color[1], color[2]);
            } 
        }
        public XnaColor getXnaColor()
        {
            var color = this.Cloth.Rgb.ToList();
            return new XnaColor(color[0], color[1], color[2]);
        }

        public void setName(string name)
        {
            this.Name = name;
        }

        public void setRGB(string colorCode)
        {
            colorCode = Regex.Replace(colorCode, "[^a-fA-F0-9]", string.Empty);

            if (colorCode.Length == 6)
            {
                var r = System.Convert.ToByte(colorCode.Substring(0, 2), 16);
                var g = System.Convert.ToByte(colorCode.Substring(2, 2), 16);
                var b = System.Convert.ToByte(colorCode.Substring(4, 2), 16);
                this.setRGB(r, g, b);
            }
            else
            {
                this.setRGB(255, 255, 255);
            }
 
        }

        public void setRGB()
        {
            this.setRGB(0, 0, 0);
        }

        public void setRGB(int r, int g, int b)
        {
            this.Cloth.Rgb = new List<int> { r, g, b };
            this.Name = $"RGB: {r} {g} {b}";

        }
    }
}