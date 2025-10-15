using Microsoft.Xna.Framework.Graphics;
using MonoEngine.Engine.Collider;
using MonoEngine.Engine.MathStuff;
using System;

namespace MonoEngine.Engine
{
    public abstract class PhysicalObject
    {
        public abstract ICollider Collison { get; }
        public Vector2 Position
        {
            get => Collison.Center;
        }


        float mass;
        /// <summary>
        /// Mass de l'objet
        /// </summary>
        public float Mass { get => mass;  }


        Vector2 velocity = Vector2.Null;

        /// <summary>
        /// Vitesse vectorielle de l'objet
        /// </summary>
        public Vector2 Velocity
        {
            get => velocity;
        }

        Vector2 acceleration = Vector2.Null;

        /// <summary>
        /// Accéleration de l'objet
        /// </summary>
        Vector2 Acceleration
        {
            get => acceleration;
        }

        /// <summary>
        /// Force de l'objet (Acceleration * mass)
        /// </summary>
        public Vector2 Force
        {
            get
            {
                return acceleration * mass;
            }
        }


        /// <summary>
        /// Quand l'objet rentre en movement
        /// </summary>
        public Update onMovement;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x">Position x</param>
        /// <param name="y">Position y</param>
        /// <param name="mass">la Masse</param>
        /// <param name="radius">le Rayon</param>

        protected PhysicalObject( float mass = 1, float radius = 5)
        {
            this.mass = mass;
            ApplyForce(new Vector2(0, Const.GRAVITY * mass));
        }

        /// <summary>
        /// Permet d'appliquer une force à cet objet
        /// </summary>
        /// <param name="newForce"></param>
        public void ApplyForce(Vector2 newForce)
        {
            acceleration += newForce / mass;
        }

        /// <summary>
        /// Permet d'empêcher un objet d'aller plus loin dans une direction
        /// </summary>
        /// <param name="direction"></param>
        public void Block(Vector2 direction)
        {
            // Vérifie si l'objet va dans la direction du blockage
            Vector2 v = direction + velocity.Normalized;
            if (MathF.Abs(v.X) + MathF.Abs(v.Y) < 1)
                return; // Si non ne rien faire

            // Si oui rediriger sa vitesse et son accélération dans un vecteur orthogonal à la
            Vector2 rightAngleDirection = direction.GetOrthogonal();
            velocity = velocity.ProjectionOn(rightAngleDirection);
            acceleration = acceleration.ProjectionOn(rightAngleDirection);

        }

        /// <summary>
        /// S'effectue à chaque update
        /// </summary>
        /// <param name="deltaTime">temps depuis la dernière update</param>
        public void Update(float deltaTime)
        {
            // A CORRIGER FRICTION DE L'AIRE
            Vector2 airDrag = GetAirFirction(); // Résitance de l'air
            ApplyForce(airDrag);

            UpdateSpeed(deltaTime);
            Movement(deltaTime);
            ApplyForce(-airDrag);
        }

        protected virtual Vector2 GetAirFirction()
        {
            return velocity * -Const.AIR_FRICTION;
        }


        /// <summary>
        /// Acctualise la vitesse de l'objet
        /// </summary>
        /// <param name="deltaTime"></param>
        private void UpdateSpeed(float deltaTime)
        {
            velocity += Acceleration * deltaTime;
        }

        /// <summary>
        /// Effectue un movement en fonction de la vitesse (s'effectue à chaque Update)
        /// </summary>
        /// <param name="deltaTime">temps depuis la dernière update</param>
        private void Movement(float deltaTime)
        {
            if (velocity != Vector2.Null)
            {
                GoTo(Position + velocity * deltaTime);
                onMovement?.Invoke(deltaTime);
            }
        }

        /// <summary>
        /// Permet d'annuler le movement précédent
        /// </summary>
        /// <param name="deltaTime">temps depuis la dernière update</param>
        private void UndoMovement(float deltaTime)
        {

            if (velocity != Vector2.Null)
                GoTo(Position - velocity * deltaTime);
        }

        /// <summary>
        /// Permet de modifier le dernier movement afin 
        /// de redirigé l'objet dans une direction orthogonal
        /// à la direction en paramaêtre ou de l'arrêter
        /// </summary>
        /// <param name="deltaTime">temps depuis la dernière update</param>
        /// <param name="direction">Direction de bloquage</param>
        public void RedirectMovement(float deltaTime, Vector2 direction)
        {
            UndoMovement(deltaTime);
            Block(direction);

            if (velocity != Vector2.Null)
                GoTo(Position + velocity * deltaTime);
        }

        protected abstract void GoTo(Vector2 newPosition);
    }
}
