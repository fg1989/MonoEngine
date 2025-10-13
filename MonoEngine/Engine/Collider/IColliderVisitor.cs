
using MonoEngine.Engine.Collider.Figure;

namespace MonoEngine.Engine.Collider
{
    public interface IColliderVisitor
    {
        public bool Intersects(ICollider collider);
        public bool Intersects(Circle collider);
        public bool Intersects(Rectangle collider);
        public bool Intersects(Polygone collider);
    }
}
