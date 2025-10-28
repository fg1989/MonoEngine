namespace Common.Figure;

public interface IFigure<out TSelf>
{
    Vector2 FixedPoint { get; }

    TSelf MoveBy(Vector2 decalage);

    bool Collide(Rectangle rectangle);
}