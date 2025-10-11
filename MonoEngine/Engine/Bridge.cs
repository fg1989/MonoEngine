using MonoEngine.Engine.MathStuff;
using System;
using System.Diagnostics;

namespace MonoEngine.Engine
{
    public class Bridge
    {
        Edge start;
        Edge end;
        public Edge[] Edges
        {
            get { return [start, end]; }
        }

        float length = 150;
        //float rigidity = 1000;
        //float absorption = 0.9f;
        public Bridge(Edge start, Edge end)
        {
            this.start = start;
            this.end = end;
        }
        public Bridge(Edge start, Edge end, float length, float rigidity, float absorption) : this(start, end)
        {
            this.length = length;

            //this.rigidity = rigidity;
            //this.absorption = absorption;
        }

        Func<bool>[] remove = new Func<bool>[2];
        public void Update(float deltaTime)
        {
            ClearForce();
            Vector2 delta = end.Position - start.Position;
            delta *= start.Position.X < end.Position.X ? 1 : -1;


            Vector2 direction = delta.Normalized;

            float displacement = delta.Norm - length;

            if (displacement == 0) return;

            start.Block((direction * displacement).Normalized);
            remove[0] = start.ApplyForce(-start.Force.ProjectionOn(direction));

        }
        void ClearForce()
        {
            foreach (var item in remove)
            {
                if (item != null)
                    item();
            }
        }
    }
}
