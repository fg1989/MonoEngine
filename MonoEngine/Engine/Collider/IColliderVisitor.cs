
using MonoEngine.Engine.Collider.Figure;

namespace MonoEngine.Engine.Collider
{
    public interface IColliderVisitor
    {
        public bool Intersects(ICollider collider);
        public bool Intersects(Circle circle);
        public bool Intersects(Rectangle rectangle);
        public bool Intersects(Polygone polygone);
    }
}
