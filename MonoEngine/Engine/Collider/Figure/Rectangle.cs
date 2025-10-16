using MonoEngine.Engine.MathStuff;
using System;
using System.Diagnostics;
using MonoRectangle = Microsoft.Xna.Framework.Rectangle;
using Rectangle = MonoEngine.Engine.Collider.Figure.Rectangle;

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
        public Vector2[] Edges 
        { 
            get =>  [
                position,                           // En haut à gauche
                new Vector2(X+size.X, Y),           // En haut à droite
                new Vector2(X+size.X, Y+size.Y),    // En bas à droite
                new Vector2(X, Y+size.Y)            // En bas à gauche
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
        public bool Intersects(Circle circle)
        {
            float radius = circle.Radius;
            Vector2[] points = [
                new Vector2(circle.Center.X+radius, circle.Center.Y),
                new Vector2(circle.Center.X-radius, circle.Center.Y),
                new Vector2(circle.Center.X, circle.Center.Y+radius),
                new Vector2(circle.Center.X, circle.Center.Y-radius),
                ];
            foreach (var item in points)
            {
                if (Contains(item))
                    return true;
            }
            foreach (var item in Edges)
            {
                if (circle.Contains(item))
                    return true;
            }
            return false;

        }

        /// <summary>
        /// Permet de vérifier si 2 Rectangles sont en collision
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Intersects(Rectangle other, out Vector2 direction)
        {
            foreach (var edge in Edges)
            {
                if (other.Contains(edge))
                {
                    Vector2 distance = new Vector2(edge.X - other.Center.X, edge.Y - other.Center.Y);
                    if (distance.X > distance.Y)
                    {
                        return new Vector2(1);
                    }
                    if (distance.X < distance.Y)
                    {

                    }
                    if (distance.X == distance.Y)
                    {
                    }

                }
            }
            foreach (var edge in other.Edges)
            {
                if (Contains(edge))
                    return true;
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
            foreach (var item in polygone.Points)
            {
                if (Contains(item))
                return true;
            }
            foreach (var edge in Edges)
            {
                if (polygone.Contains(edge))
                    return true;
            }
            return false;
        }
    }
}
