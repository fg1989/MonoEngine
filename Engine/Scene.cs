using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Rectangle = Engine.Collider.Figure.Rectangle;

namespace Engine;

public class Scene
{
    protected internal List<PhysicalObject> Objects { get; } = [];

    protected internal List<Rectangle> FixedRectangles { get; } = [];

    protected internal List<VisualText> Texts { get; } = [];

    protected internal List<RigidLink> Links { get; } = [];

    protected internal virtual Scene UpdateScene(GameTime gameTime) => this;
}