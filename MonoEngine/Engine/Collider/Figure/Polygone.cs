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
                for (int i = 0; i < Points.Length; i++)
                {
                    Vector2 start = Points[i];
                    Vector2 end = i == Points.Length - 1 ? Points[0] : Points[i + 1];
                    Vector2 side = start - end;
                    Vector2 t = point - end;

                    float direction = side.X * t.Y - side.Y * t.X;
                    if (direction < 0)
                        return false;
                }
                return true;
            }
            // A FAIRE Si polygone non-convex
            return false;
        }

        public Vector2 Accept(IColliderVisitor visitor)
        {
            return visitor.Intersects(this);
        }

        /// <summary>
        /// Permet de vérifier si le polygone touche un Collider (casting automatique)
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public Vector2 Intersects(ICollider other)
        {
            return other.Intersects(this);
        }

        /// <summary>
        /// Permet de vérifier si le polygone touche un cercle
        /// </summary>
        /// <param name="circle"></param>
        /// <returns></returns>
        public Vector2 Intersects(Circle circle)
        {
            Segment[] segments = Segments;
            if (Convex)
                foreach (Segment segment in segments)
                {
                    Vector2 centerCircle = circle.Center;
                    Vector2 nearest = segment.GetNearest(centerCircle);
                    Vector2 VectorSpaceBetweem = nearest - centerCircle;
                    float distance = (nearest - centerCircle).Norm;
                    if (distance <= circle.Radius)
                    {
                        return VectorSpaceBetweem;
                    }
                }

            return Vector2.Null;

        }

        /// <summary>
        /// Permet de vérifier si le polygone touche un rectangle
        /// </summary>
        /// <param name="rectangle"></param>
        /// <returns></returns>
        public Vector2 Intersects(Rectangle rectangle)
        {
            return rectangle.Intersects(this);
        }

        /// <summary>
        /// Permet de vérifier si le polygone touche un rectangle
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public Vector2 Intersects(Polygone other)
        {

            for (int i = 0; i < Points.Length; i++)
            {
                Vector2 point = Points[i];
                if (other.Contains(point))
                {
                    Segment[] otherSegments = other.Segments;
                    HashSet<Segment> segmentsIntersected = new HashSet<Segment>();
                    Segment[] segments = [
                        new Segment(Points[i <= 0 ? Points.Length-1 : i-1],point),
                        new Segment(point,Points[(i+1)%(Points.Length-1)])
                        ];
                    foreach (var otherSegment in otherSegments)
                    {
                        foreach (var segment in segments)
                        {
                            if (otherSegment.Intersects(segment))
                                segmentsIntersected.Add(otherSegment);
                        }
                    }
                    if (segmentsIntersected.Count == 1)
                    {
                        return segmentsIntersected.ToArray()[0].FromBase.Normalized;
                    }
                    if (segmentsIntersected.Count >= 2)
                    {
                        return (segmentsIntersected.ToArray()[0].FromBase - segmentsIntersected.ToArray()[1].FromBase).Normalized;
                    }

                }
            }
            for (int i = 0; i < other.Points.Length; i++)
            {
                Vector2 point = other.Points[i];
                if (Contains(point))
                {
                    Segment[] mySegments = Segments;
                    HashSet<Segment> segmentsIntersected = new HashSet<Segment>();
                    Segment[] segments = [
                        new Segment(other.Points[i <= 0 ? other.Points.Length-1 : i-1],point),
                        new Segment(point,other.Points[(i+1)%(other.Points.Length-1)])
                        ];
                    foreach (var otherSegment in mySegments)
                    {
                        foreach (var segment in segments)
                        {
                            if (otherSegment.Intersects(segment))
                                segmentsIntersected.Add(otherSegment);
                        }
                    }
                    if (segmentsIntersected.Count == 1)
                    {
                        return segmentsIntersected.ToArray()[0].FromBase.Normalized;
                    }
                    if (segmentsIntersected.Count >= 2)
                    {
                        return (segmentsIntersected.ToArray()[0].FromBase - segmentsIntersected.ToArray()[1].FromBase).Normalized;
                    }
                }
            }
            return Vector2.Null;
        }
    }
}
