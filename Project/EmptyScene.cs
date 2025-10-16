using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoEngine;

namespace Project;

internal sealed class EmptyScene : Scene
{
    protected override Scene UpdateScene(GameTime gameTime)
        => Keyboard.GetState().IsKeyDown(Keys.Back) ? new MenuScene() : this;
}