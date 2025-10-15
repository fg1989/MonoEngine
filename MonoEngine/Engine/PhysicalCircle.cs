

using Microsoft.Xna.Framework.Graphics;
using MonoEngine.Engine.Collider;
using MonoEngine.Engine.MathStuff;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace MonoEngine.Engine
{
    /// <summary>
    /// Permet de géré la physique d'un cercle 
    /// A FAIRE CREER CLASSE PARENT
    /// </summary>
    public class PhysicalCircle : PhysicalObject
    {

        private Circle circle;
        /// <summary>
        /// Figure de collision
        /// </summary>
        /// 

        public override ICollider Collison { get => circle; }

        public Circle Circle { get => circle; }


        /// <summary>
        /// Rayon
        /// </summary>
        public float Radius { get => circle.Radius; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x">Position x</param>
        /// <param name="y">Position y</param>
        /// <param name="mass">la Masse</param>
        /// <param name="radius">le Rayon</param>
        public PhysicalCircle(float x, float y, float mass = 1, float radius = 5) : base(mass) 
        {
            circle = new Circle(new Vector2(x,y),radius);
        }


        protected override void GoTo(Vector2 newPosition)
        {
            circle = new Circle(newPosition, Radius);
        }
    }
}
