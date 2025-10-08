

using MonoEngine.Engine.MathStuff;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MonoEngine.Engine
{
    public class Edge
    {
        Vector2 position;
        float mass;
        List<Vector2> forces;

        Vector2 Acceleration
        {
            get { return Force /  mass; }
        }

        public Vector2 Position
        {
            get { return position; }
        }

        public Vector2 Force
        {
            get { return forces.Aggregate(Vector2.Null, (sum,v) => sum + v); }
        }

        public Edge()
        {
            forces.Add(new Vector2(0, -ConstAndFunc.GRAVITY));
        }
        public Edge(float x, float y, float mass) : this() 
        {
            position = new Vector2(x, y);
            this.mass = mass;
        }

        public void ApplyForce(Vector2 newForce)
        {
            forces.Add(newForce);
        }
    }
}
