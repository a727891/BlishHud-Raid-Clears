
using Microsoft.Xna.Framework;

namespace RaidClears.Utils;
public static class Vector2Extensions
{
    public static Point ToPoint(this Vector2 v) => new((int)v.X, (int)v.Y);
}
