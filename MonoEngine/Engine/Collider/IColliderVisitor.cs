using MonoEngine.Engine.Collider.Figure;

namespace MonoEngine.Engine.Collider;

public interface IColliderVisitor<out T>
{
    T Visit(Circle circle);

    T Visit(Rectangle rectangle);

    T Visit(Polygone polygone);
}

public interface IColliderVisitor
{
    void Visit(Circle circle);

    void Visit(Rectangle rectangle);

    void Visit(Polygone polygone);
}