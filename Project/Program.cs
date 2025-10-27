using Engine;

namespace Project;

internal static class Program
{
    private static void Main()
    {
        using EngineGame game = new(new MenuScene());
        game.Run();
    }
}