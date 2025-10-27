using Engine;
using Microsoft.Xna.Framework.Input;

namespace Project;

internal sealed class EmptyScene : PhysicalScene
{
    public override PhysicalScene UpdatePhysicalScene(float deltaTime)
        => Keyboard.GetState().IsKeyDown(Keys.Back) ? new MenuScene() : this;
}