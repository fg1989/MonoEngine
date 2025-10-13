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
            if (convex)
            {
                for (int i = 0; i < Points.Length; i++)
                {
                    Vector2 start = Points[i];
                    Vector2 end = i == Points.Length - 1 ? Points[0] : Points[i + 1];
                    Vector2 side = start - end;
                    Vector2 t = point - end;

                    float direction = side.X * t.Y - side.Y * t.X;
                    if (direction < 0)
                        return false;  // un point à droite et on arrête tout.
                }
                Debug.WriteLine("TOUCHER");
                return true;
            }
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
            return other.Intersects(this);
        }

        /// <summary>
        /// Permet de vérifier si le polygone touche un cercle
        /// </summary>
        /// <param name="circle"></param>
        /// <returns></returns>
        public bool Intersects(Circle circle)
        {
            foreach (Vector2 point in Points)
            {
                if (circle.Contains(point))
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
            foreach (Vector2 point in Points)
            {
                if (other.Contains(point))
                    return true;
            }
            return false;
        }
    }
}
