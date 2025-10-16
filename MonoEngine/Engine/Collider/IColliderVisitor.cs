
using MonoEngine.Engine.Collider.Figure;
using MonoEngine.Engine.MathStuff;

namespace MonoEngine.Engine.Collider
{
    public interface IColliderVisitor
    {
        public Vector2 Intersects(Circle circle);
        public Vector2 Intersects(Rectangle rectangle);
        public Vector2 Intersects(Polygone polygone);
    }
}
