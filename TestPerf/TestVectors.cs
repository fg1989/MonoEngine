using Engine.MathStuff;
using System;

namespace TestPerf;

internal static class TestVectors
{
    internal static void TestVect()
    {
        for (float i = 0; i < 1f; i += 0.1f)
        {
            Vector2 vec = new(1, i);
            Console.WriteLine($"{vec} : {vec.AngleRad}");
        }

        for (float i = 1f; i > 0f; i -= 0.1f)
        {
            Vector2 vec = new(i, 1);
            Console.WriteLine($"{vec} : {vec.AngleRad}");
        }

        for (float i = 0f; i > -1f; i -= 0.1f)
        {
            Vector2 vec = new(i, 1);
            Console.WriteLine($"{vec} : {vec.AngleRad}");
        }

        for (float i = 1f; i > 0; i -= 0.1f)
        {
            Vector2 vec = new(-1, i);
            Console.WriteLine($"{vec} : {vec.AngleRad}");
        }

        for (float i = 0f; i > -1f; i -= 0.1f)
        {
            Vector2 vec = new(-1, i);
            Console.WriteLine($"{vec} : {vec.AngleRad}");
        }

        for (float i = -1f; i < 0f; i += 0.1f)
        {
            Vector2 vec = new(i, -1);
            Console.WriteLine($"{vec} : {vec.AngleRad}");
        }

        for (float i = 0f; i < 1f; i += 0.1f)
        {
            Vector2 vec = new(i, -1);
            Console.WriteLine($"{vec} : {vec.AngleRad}");
        }

        for (float i = -1f; i < 0f; i += 0.1f)
        {
            Vector2 vec = new(1, i);
            Console.WriteLine($"{vec} : {vec.AngleRad}");
        }
    }
}