using MonoEngine.Engine.MathStuff;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        public Vector2[] Points
        {
            get => points.ToArray();
            private set
            {
                points = value.ToList();
                convex = IsConvex();
            }
        }
        public Segment[] Segments
        {
            get
            {
                Segment[] segments = new Segment[points.Count];

                for (int i = 0; i < Points.Length; i++)
                {
                    segments[i] = new Segment(Points[i], Points[(i + 1) % (Points.Length - 1)]);

                }
                return segments;

            }
        }

        public Vector2 Center
        {
            get => points.Aggregate(Vector2.Null, (accumulate, vector) => vector + accumulate) / points.Count; // Moyenne des points
        }


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
                    Vector2.CreatePolar(radius, i * (MathF.PI * 2 / side) - MathF.PI / 2)
                    + centerPosition);
            }
        }

        private bool IsConvex()
        {
            return true;
        }

        /// <summary>
        /// Vérifie si un point est dans le Polygone
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public bool Contains(Vector2 point)
        {
            if (convex)
            {
                foreach (var segment in Segments)
                {
                    if(!segment.IsAtRight(point))
                        return false;
                }
                return true;
            }
            // A FAIRE Si polygone non-convex
            return false;
        }

        public bool Accept(IColliderVisitor visitor)
        {
            return visitor.Intersects(this);
        }

        /// <summary>
        /// Permet de vérifier si le polygone touche un Collider (casting automatique)
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Intersects(ICollider other)
        {
            return other.Accept(this);
        }
        public bool Intersects(Segment segment)
        {
            return segment.Intersects(this);
        }

        /// <summary>
        /// Permet de vérifier si le polygone touche un cercle
        /// </summary>
        /// <param name="circle"></param>
        /// <returns></returns> 
        public bool Intersects(Circle circle)
        {
            
            if (Contains(circle.Center))
                return true;

            foreach (Segment segment in Segments)
                {
                    if(segment.Intersects(circle))
                        return true;
                }
            
            return false;

        }

        /// <summary>
        /// Permet de vérifier si le polygone touche un rectangle
        /// </summary>
        /// <param name="rectangle"></param>
        /// <returns></returns>
        public bool Intersects(Rectangle rectangle)
        {
            return rectangle.Intersects(this);
        }

        /// <summary>
        /// Permet de vérifier si le polygone touche un rectangle
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Intersects(Polygone other)
        {
            foreach (var item in other.Points)
            {
                if(Contains(item)) return true;
            }
            foreach (var item in points)
            {
                if(other.Contains(item)) return true;
            }
            return false;
        }

    }
}
