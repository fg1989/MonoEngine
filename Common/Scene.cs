using Common.Figure;
using System.Collections.Generic;

namespace Common;

public interface ILink
{
    Vector2 StartPoint { get; }

    Vector2 EndPoint { get; }
}

public interface IPhysicObject
{
    IFigure Collison { get; }
}

public abstract class Scene<TLink, TPhysicObject> where TLink : ILink where TPhysicObject : IPhysicObject
{
    public virtual Scene<TLink, TPhysicObject> UpdateScene(float deltaTime) => this;

    public List<Rectangle> FixedRectangles { get; } = [];

    public List<VisualText> Texts { get; } = [];

    public List<TLink> Links { get; } = [];

    public List<TPhysicObject> Objects { get; } = [];
}