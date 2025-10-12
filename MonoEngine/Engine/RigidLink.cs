using MonoEngine.Engine.MathStuff;
using System;
using System.Diagnostics;

namespace MonoEngine.Engine
{
    /// <summary>
    /// Permet de géré un lien rigide entre deux objets phisiques
    /// </summary>
    public class RigidLink
    {
        PhysicCircle start;
        PhysicCircle end;

        /// <summary>
        /// Retourn un tableau contenant les 2 points que le lien relie
        /// </summary>
        public PhysicCircle[] Edges
        {
            get { return [start, end]; }
        }

        float length;
        /// <summary>
        /// Longueur du lien
        /// </summary>
        public float Length
        {
            get { return length; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="start">Premier objet</param>
        /// <param name="end">Deuxième objet</param>
        /// <param name="length">Longueur</param>
        public RigidLink(PhysicCircle start, PhysicCircle end, float length = 150) 
        {
            this.start = start;
            this.end = end;
            this.length = length;

            start.onMovement += OnStartMove;
        }

        /// <summary>
        /// Quand l'objet "start" effectue un movement
        /// </summary>
        /// <param name="deltaTime"></param>
        private void OnStartMove(float deltaTime)
        {
            // A CORRIGER UTILISER Vector2.GetDirection()
            Vector2 delta = end.Position - start.Position;
            delta *= start.Position.X < end.Position.X ? 1 : -1;
            Vector2 direction = delta.Normalized;

            float displacement = delta.Norm - length;

            if (displacement == 0) return;

            if (displacement < 0)
            {
                // Pour évité que les deux points ne se raproche
                start.Block(direction);
                return;
            }
            // Pour évité que les deux points ne s'éloigne
            start.RedirectMovement(deltaTime, -direction);

        }

    }
}
