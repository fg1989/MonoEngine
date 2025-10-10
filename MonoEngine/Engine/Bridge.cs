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
        float rigidity = 1000;
        float absorption = 0.9f;
        public Bridge(Edge start, Edge end)
        {
            this.start = start;
            this.end = end;
        }
        public Bridge(Edge start, Edge end,float length, float rigidity, float absorption) : this(start, end)
        {
            this.rigidity = rigidity;
            this.length = length;
            this.absorption = absorption;
        }

        Func<bool>[] remove = new Func<bool>[2];
        public void Update(float deltaTime)
        {
            ClearForce();

            Vector2 delta = end.Position - start.Position;
            
            if (delta.Norm == 0) return;

            Vector2 direction = delta.Normalized; 

            // Écart à la longueur d’origine
            float displacement = delta.Norm - length;

            // Force élastique selon la loi de Hooke
            Vector2 springForce = -rigidity * displacement * direction;

            // Amortissement : freine la différence de vitesse
            Vector2 relativeVelocity = end.Speed - start.Speed;
            Vector2 dampingForce = -delta.Norm * (relativeVelocity * direction) * direction;

            // Force totale
            Vector2 totalForce = springForce + dampingForce;

            remove[0] = start.ApplyForce(-totalForce);
            remove[1] = end.ApplyForce(totalForce);

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
