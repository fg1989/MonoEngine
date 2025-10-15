using MonoEngine.Engine.Collider;
using MonoEngine.Engine.Collider.Figure;
using MonoEngine.Engine.MathStuff;
using System;

namespace MonoEngine.Engine
{
    public class PhysicalRectangle : PhysicalObject
    {
        Rectangle rectangle;
        public Rectangle Rectangle { get => rectangle; }
        public Vector2 Size => Rectangle.Size;
        
        public override ICollider Collison => rectangle;


        /// <summary>
        /// 
        /// </summary>
        /// <param name="x">Position x</param>
        /// <param name="y">Position y</param>
        /// <param name="width">Largeur</param>
        /// <param name="height">Longueur</param>
        /// <param name="mass">la Masse</param>
        public PhysicalRectangle(float x, float y,float width, float height, float mass = 1) : base(mass)
        {
            rectangle = new Rectangle(x,y, width, height);
        }

        protected override void GoTo(Vector2 newPosition)
        {
            rectangle = new Rectangle(newPosition-Size/2, Size);
        }
    }
}
