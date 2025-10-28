using System.Runtime.InteropServices;

namespace Common.Figure;

/// <summary>Représente un cercle</summary>
[StructLayout(LayoutKind.Auto)]
public readonly record struct Circle(Vector2 Center, float Radius) : IFigure<Circle>
{
    /// <summary>Permet d'obtenir le diamètre</summary>
    public float Diameter => Radius * 2;

    /// <summary>Vérifie si le point est contenu dans le cercle</summary>
    public bool Contains(Vector2 point) => Center.GetSquaredDistance(point) <= Radius * Radius;

    /// <summary>Permet de vérifier si le cercle est en collision avec un polygone</summary>
    public bool Collide(Circle circle)
    {
        float distance = circle.Center.GetSquaredDistance(Center);
        float dist = Radius + circle.Radius;
        return distance <= dist * dist;
    }

    /// <summary>Permet de vérifier si le cercle est en collision avec un Rectangle</summary>
    public bool Collide(Rectangle rectangle) => rectangle.Collide(this);

    public Vector2 FixedPoint => Center;

    public Circle MoveBy(Vector2 decalage) => new(Center + decalage, Radius);
}