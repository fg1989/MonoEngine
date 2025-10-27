using System.Runtime.InteropServices;

namespace Common.Figure;

[StructLayout(LayoutKind.Auto)]
public readonly record struct Rectangle(Vector2 Position, Vector2 Size) : IFigure
{
    public readonly float X => Position.X;

    public readonly float Y => Position.Y;

    public readonly float Width => Size.X;

    public readonly float Height => Size.Y;

    public Rectangle(float x, float y, float width, float height) : this(new Vector2(x, y), new Vector2(width, height))
    {
    }

    /// <summary>Vérifie si le rectangle contient un point donné</summary>
    public readonly bool Contains(Vector2 point)
        => point.X >= X && point.X <= X + Width && point.Y >= Y && point.Y <= Y + Height;

    public T Accept<T>(IFigureVisitor<T> visitor) => visitor.Visit(this);

    public void Accept(IFigureVisitor visitor) => visitor.Visit(this);

    /// <summary>Permet de vérifier si le rectangle est en collision avec un cercle</summary>
    public bool Visit(Circle circle)
    {
        return Visit(
            new Rectangle(
                circle.Center - new Vector2(circle.Radius, circle.Radius),
                new Vector2(circle.Diameter, circle.Diameter)))
            && (circle.Contains(Position)
            || circle.Contains(new Vector2(X + Width, Y))
            || circle.Contains(new Vector2(X, Y + Height))
            || circle.Contains(new Vector2(X + Width, Y + Height))
            || Contains(circle.Center)
            || (circle.Center.X >= Position.X && circle.Center.X <= Position.X + Size.X)
            || (circle.Center.Y >= Position.Y && circle.Center.Y <= Position.Y + Size.Y));
    }

    /// <summary>Permet de vérifier si 2 Rectangles sont en collision</summary>
    public bool Visit(Rectangle rectangle)
        => rectangle.X <= X + Width
        && rectangle.Y <= Y + Height
        && X <= rectangle.X + rectangle.Width
        && Y <= rectangle.Y + rectangle.Height;

    /// <summary>Permet de vérifier si le rectangle est en collision avec un polygone</summary>
    public bool Visit(Polygone polygone)
    {
        if (polygone.Contains(Position)
            || polygone.Contains(new Vector2(X + Width, Y))
            || polygone.Contains(new Vector2(X, Y + Height))
            || polygone.Contains(new Vector2(X + Width, Y + Height)))
        {
            return true;
        }

        foreach (Vector2 item in polygone.Points)
        {
            if (Contains(item))
                return true;
        }

        return false;
    }

    public Vector2 FixedPoint => Position;

    public IFigure MoveBy(Vector2 decalage) => new Rectangle(Position + decalage, Size);
}