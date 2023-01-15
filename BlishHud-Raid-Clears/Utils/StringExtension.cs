
using Microsoft.Xna.Framework;
using RaidClears.Settings.Models;

namespace RaidClears.Utils
{
    internal static class StringExtensions
    {
        public static Color HexToXNAColor(this string s)
        {
            ColorHelper cs = new ColorHelper(s);
            return cs.XnaColor;
        }
    }
}
