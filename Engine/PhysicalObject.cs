using Common;
using Common.Figure;
using System.Diagnostics;

namespace Engine;

public abstract class PhysicalObject
{
    private protected PhysicalObject(float mass)
    {
        Debug.Assert(mass > 0);
        Mass = mass;
        AddContinusForce(new Vector2(0, Constants.Gravity * mass));
    }

    /// <summary>Mass de l'objet</summary>
    public float Mass { get; }

    /// <summary>Vitesse vectorielle de l'objet</summary>
    public Vector2 Velocity { get; internal set; } = new();

    internal Vector2 TempVelocityCorrection { get; set; } = new();

    /// <summary>Accéleration de l'objet</summary>
    public Vector2 Acceleration { get; set; } = new();

    /// <summary>Force de l'objet (Acceleration * mass)</summary>
    public Vector2 Force => Acceleration * Mass;

    /// <summary>Permet d'appliquer une force à cet objet</summary>
    public void AddContinusForce(Vector2 newForce) => Acceleration += newForce / Mass;

    public void AddPointForce(Vector2 newForce) => Velocity += newForce / Mass;

    /// <summary>Acctualise la vitesse de l'objet</summary>
    internal void UpdateSpeed(float deltaTime)
    {
        Velocity += Acceleration * deltaTime;
        Velocity -= Velocity * Constants.AirFriction * deltaTime / Mass;
    }

    internal abstract Vector2 FixedPoint { get; }
}

/// <summary>Create a <see cref="PhysicalObject{TFigure}"/></summary>
public sealed class PhysicalObject<TFigure>(TFigure collision, float mass = 1)
    : PhysicalObject(mass), IPhysicObject<TFigure> where TFigure : IFigure<TFigure>
{
    public TFigure Figure { get; set; } = collision;

    internal override Vector2 FixedPoint => Figure.FixedPoint;

    public void MoveBy(Vector2 decalage) => Figure = Figure.MoveBy(decalage);
}