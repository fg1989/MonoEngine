using MonoEngine.Engine.MathStuff;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoEngine.Engine.Collider
{
    public interface ICollider
    {
        public Vector2 Center { get; }

        public bool Contains(Vector2 point);
        public Vector2 Intersects(ICollider collider);
        public Vector2 Accept(IColliderVisitor visitor);
    }
}
