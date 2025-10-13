using MonoEngine.Engine.MathStuff;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoEngine.Engine.Collider.Figure
{
    public struct Polygone : ICollider, IColliderVisitor
    {

        public bool Contains(Vector2 point)
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }

        public bool Intersects(Rectangle collider)
        {
            throw new NotImplementedException();
        }

        public bool Intersects(Polygone collider)
        {
            throw new NotImplementedException();
        }
    }
}
