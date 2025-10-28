using Common.Figure;
using Engine;
using MonoRenderer;

namespace Project;

internal static class Program
{
    private static void Main()
    {
        using EngineGame<RigidLink, PhysicalObject<Circle>, PhysicalObject<Rectangle>, PhysicalObject<Polygone>>
            game = new(new MenuScene());
        game.Run();
    }
}