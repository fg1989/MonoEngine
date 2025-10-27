using Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Project;

internal sealed class EmptyScene : Scene
{
    protected override Scene UpdateScene(GameTime gameTime)
        => Keyboard.GetState().IsKeyDown(Keys.Back) ? new MenuScene() : this;
}