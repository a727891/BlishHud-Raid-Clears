
using Microsoft.Xna.Framework;

namespace RaidClears.Utils
{
    internal static class Vector2Extensions
    {
        public static Point ToPoint(this Vector2 v)
        {
            return new Point((int)v.X, (int)v.Y);
        }
    }
}
