using MonoEngine.Engine.MathStuff;
using System.Runtime.InteropServices;

namespace MonoEngine.Engine.Collider.Figure;

/// <summary>Représente un cercle</summary>
[StructLayout(LayoutKind.Auto)]
public readonly record struct Circle(Vector2 Center, float Radius) : ICollider
{
    /// <summary>Permet d'obtenir le diamètre</summary>
    public readonly float Diameter => Radius * 2;

    /// <summary>Vérifie si le point est contenu dans le cercle</summary>
    public readonly bool Contains(Vector2 point) => Center.GetSquaredDistance(point) <= Radius * Radius;

    /// <summary>Si le cercle en param est en collsion avec celui ci</summary>
    readonly T ICollider.Accept<T>(IColliderVisitor<T> visitor) => visitor.Visit(this);

    public void Accept(IColliderVisitor visitor) => visitor.Visit(this);

    /// <summary>Permet de vérifier si le cercle est en collision avec un polygone</summary>
    public bool Visit(Circle circle)
    {
        float distance = circle.Center.GetSquaredDistance(Center);
        float dist = Radius + circle.Radius;
        return distance <= dist * dist;
    }

    /// <summary>Permet de vérifier si le cercle est en collision avec un Rectangle</summary>
    public bool Visit(Rectangle rectangle) => rectangle.Intersects(this);

    /// <summary>Permet de vérifier si le cercle est en collision avec un polygone</summary>
    public bool Visit(Polygone polygone) => polygone.Intersects(this);

    public Vector2 FixedPoint => Center;

    public ICollider MoveBy(Vector2 decalage) => new Circle(Center + decalage, Radius);
}