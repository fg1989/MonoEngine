using MonoEngine.Engine.MathStuff;
using System;
using System.Collections.Generic;
using System.Linq;
using MonoRectangle = Microsoft.Xna.Framework.Rectangle;

namespace MonoEngine.Engine.Collider.Figure
{
    public struct Rectangle : ICollider, IColliderVisitor
    {
        private Vector2 position;
        /// <summary>
        /// la position en haut à gauche du rectangle
        /// </summary>
        public Vector2 Position
        {
            get => position;
        }
        private Vector2 size;
        /// <summary>
        /// Taille du rectangle
        /// </summary>
        public Vector2 Size
        {
            get => size;
        }

        public float X { get => position.X; }
        public float Y { get => position.Y; }
        public float Width { get => size.X; }
        public float Height { get => size.Y; }

        public float Left { get => position.X; }
        public float Top { get => position.Y; }
        public float Right { get => position.X + size.X; }

        public float Bottom { get => position.Y + size.Y; }

        /// <summary>
        /// Position du centre du rectangle
        /// </summary>
        public Vector2 Center
        {
            get => position + (size / 2);
        }

        /// <summary>
        /// Les 4 coins du rectangle
        /// </summary>
        public Vector2[] Points
        {
            get => [
                position,                           // En haut à gauche
                new Vector2(X+size.X, Y),           // En haut à droite
                new Vector2(X+size.X, Y+size.Y),    // En bas à droite
                new Vector2(X, Y+size.Y)            // En bas à gauche
            ];
        }
        public Segment[] Segments
        {
            get =>[
                new Segment(Points[0],Points[1]),   // En haut
                new Segment(Points[1],Points[2]),   // A droite
                new Segment(Points[2],Points[3]),   // En bas
                new Segment(Points[3],Points[0])    // A gauche
                ];
            
        }

        public Rectangle(float x, float y, float width, float height)
        {
            position = new Vector2(x, y);
            size = new Vector2(width, height);
        }

        public Rectangle(Vector2 position, Vector2 size)
        {
            this.position = position;
            this.size = size;
        }

        public static implicit operator MonoRectangle(Rectangle d) => new MonoRectangle((int)d.X, (int)d.Y, (int)d.Width, (int)d.Height);


        /// <summary>
        /// Vérifie si le rectangle contient un point donné
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public bool Contains(Vector2 point)
        {

            return
               point.X >= X
            && point.X < X + Width
            && point.Y >= Y
            && point.Y < Y + Height;
        }

        public Vector2 Accept(IColliderVisitor visitor)
        {
            return visitor.Intersects(this);
        }

        /// <summary>
        /// Permet de vérifier si 2 ICollider sont en collision
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public Vector2 Intersects(ICollider other)
        {
            // permet d'apeler une fonction différente en fonction du type de collider
            return other.Accept(this);
        }

        /// <summary>
        /// Permet de vérifier si le rectangle est en collision avec un cercle
        /// </summary>
        /// <param name="circle"></param>
        /// <returns></returns>
        public Vector2 Intersects(Circle circle)
        {
            float radius = circle.Radius;

            foreach (var item in Points)
            {
                if (circle.Contains(item))
                    return (circle.Center - Center).GetIn8Directions();
            }

            Vector2[] points = [
                new Vector2(circle.Center.X+radius, circle.Center.Y),
                new Vector2(circle.Center.X-radius, circle.Center.Y),
                new Vector2(circle.Center.X, circle.Center.Y+radius),
                new Vector2(circle.Center.X, circle.Center.Y-radius),
                ];
            foreach (var item in points)
            {
                if (Contains(item))
                    return (circle.Center - Center).GetIn8Directions();
            }
            return Vector2.Null;

        }

        /// <summary>
        /// Permet de vérifier si 2 Rectangles sont en collision
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public Vector2 Intersects(Rectangle other)
        {
            foreach (var edge in Points)
            {
                if (other.Contains(edge))
                {
                    Vector2 distance =  other.Center - edge;
                    return distance.GetIn8Directions();
                }
            }

            foreach (var edge in other.Points)
            {
                if (Contains(edge))
                {
                    Vector2 distance = edge - Center;        
                    return distance.GetIn8Directions();
                }
            }
            return Vector2.Null;
        }

        /// <summary>
        /// Permet de vérifier si le rectangle est en collision avec un polygone
        /// </summary>
        /// <param name="polygone"></param>
        /// <returns></returns>
        public Vector2 Intersects(Polygone polygone)
        {
            for (int i = 0; i < polygone.Points.Length; i++)
            {
                Vector2 point = polygone.Points[i];
                if (Contains(point))
                {
                    Segment[] mySegments = polygone.Segments;
                    HashSet<Segment> segmentsIntersected = new HashSet<Segment>();
                    Segment[] segments = [
                        new Segment(polygone.Points[i <= 0 ? polygone.Points.Length-1 : i-1],point),
                        new Segment(point,polygone.Points[(i+1)%(polygone.Points.Length-1)])
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
            for (int i = 0; i < Points.Length; i++)
            {
                Vector2 point = Points[i];
                if (Points.Contains(point))
                {
                    Segment[] mySegments = Segments;
                    HashSet<Segment> segmentsIntersected = new HashSet<Segment>();
                    Segment[] segments = [
                        new Segment(Points[i <= 0 ? Points.Length-1 : i-1],point),
                        new Segment(point,Points[(i+1)%(Points.Length-1)])
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
