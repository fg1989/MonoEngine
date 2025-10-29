using BenchmarkDotNet.Attributes;
using Common.Figure;
using Engine;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TestPerf;

public class ParallelForeachUpdateSpeed
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

    [Benchmark(Baseline = true)]
    public void Normal()
    {
        for (float i = 0; i < 10; ++i)
        {
            foreach (PhysicalObject<Circle> item in circles)
                item.UpdateSpeed(i);
        }
    }

    [Benchmark]
    public void Multithread()
    {
        for (float i = 0; i < 10; ++i)
            Parallel.ForEach(circles, x => x.UpdateSpeed(i));
    }

    private readonly List<PhysicalObject<Circle>> circles = [];

    [Params(100, 1000, 10000, 100000)]
    public int N { get; set; }
}