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
        public Vector2 Center { get; set; }
        public bool Contains(Vector2 point);
        public bool Intersects(ICollider collider);
        public bool Accept(IColliderVisitor visitor);
    }
}
