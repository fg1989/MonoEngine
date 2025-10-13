

using Microsoft.Xna.Framework.Graphics;
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
    public class PhysicCircle : PhysicalObject
    {
        private float radius;

        /// <summary>
        /// Rayon
        /// </summary>
        public float Radius { get { return radius; } }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="x">Position x</param>
        /// <param name="y">Position y</param>
        /// <param name="mass">la Masse</param>
        /// <param name="radius">le Rayon</param>

        public PhysicCircle(float x, float y, float mass = 1, float radius = 5) : base(x, y, mass) 
        {
            this.radius = radius;
            ApplyForce(new Vector2(0, Const.GRAVITY * mass));
        }


    }
}
