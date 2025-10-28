using Common.Figure;
using System.Collections.Generic;

namespace Common;

public interface ILink
{
    Vector2 StartPoint { get; }

    Vector2 EndPoint { get; }
}

public interface IPhysicObject<out TFigure>
{
    TFigure Figure { get; }
}

public abstract class Scene<TLink, TCircle, TRectangle, TPoly>
    where TLink : ILink
    where TCircle : IPhysicObject<Circle>
    where TRectangle : IPhysicObject<Rectangle>
    where TPoly : IPhysicObject<Polygone>
{
    public virtual Scene<TLink, TCircle, TRectangle, TPoly> UpdateScene(float deltaTime) => this;

    public List<Rectangle> FixedRectangles { get; } = [];

    public List<VisualText> Texts { get; } = [];

    public List<TLink> Links { get; } = [];

    public List<TCircle> Circles { get; } = [];

    public List<TRectangle> Rectangles { get; } = [];

    public List<TPoly> Polygones { get; } = [];
}