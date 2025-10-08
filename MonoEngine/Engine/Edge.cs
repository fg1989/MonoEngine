

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
        Vector2 Acceleration
        {
            get { return Force / mass; }
        }


        List<Vector2> forces = new List<Vector2>();
        public Vector2 Force
        {
            get { return forces.Aggregate(Vector2.Null, (sum, v) => sum + v); }
        }

        Vector2 speed = Vector2.Null;
        public Vector2 Speed 
        { 
            get { return speed; } 
        }
        public Edge()
        {
            forces.Add(new Vector2(0, ConstAndFunc.GRAVITY*mass));
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
        public void RemoveForce(Vector2 oldForce)
        {
            if (!forces.Remove(oldForce))
            {
                throw new Exception("This Force doesn't exist : " + oldForce);
            }
        }

        public void Update(float deltaTime)
        {
            UpdateSpeed(deltaTime);
            ApplySpeed(deltaTime);
        }

        private void UpdateSpeed(float deltaTime)
        {
            speed += Acceleration * deltaTime;
        }
        private void ApplySpeed(float deltaTime)
        {
            position += speed * deltaTime;
        }
    }
}
