using Microsoft.Xna.Framework;
using MonoEngine.Engine;
using System.Collections.Generic;
using Rectangle = MonoEngine.Engine.Collider.Figure.Rectangle;

namespace MonoEngine;

public class Scene
{
    protected internal List<PhysicalObject> Objects { get; } = [];

    protected internal List<Rectangle> FixedRectangles { get; } = [];

    protected internal List<VisualText> Texts { get; } = [];

    protected internal List<RigidLink> Links { get; } = [];

    protected internal virtual Scene UpdateScene(GameTime gameTime) => this;
}