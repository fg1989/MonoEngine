
using MonoEngine.Engine.Collider;
using MonoEngine.Engine.Collider.Figure;
using MonoEngine.Engine.MathStuff;
using System;


/// <summary>
/// Foncticonne comme Rectangle mais représente une sphere
/// </summary>
public struct Circle : ICollider, IColliderVisitor
{
    public Vector2 Center { get; set; }
    public float Radius { get; set; }

    public Circle(Vector2 center, float radius)
    {
        Center = center;
        Radius = radius;
    }

    /// <summary>
    /// Permet d'obtenir le diamèter
    /// </summary>
    public float Diameter => Radius * 2;

    /// <summary>
    /// Si le point est contenu dans la sphere
    /// </summary>
    /// <param name="point"></param>
    /// <returns></returns>
    public bool Contains(Vector2 point)
    {
        return Center.GetDistance(point) <= Radius;
    }

    /// <summary>
    /// Si le cercle en param est en collsion avec celui ci
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    

    public bool Accept(IColliderVisitor visitor)
    {
        return visitor.Intersects(this);
    }

    public bool Intersects(ICollider other)
    {
        return other.Intersects(this);
    }

    public bool Intersects(Circle collider)
    {
        throw new NotImplementedException();
    }

    public bool Intersects(Rectangle collider)
    {
        throw new NotImplementedException();
    }

    public bool Intersects(Polygone collider)
    {
        throw new NotImplementedException();
    }
}