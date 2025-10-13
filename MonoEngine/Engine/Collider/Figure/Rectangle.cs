using MonoEngine.Engine.MathStuff;

namespace MonoEngine.Engine.Collider.Figure
{
    public struct Rectangle : ICollider, IColliderVisitor
    {
        private Vector2 position;
        private Vector2 size;
        private float roation;

        public bool Contains(Vector2 point)
        {
            throw new System.NotImplementedException();
        }

        public bool Accept(IColliderVisitor visitor)
        {
            return visitor.Intersects(this);
        }

        public bool Intersects(ICollider other)
        {
            return other.Intersects(this);
        }
        public bool Intersects(Circle collider)
        {
            throw new System.NotImplementedException();
        }

        public bool Intersects(Rectangle collider)
        {
            throw new System.NotImplementedException();
        }

        public bool Intersects(Polygone collider)
        {
            throw new System.NotImplementedException();
        }
    }
}
