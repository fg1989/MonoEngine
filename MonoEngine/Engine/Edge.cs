

using MonoEngine.Engine.MathStuff;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MonoEngine.Engine
{
    public class Edge
    {

        Vector2 position = Vector2.Null;
        public Vector2 Position
        {
            get { return position; }
        }

        float mass = 1;
        public float Mass { get { return mass; } }

        List<Vector2> forces = new List<Vector2>();
        public Vector2 Force
        {
            get {

                return forces.Aggregate(Vector2.Null, (sum, v) => sum + v);
            
            }
        }

        Vector2 velocity = Vector2.Null;
        public Vector2 Speed
        {
            get { return velocity; }
        }

        Vector2 Acceleration
        {
            get { return Force / mass ; }
        }

        public Edge()
        {
            forces.Add(new Vector2(0, ConstAndFunc.GRAVITY * mass));
        }
        public Edge(float x, float y, float mass) : this()
        {
            position = new Vector2(x, y);
            this.mass = mass;
        }



        public Func<bool> ApplyForce(Vector2 newForce)
        {
            forces.Add(newForce);
            return () => forces.Remove(newForce);
        }

        public void Update(float deltaTime)
        {
            Func<bool> RemoveAirResistance = null;
            if (velocity.Norm > 0)
            {
                Vector2 airDrag = velocity * -ConstAndFunc.AIR_FRICTION;
                RemoveAirResistance = ApplyForce(airDrag);

                System.Diagnostics.Debug.WriteLine(velocity);
            }


            UpdateSpeed(deltaTime);
            ApplySpeed(deltaTime);
            if (RemoveAirResistance != null)
            {
                RemoveAirResistance();
            }

        }

        private void UpdateSpeed(float deltaTime)
        {
            velocity += Acceleration * deltaTime;
        }
        private void ApplySpeed(float deltaTime)
        {
            position += velocity * deltaTime ;
        }
    }
}
