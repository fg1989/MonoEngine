namespace Common.Figure;

public interface IFigureVisitor<out T>
{
    T Visit(Circle circle);

    T Visit(Rectangle rectangle);

    T Visit(Polygone polygone);
}

public interface IFigureVisitor
{
    void Visit(Circle circle);

    void Visit(Rectangle rectangle);

    void Visit(Polygone polygone);
}