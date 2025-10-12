

using Microsoft.Xna.Framework.Graphics;
using MonoEngine.Engine.MathStuff;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        private float radius = 5;
        public float Radius { get { return radius; } }

        Vector2 acceleration = Vector2.Null;
        Vector2 Acceleration
        {
            get { return acceleration; }
        }
        public Vector2 Force
        {
            get {
                return acceleration*mass;
            }
        }

        Vector2 velocity = Vector2.Null;
        public Vector2 Velocity
        {
            get { return velocity; }
        }

        public delegate void OnMovement(float deltaTime);
        public OnMovement onMovement;


        public Edge()
        {
            ApplyForce(new Vector2(0, ConstAndFunc.GRAVITY * mass));
        }
        public Edge(float x, float y, float mass, float radius) : this()
        {
            position = new Vector2(x, y);
            this.mass = mass;
            this.radius = radius;
        }


        public void ApplyForce(Vector2 newForce)
        {
            acceleration += newForce/mass;
        }

        public void Block(Vector2 direction)
        {
            Debug.WriteLine(direction + velocity.Normalized);

            
            Vector2 v = direction + velocity.Normalized;
            if (MathF.Abs(v.X) + MathF.Abs(v.Y) < 1)
                return;

            Vector2 rightAngleDirection = direction.GetOrthogonal();
            velocity = velocity.ProjectionOn(rightAngleDirection);
            acceleration = acceleration.ProjectionOn(rightAngleDirection);

        }


        public void Update(float deltaTime)
        {

            Vector2 airDrag = velocity * -ConstAndFunc.AIR_FRICTION*deltaTime; // Résitance de l'air
            ApplyForce(airDrag);

            UpdateSpeed(deltaTime);
            Movement(deltaTime);
        }

        private void UpdateSpeed(float deltaTime)
        {
            velocity += Acceleration * deltaTime;
        }
        private void Movement(float deltaTime)
        {
            position += velocity * deltaTime ;
            onMovement?.Invoke(deltaTime);
        }
        public void UndoMovement(float deltaTime)
        {
            position -= velocity * deltaTime;
        }
        public void RedirectMovement(float deltaTime, Vector2 direction)
        {
            UndoMovement(deltaTime);
            Block(direction);
            position += velocity * deltaTime;
        }
    }
}
