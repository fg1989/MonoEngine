using Engine.MathStuff;

namespace Engine.Collider;

public interface ICollider : IColliderVisitor<bool>
{
    Vector2 FixedPoint { get; }

    bool Contains(Vector2 point);

    T Accept<T>(IColliderVisitor<T> visitor);

    void Accept(IColliderVisitor visitor);

    ICollider MoveBy(Vector2 decalage);
}

internal static class ColliderHelper
{
    extension(ICollider collider)
    {
        /// <summary>Permet de vérifier si 2 ICollider sont en collision</summary>
        internal bool Intersects(ICollider other) => other.Accept(collider);
    }
}