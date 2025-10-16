using BenchmarkDotNet.Running;

namespace TestPerf;

internal static class Program
{
#pragma warning disable S125 // Sections of code should not be commented out
    public static void Main()
         // => TestVectors.TestVect();
         => BenchmarkRunner.Run<BenchmarkRectangleCollision>();
    // => new BenchmarkRectangleCollision().TestFuncs();
#pragma warning restore S125 // Sections of code should not be commented out
}