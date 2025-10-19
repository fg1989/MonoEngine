using MonoEngine.Engine.Collider;
using MonoEngine.Engine.Collider.Figure;
using MonoEngine.Engine.MathStuff;
using System;
using System.Diagnostics;


/// <summary>
/// Représente un cercle
/// </summary>
public struct Circle : ICollider, IColliderVisitor
{
    private Vector2 center;
    public Vector2 Center
    {
        get => center;
    }

    private float radius;
    public float Radius { get => radius; }

    public Circle(Vector2 center, float radius)
    {
        this.center = center;
        this.radius = radius;
    }

    /// <summary>
    /// Permet d'obtenir le diamètre
    /// </summary>
    public float Diameter => Radius * 2;

    /// <summary>
    /// Vérifie si le point est contenu dans le cercle
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
        return other.Accept(this);
    }

    public bool Intersects(Segment segment)
    {
        return segment.Intersects(this);
    }

    /// <summary>
    /// Permet de vérifier si le cercle est en collision avec un polygone
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public bool Intersects(Circle other)
    {
        float distance = other.Center.GetDistance(this.Center);

        return distance <= Radius;
    }

    /// <summary>
    /// Permet de vérifier si le cercle est en collision avec un Rectangle
    /// </summary>
    /// <param name="rectangle"></param>
    /// <returns></returns>
    public bool Intersects(Rectangle rectangle)
    {
        return rectangle.Intersects(this);
    }

    /// <summary>
    /// Permet de vérifier si le cercle est en collision avec un polygone
    /// </summary>
    /// <param name="polygone"></param>
    /// <returns></returns>
    public bool Intersects(Polygone polygone)
    {
        return polygone.Intersects(this);
    }

}