using MonoEngine.Engine.Collider;
using MonoEngine.Engine.Collider.Figure;
using MonoEngine.Engine.MathStuff;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoEngine.Engine
{
    public class PhysicalPolygone : PhysicalObject
    {
        private Polygone polygone;
        public override ICollider Collison => polygone;

        public Polygone Polygone => polygone;

        public PhysicalPolygone(Polygone polygone, float mass = 1) : base(mass)
        {
            this.polygone = polygone;
        }

        protected override void GoTo(Vector2 newPosition)
        {
            Vector2 center = Position;
            Vector2[] newPoints = polygone.Points.Select(point=> point-center+newPosition).ToArray();
            polygone = new Polygone(newPoints);
        }
    }
}
