using Microsoft.Xna.Framework;
using RaidClears.Settings.Models;

namespace RaidClears.Utils;

public static class StringExtensions
{
    public static Color HexToXnaColor(this string s) => new ColorHelper(s).XnaColor;
}
