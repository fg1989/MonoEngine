
using MonoEngine.Engine.Collider.Figure;
using MonoEngine.Engine.MathStuff;

namespace MonoEngine.Engine.Collider
{
    public interface IColliderVisitor
    {
        public bool Intersects(Segment segment);
        public bool Intersects(Circle circle);
        public bool Intersects(Rectangle rectangle);
        public bool Intersects(Polygone polygone);
    }
}
