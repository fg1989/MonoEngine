using BenchmarkDotNet.Attributes;
using Common;
using Common.Figure;
using Engine;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TestPerf;

public class ParallelForeachMoveObjects
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

        for (int i = 0; i < M; i++)
            rectangles.Add(new Rectangle(new(r.NextSingle(), r.NextSingle()), new(r.NextSingle(), r.NextSingle())));
    }

    [Benchmark(Baseline = true)]
    public void Normal()
    {
        for (float i = 0; i < 10; ++i)
        {
            foreach (PhysicalObject<Circle> item in circles)
                MoveObject(rectangles, item, i);
        }
    }

    [Benchmark]
    public void Multithread()
    {
        for (float i = 0; i < 10; ++i)
            Parallel.ForEach(circles, x => MoveObject(rectangles, x, i));
    }

    private static void MoveObject<T>(List<Rectangle> rectangles, PhysicalObject<T> item, float deltaTime) where T : IFigure<T>
    {
        Vector2 decal = (item.Velocity * deltaTime) + item.TempVelocityCorrection;
        item.TempVelocityCorrection = new();

        foreach (Rectangle subItem in rectangles)
        {
            if (item.Figure.Collide(subItem))
            {
                item.Velocity = new Vector2(0, 0);
                return;
            }
        }

        if (decal != Vector2.Null)
            item.MoveBy(decal);
    }

    private readonly List<PhysicalObject<Circle>> circles = [];
    private readonly List<Rectangle> rectangles = [];

    [Params(10, 100, 1000, 10000)]
    public int N { get; set; }

    [Params(10, 100, 1000, 10000)]
    public int M { get; set; }
}