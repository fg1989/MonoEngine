using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Running;
using System;

namespace TestPerf;

internal static class Program
{
#pragma warning disable S125 // Sections of code should not be commented out
    public static void Main()
    {
        // new BenchmarkRectangleCollision().TestFuncs();
        // TestVectors.TestVect();
        /*BenchmarkRunner.Run<ParallelForeachMethodUpdateSpeed>(DefaultConfig.Instance.WithArtifactsPath(
            "./ParallelForeachMethodUpdateSpeedResult"));*/
        // BenchmarkRunner.Run<BenchmarkRectangleCollision>(DefaultConfig.Instance.WithArtifactsPath("./BenchmarkRectangleCollisionResult"));
        // BenchmarkRunner.Run<ParallelForeachUpdateSpeed>(DefaultConfig.Instance.WithArtifactsPath("./ParallelForeachUpdateSpeedResult"));
        BenchmarkRunner.Run<ParallelForeachMoveObjects>(DefaultConfig.Instance.WithArtifactsPath("./ParallelForeachMoveObjectsResult"));
        Console.ReadLine();
    }
#pragma warning restore S125 // Sections of code should not be commented out
}