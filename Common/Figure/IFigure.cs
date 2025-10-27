namespace Common.Figure;

public interface IFigure : IFigureVisitor<bool>
{
    Vector2 FixedPoint { get; }

    bool Contains(Vector2 point);

    T Accept<T>(IFigureVisitor<T> visitor);

    void Accept(IFigureVisitor visitor);

    IFigure MoveBy(Vector2 decalage);
}