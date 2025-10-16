using System;
using System.Runtime.InteropServices;
using MonoVector2 = Microsoft.Xna.Framework.Vector2;

namespace MonoEngine.Engine.MathStuff;

/// <summary>Permet de gérer un vecteur bidimensionelle</summary>
/// <param name="X">Valeur X</param>
/// <param name="Y">Valeur Y</param>
[StructLayout(LayoutKind.Auto)]
public readonly record struct Vector2(float X, float Y)
{
    public Vector2() : this(0, 0)
    {
    }

    /// <summary>Vecteur : X = 0 et Y = 0</summary>
    public static Vector2 Null => new();

    /// <summary>Norm / Intensité</summary>
    public readonly float Norm => MathF.Sqrt((X * X) + (Y * Y));

    /// <summary>Norm / Intensité au carré</summary>
    public readonly float SquaredNorm => (X * X) + (Y * Y);

    /// <summary>Angle en radiant (0 rad étant à droite)</summary>
    /// <remarks>L'angle résultat est compris entre -0.5PI et 1.5Pi</remarks>
    public float AngleRad
    {
        get
        {
            if (float.IsNull(X))
            {
                return Y switch
                {
                    < 0 => MathF.PI / -2f,
                    > 0 => MathF.PI / 2,
                    _ => float.NaN,
                };
            }

            float value = MathF.Atan(Y / X);
            return X > 0 ? value : value + MathF.PI;
        }
    }

    /// <summary>Vecteur normalisé</summary>
    public readonly Vector2 Normalized => new(X / Norm, Y / Norm);

    /// <summary>Créer un vecteur à partir des données polaire</summary>
    public static Vector2 CreatePolar(float norm, float angleRad)
    {
        (float sin, float cos) = MathF.SinCos(angleRad);
        return new(norm * cos, norm * sin);
    }

    /// <summary>Calcul la distance entre ce vecteur et celui en paramêtre</summary>
    public readonly float GetDistance(Vector2 point)
    {
        float deltaX = X - point.X;
        float deltaY = Y - point.Y;
        return MathF.Sqrt((deltaX * deltaX) + (deltaY * deltaY));
    }

    /// <summary>Calcul le carré de la distance entre ce vecteur et celui en paramêtre</summary>
    internal readonly float GetSquaredDistance(Vector2 point)
    {
        float deltaX = X - point.X;
        float deltaY = Y - point.Y;
        return (deltaX * deltaX) + (deltaY * deltaY);
    }

    /// <summary>Calcul la direction entre ce vecteur et celui en paramêtre</summary>
    public readonly Vector2 GetDirection(Vector2 point) => this - point;

    /// <summary>Calcul la direction entre ce vecteur et celui en paramêtre avec un vecteur normalisé</summary>
    public readonly Vector2 GetNormDirection(Vector2 point) => (this - point).Normalized;

    /// <summary>Projete ce vecteur sur celui en paramêtre</summary>
    internal readonly Vector2 ProjectionOn(Vector2 other)
        => other * this / other.SquaredNorm * other; // équivalent a (this * other.Normalized) * other.Normalized

    /// <summary>Calcul un vecteur orthogonal à celui si</summary>
#pragma warning disable S2234 // Arguments should be passed in the same order as the method parameters
    public readonly Vector2 Orthogonal => new(Y, -X);
#pragma warning restore S2234 // Arguments should be passed in the same order as the method parameters

    public static Vector2 operator +(Vector2 operand) => operand;

    public static Vector2 Plus(Vector2 item) => item;

    public static Vector2 operator +(Vector2 left, Vector2 right) => Add(left, right);

    public static Vector2 Add(Vector2 left, Vector2 right) => new(left.X + right.X, left.Y + right.Y);

    public static Vector2 operator -(Vector2 left, Vector2 right) => Subtract(left, right);

    public static Vector2 Subtract(Vector2 left, Vector2 right) => new(left.X - right.X, left.Y - right.Y);

    public static Vector2 operator -(Vector2 operand) => Negate(operand);

    public static Vector2 Negate(Vector2 item) => new(-item.X, -item.Y);

    public static Vector2 operator *(Vector2 left, float right) => Multiply(left, right);

    public static Vector2 operator *(float left, Vector2 right) => Multiply(right, left);

    public static float operator *(Vector2 left, Vector2 right) => Multiply(left, right);

    public static Vector2 Multiply(Vector2 left, float right) => new(left.X * right, left.Y * right);

    public static float Multiply(Vector2 left, Vector2 right) => (left.X * right.X) + (left.Y * right.Y);

    public static Vector2 operator /(Vector2 left, float right) => Divide(left, right);

    public static Vector2 Divide(Vector2 left, float right) => new(left.X / right, left.Y / right);

    public static implicit operator MonoVector2(Vector2 d) => d.ToVector2();

    public readonly MonoVector2 ToVector2() => new(X, Y);

    public override readonly string ToString() => $"({X}; {Y})";
}