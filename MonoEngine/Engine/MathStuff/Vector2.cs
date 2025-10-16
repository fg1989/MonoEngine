using System;
using MonoVector2 = Microsoft.Xna.Framework.Vector2;

namespace MonoEngine.Engine.MathStuff
{
    /// <summary>
    /// Permet de gérer un vecteur bidimensionelle
    /// </summary>
    public struct Vector2 : IEquatable<Vector2>
    {
        private static Vector2 vectorNull = new Vector2(0, 0);
        /// <summary>
        ///  Vecteur : X = 0 et Y = 0
        /// </summary>
        public static Vector2 Null { get { return vectorNull; } }

        private float x;
        private float y;

        /// <summary>
        /// Valeur X
        /// </summary>
        public float X
        {
            get { return x; }
            set { x = value; }
        }

        /// <summary>
        /// Valeur Y
        /// </summary>
        public float Y
        {
            get { return y; }
            set { y = value; }
        }

        /// <summary>
        /// Norm / Intensité
        /// </summary>
        public float Norm
        {
            get { return MathF.Sqrt(X * X + Y * Y); }
        }

        /// <summary>
        /// Norm / Intensité au carré
        /// </summary>
        public float SquaredNorm
        {
            get { return X * X + Y * Y; }
        }


        /// <summary>
        /// Angle en radiant (0 rad étant à droite)
        /// </summary>
        public float AngleRad
        {
            get
            {
                if (x == 0)
                    return 0;
                float value = MathF.Atan(y / x);
                return x > 0 ? value : value + MathF.PI;
            }
            set
            {
                float norm = Norm;
                float angleRad = value;
                Vector2 newVector = CreatePolar(norm, angleRad);
                x = newVector.X;
                y = newVector.Y;
            }
        }

        /// <summary>
        /// Vecteur normalisé
        /// </summary>
        public Vector2 Normalized
        {
            get
            {
                return new Vector2(X / Norm, Y / Norm);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x">Valeur X</param>
        /// <param name="y">Valeur Y</param>
        public Vector2(float x, float y)
        {
            this.x = x;
            this.y = y;
        }

        /// <summary>
        /// Créer un vecteur à partir des données polaire
        /// </summary>
        /// <param name="norm"></param>
        /// <param name="angleRad"></param>
        /// <returns></returns>
        public static Vector2 CreatePolar(float norm, float angleRad)
        {
            return new Vector2(norm * MathF.Cos(angleRad), norm * MathF.Sin(angleRad));
        }

        /// <summary>
        /// Calcul la distance entre ce vecteur et celui en paramêtre
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public float GetDistance(Vector2 point)
        {
            float deltaX = X - point.X;
            float deltaY = Y - point.Y;
            return MathF.Sqrt(deltaX * deltaX + deltaY * deltaY);
        }

        /// <summary>
        /// Calcul la direction entre ce vecteur et celui en paramêtre
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public Vector2 GetDirection(Vector2 point)
        {
            return (this - point).Normalized;
        }

        /// <summary>
        /// Projete ce vecteur sur celui en paramêtre
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public Vector2 ProjectOn(Vector2 other)
        {
            return other * this / other.SquaredNorm * other;
        }

        /// <summary>
        /// Calcul un vecteur orthogonal à celui si
        /// </summary>
        /// <returns></returns>
        public Vector2 GetOrthogonal()
        {
            if (X == 0 || Y == 0)
            {
                return new Vector2(Y, X);
            }
            return new Vector2(1 / X, -1 / Y).Normalized;
        }

        /// <summary>
        /// Cette fonction renvoie un vecteur unitaire indiquant la direction principale
        /// (horizontale, verticale ou diagonale) du vecteur donné, arrondie aux 8 directions cardinales.
        /// </summary>
        /// <param name="vector"></param>
        /// <returns></returns>
        public Vector2 GetIn8Directions()
        {
            if (MathF.Abs(X) > MathF.Abs(Y))
            {
                return new Vector2(
                    X < 0 ? -1 : 1,
                    0);
            }
            if (MathF.Abs(X) < MathF.Abs(Y))
            {
                return new Vector2(
                    0,
                    Y < 0 ? -1 : 1);
            }
            if (MathF.Abs(X) == MathF.Abs(Y))
            {
                return new Vector2(
                    X < 0 ? -1 : 1,
                    Y < 0 ? -1 : 1).Normalized;
            }
            return Null;
        }

        public float Determinant(Vector2 other)
        {
            return X*other.Y - Y*other.X;
        }

        public static bool operator ==(Vector2 left, Vector2 right)
        => left.Equals(right);

        public static bool operator !=(Vector2 left, Vector2 right)
        => !(left == right);


        public static Vector2 operator +(Vector2 operand) => operand;
        public static Vector2 operator -(Vector2 operand) => new Vector2(-operand.X, -operand.Y);

        public static Vector2 operator +(Vector2 left, Vector2 right)
            => new Vector2(left.X + right.X, left.Y + right.Y);

        public static Vector2 operator -(Vector2 left, Vector2 right)
            => new Vector2(left.X - right.X, left.Y - right.Y);

        public static Vector2 operator *(Vector2 left, float right)
            => new Vector2(left.X * right, left.Y * right);
        public static Vector2 operator *(float left, Vector2 right)
            => new Vector2(right.X * left, right.Y * left);

        public static float operator *(Vector2 left, Vector2 right)
            => left.X * right.X + left.Y * right.Y;
        public static Vector2 operator /(Vector2 left, float right)
            => new Vector2(left.X / right, left.Y / right);


        public static implicit operator MonoVector2(Vector2 d) => new MonoVector2(d.x, d.y);

        public override bool Equals(object obj)
        {
            return obj is Vector2 v && Equals(v);
        }
        public bool Equals(Vector2 other)
        {
            return other.X == X && other.Y == Y;
        }

        public override int GetHashCode()
        {
            return X.GetHashCode() ^ Y.GetHashCode();
        }

        public override string ToString()
        {
            return $"({x}; {y})";
        }

    }
}
