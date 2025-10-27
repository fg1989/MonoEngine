using Common;

namespace Engine;

public class PhysicalScene : Scene<RigidLink, PhysicalObject>
{
    public sealed override PhysicalScene UpdateScene(float deltaTime)
    {
        PhysicalScene result = UpdatePhysicalScene(deltaTime);
        PhysicEngine.Update(result, deltaTime);
        return result;
    }

    public virtual PhysicalScene UpdatePhysicalScene(float deltaTime) => this;
}