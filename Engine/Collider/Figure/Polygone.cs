using Engine.MathStuff;
using System;
using System.Collections.Generic;

namespace Engine.Collider.Figure;

#pragma warning disable CA1819 // Properties should not return arrays
#pragma warning disable MA0109 // Consider adding an overload with a Span<T> or Memory<T>
public readonly record struct Polygone(Vector2[] Points) : ICollider
#pragma warning restore MA0109 // Consider adding an overload with a Span<T> or Memory<T>
#pragma warning restore CA1819 // Properties should not return arrays
{
    public Polygone(Vector2 centerPosition, float radius, float side) : this(CalculateRegularPolygon(centerPosition, radius, side))
    {
    }

    private static Vector2[] CalculateRegularPolygon(Vector2 centerPosition, float radius, float side)
    {
        List<Vector2> points = [];

        for (int i = 0; i < side; i++)
            points.Add(Vector2.CreatePolar(radius, (i * (MathF.PI * 2 / side)) - (MathF.PI / 2)) + centerPosition);

        return [.. points];
    }

    /// <summary>Vérifie si un point est dans le Polygone</summary>
    public readonly bool Contains(Vector2 point)
    {
        for (int i = 0; i < Points.Length - 1; i++)
        {
            Vector2 start = Points[i];
            Vector2 end = Points[i + 1];
            Vector2 side = start - end;
            Vector2 t = point - end;

            if ((side.X * t.Y) > (side.Y * t.X))
                return false;
        }

        Vector2 st = Points[^1];
        Vector2 e = Points[0];
        Vector2 si = st - e;
        Vector2 tt = point - e;

        return (si.X * tt.Y) <= (si.Y * tt.X);
    }

    readonly T ICollider.Accept<T>(IColliderVisitor<T> visitor) => visitor.Visit(this);

    public void Accept(IColliderVisitor visitor) => visitor.Visit(this);

    /// <summary>Permet de vérifier si le polygone touche un cercle</summary>
    /// <exception cref="NotSupportedException"></exception>
    public bool Visit(Circle circle) => false; // TODO : Ne marche pas

    /// <summary>Permet de vérifier si le polygone touche un rectangle</summary>
    public bool Visit(Rectangle rectangle) => rectangle.Intersects(this);

    /// <summary>Permet de vérifier si le polygone touche un rectangle</summary>
    public bool Visit(Polygone polygone)
    {
        foreach (Vector2 point in Points)
        {
            if (polygone.Contains(point))
                return true;
        }

        foreach (Vector2 point in polygone.Points)
        {
            if (Contains(point))
                return true;
        }

        return false;
    }

    public Vector2 FixedPoint => Points[0];

    public ICollider MoveBy(Vector2 decalage)
    {
        // Attention modification d'une structure existante
        Vector2[] point = Points;
        for (int i = 0; i < Points.Length; i++)
            point[i] += decalage;

        return this;
    }
}