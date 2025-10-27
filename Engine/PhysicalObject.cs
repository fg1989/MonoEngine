using Engine.Collider;
using Engine.MathStuff;
using System.Diagnostics;

namespace Engine;

public sealed class PhysicalObject
{
    public ICollider Collison { get; set; }

    internal Vector2 FixedPoint => Collison.FixedPoint;

    /// <summary>Mass de l'objet</summary>
    public float Mass { get; }

    /// <summary>Vitesse vectorielle de l'objet</summary>
    public Vector2 Velocity { get; internal set; } = new();

    internal Vector2 TempVelocityCorrection { get; set; } = new();

    /// <summary>Accéleration de l'objet</summary>
    public Vector2 Acceleration { get; set; } = new();

    /// <summary>Force de l'objet (Acceleration * mass)</summary>
    public Vector2 Force => Acceleration * Mass;

    /// <summary>Create a <see cref="PhysicalObject"/></summary>
    public PhysicalObject(ICollider collision, float mass = 1)
    {
        Debug.Assert(mass > 0);
        Mass = mass;
        Collison = collision;
        AddContinusForce(new Vector2(0, Constants.Gravity * mass));
    }

    /// <summary>Permet d'appliquer une force à cet objet</summary>
    public void AddContinusForce(Vector2 newForce) => Acceleration += newForce / Mass;

    public void AddPointForce(Vector2 newForce) => Velocity += newForce / Mass;

    private Vector2 GetAirFriction(float deltaTime) => Velocity * -Constants.AirFriction * deltaTime / Mass;

    /// <summary>Acctualise la vitesse de l'objet</summary>
    internal void UpdateSpeed(float deltaTime)
    {
        Velocity += Acceleration * deltaTime;
        Velocity += GetAirFriction(deltaTime);
    }

    public void MoveBy(Vector2 decalage) => Collison = Collison.MoveBy(decalage);
}