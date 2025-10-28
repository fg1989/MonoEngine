using Common;
using Common.Figure;

namespace Engine;

public class PhysicalScene : Scene<RigidLink, PhysicalObject<Circle>, PhysicalObject<Rectangle>, PhysicalObject<Polygone>>
{
    public sealed override PhysicalScene UpdateScene(float deltaTime)
    {
        PhysicalScene result = UpdatePhysicalScene(deltaTime);
        PhysicEngine.Update(result, deltaTime);
        return result;
    }

    public virtual PhysicalScene UpdatePhysicalScene(float deltaTime) => this;
}