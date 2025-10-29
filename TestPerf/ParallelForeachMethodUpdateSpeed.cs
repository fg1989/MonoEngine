using BenchmarkDotNet.Attributes;
using Common.Figure;
using Engine;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TestPerf;

public class ParallelForeachMethodUpdateSpeed
{
    [GlobalSetup]
    public void GlobalSetup()
    {
        Random r = new(2835);
        for (int i = 0; i < N; i++)
        {
            PhysicalObject<Circle> c = new(new(new(0, 0), 0), r.NextSingle() * 100);
            c.AddContinusForce(new(r.NextSingle(), r.NextSingle()));
            c.AddPointForce(new(r.NextSingle(), r.NextSingle()));
            circles.Add(c);
        }
    }

    [Benchmark]
    public void Static()
    {
        for (float i = 0; i < 10; ++i)
        {
            dt = i;
            Parallel.ForEach(circles, static x => x.UpdateSpeed(dt));
        }
    }

    private static float dt;

    [Benchmark]
    public void ThreadLocal()
    {
        for (float i = 0; i < 10; ++i)
        {
            Parallel.ForEach(
                circles,
                () => i,
                static (x, _, dt) =>
                {
                    x.UpdateSpeed(dt);
                    return dt;
                },
                static _ => { });
        }
    }

    [Benchmark(Baseline = true)]
    public void Capture()
    {
        for (float i = 0; i < 10; ++i)
            Parallel.ForEach(circles, x => x.UpdateSpeed(i));
    }

    private readonly List<PhysicalObject<Circle>> circles = [];

    private const int N = 800;
}