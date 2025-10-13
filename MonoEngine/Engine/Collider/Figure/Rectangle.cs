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
        private Vector2 size;

        public float X { get => position.X; }
        public float Y { get => position.Y; }
        public float Width { get => size.X; }
        public float Height { get => size.Y; }

        /// <summary>
        /// Position du centre du rectangle
        /// </summary>
        public Vector2 Center
        {
            get => position + (size / 2);
            set => position = value - (size / 2);
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
            float x = MathF.Min(point.X, position.X + size.X);
            MathF.Max(x, position.X);

            float y = MathF.Min(point.Y, position.Y + size.Y);
            MathF.Max(y, position.Y);

            return x == point.X && y == point.Y;
        }

        public bool Accept(IColliderVisitor visitor)
        {
            return visitor.Intersects(this);
        }

        /// <summary>
        /// Permet de vérifier si 2 ICollider sont en collision
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Intersects(ICollider other)
        {
            // permet d'apeler une fonction différente en fonction du type de collider
            return other.Intersects(this);
        }

        /// <summary>
        /// Permet de vérifier si le rectangle est en collision avec un cercle
        /// </summary>
        /// <param name="circle"></param>
        /// <returns></returns>
        public bool Intersects(Circle circle)
        {
            foreach (var edge in Edges)
            {
                if (circle.Contains(edge))
                    Debug.WriteLine("TOUCHER");
                return true;
            }
            return false;
        }

        /// <summary>
        /// Permet de vérifier si 2 Rectangles sont en collision
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Intersects(Rectangle other)
        {
            foreach (var edge in Edges)
            {
                if (other.Contains(edge))
                    Debug.WriteLine("TOUCHER");
                return true;
            }
            return false;
        }

        /// <summary>
        /// Permet de vérifier si le rectangle est en collision avec un polygone
        /// </summary>
        /// <param name="polygone"></param>
        /// <returns></returns>
        public bool Intersects(Polygone polygone)
        {
            foreach (var item in polygone.Points)
            {
                if (Contains(item))
                    Debug.WriteLine("TOUCHER");
                return true;
            }
            return false;
        }
    }
}
