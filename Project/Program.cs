using Engine;
using MonoRenderer;

namespace Project;

internal static class Program
{
    private static void Main()
    {
        using EngineGame<RigidLink, PhysicalObject> game = new(new MenuScene());
        game.Run();
    }
}