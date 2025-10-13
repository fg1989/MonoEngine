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
        bool convex;
        public bool Convex
        {
            get => convex;
        }
        List<Vector2> points;
        public Vector2[] Points { 
            get => points.ToArray();
            set
            {
                points = value.ToList();
                convex = IsConvex();
            }
        }

        public Vector2 Center { 
            get => points.Aggregate(Vector2.Null, (accumulate, vector) => vector + accumulate)/points.Count; // Moyenne des points
            set {
                Vector2 oldCenter = Center;
                points = points.Select(vector => vector - oldCenter + value).ToList();
            } }


        public Polygone(Vector2[] points)
        {
            Points = points;
        }

        public Polygone(Vector2 centerPosition, float radius, float side)
        {
            points = new List<Vector2>();

            for (int i = 0; i < side; i++)
            {
                points.Add(
                    Vector2.CreatePolar(radius, i* (MathF.PI*2/side) - MathF.PI/2)
                    + centerPosition);
            }
        }

        private bool IsConvex()
        {
            return true;
            for (int i = 0; i < points.Count; i++)
            {
                
            }
        }

        /// <summary>
        /// Vérifie si un point est dans le Polygone
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public bool Contains(Vector2 point)
        {
            return false;
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
