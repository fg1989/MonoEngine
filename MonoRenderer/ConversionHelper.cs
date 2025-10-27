using Common;
using Common.Figure;
using MonoRectangle = Microsoft.Xna.Framework.Rectangle;
using MonoVector2 = Microsoft.Xna.Framework.Vector2;

namespace MonoRenderer;

internal static class ConversionHelper
{
    extension(Rectangle r)
    {
        public MonoRectangle ToMono() => new((int)r.X, (int)r.Y, (int)r.Width, (int)r.Height);
    }

    extension(Vector2 v)
    {
        public MonoVector2 ToMono() => new(v.X, v.Y);
    }
}