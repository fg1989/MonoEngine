using MonoEngine.Engine.MathStuff;
using System.Diagnostics;

namespace MonoEngine.Engine;

/// <summary>Permet de géré un lien rigide entre deux objets phisiques</summary>
public sealed class RigidLink
{
    public PhysicalObject StartObject { get; }

    public PhysicalObject EndObject { get; }

    public Vector2 StartPoint => StartObject.FixedPoint + field;

    public Vector2 EndPoint => EndObject.FixedPoint + field;

    /// <summary>Longueur du lien</summary>
    public float Length { get; }

    public RigidLink(PhysicalObject start, Vector2 startDecal, PhysicalObject end, Vector2 endDecal)
    {
        Debug.Assert(start != end);

        StartObject = start;
        EndObject = end;
        StartPoint = startDecal;
        EndPoint = endDecal;

        Length = (start.FixedPoint + startDecal).GetDistance(end.FixedPoint + endDecal);
    }
}