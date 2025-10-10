using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;
using MonoVector2 = Microsoft.Xna.Framework.Vector2;

namespace MonoEngine.Engine.MathStuff
{
    public class Vector2 : IComparable<Vector2>
    {
        private float x;
        private float y;
        public float X
        {
            get { return x; }
            set { x = value; }
        }
        public float Y
        {
            get { return y; }
            set { y = value; }
        }
        public float Norm
        {
            get { return MathF.Sqrt(X * X + Y * Y); }
        }
        public float AngleRadian
        {
            get { return MathF.Atan(y/x); }
        }
        public Vector2 Normalized
        {
            get
            {
                return new Vector2(X / Norm, Y / Norm);
            }
        }

        private static Vector2 vectorNull = new Vector2(0, 0);
        public static Vector2 Null { get { return vectorNull; } }

        public Vector2(float x, float y)
        {
            this.x = x;
            this.y = y;
        }

        public float GetDistance(Vector2 point)
        {
            float deltaX = X - point.X;
            float deltaY = Y - point.Y;
            return MathF.Sqrt(deltaX * deltaX + deltaY * deltaY);
        }

        public Vector2 GetDirection(Vector2 point)
        {
            return (this - point).Normalized;
        }

        public static Vector2 operator +(Vector2 operand) => operand;
        public static Vector2 operator -(Vector2 operand) => new Vector2(-operand.X, -operand.Y);


        public static Vector2 operator +(Vector2 left, Vector2 right)
            => new Vector2(left.X + right.X, left.Y + right.Y);

        public static Vector2 operator -(Vector2 left, Vector2 right)
            => new Vector2(left.X - right.X, left.Y - right.Y);

        public static Vector2 operator *(Vector2 left, float right)
            => new Vector2(left.X * right, left.Y * right);

        public static float operator *(Vector2 left, Vector2 right)
            => left.X * right.X + left.Y * right.Y;
        public static Vector2 operator /(Vector2 left, float right)
            => new Vector2(left.X / right, left.Y / right);

        public static bool operator <(Vector2 left, Vector2 right)
            => left.Norm < right.Norm;

        public static bool operator >(Vector2 left, Vector2 right)
            => left.Norm > right.Norm;

        public static implicit operator MonoVector2(Vector2 d) => new MonoVector2(d.x,d.y);

        public override bool Equals(object obj)
        {
            return obj is Vector2 v && v.X == X && v.Y == Y;
        }

        public int CompareTo(Vector2 other)
        {
            return Norm.CompareTo(other.Norm);
        }
        public override string ToString()
        {
            return $"({x}; {y})";
        }
    }
}
