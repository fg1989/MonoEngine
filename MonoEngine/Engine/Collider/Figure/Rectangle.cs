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
            return other.Accept(this);
        }
        public bool Intersects(Segment segment)
        {
            return segment.Intersects(this);
        }

        /// <summary>
        /// Permet de vérifier si le rectangle est en collision avec un cercle
        /// </summary>
        /// <param name="circle"></param>
        /// <returns></returns>
        public bool Intersects(Circle circle)
        {
            if (Contains(circle.Center)) 
                return true;

            foreach (var item in Segments)
            {
                if (item.Intersects(circle))
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
            float deltaX = (Right - other.Left) * (other.Right - Left);

            float deltaY = (Top - other.Bottom) * (other.Top - Bottom);

            return deltaX > 0 && deltaY > 0;
        }

        /// <summary>
        /// Permet de vérifier si le rectangle est en collision avec un polygone
        /// </summary>
        /// <param name="polygone"></param>
        /// <returns></returns>
        public bool Intersects(Polygone polygone)
        {
            if (Contains(polygone.Center))
                return true;

            foreach (var item in Segments)
            {
                if (item.Intersects(polygone))
                    return true;
            }
            return false;
        }

    }
}
