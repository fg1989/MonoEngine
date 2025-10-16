using BenchmarkDotNet.Attributes;
using MonoEngine.Engine.Collider.Figure;
using MonoEngine.Engine.MathStuff;
using System;
using System.Runtime.CompilerServices;

namespace TestPerf;

#pragma warning disable CA1515 // Consider making public types internal
public class BenchmarkRectangleCollision
{
    [GlobalSetup]
    public void GlobalSetup()
    {
        Random r = new(2835);
        circles = new Circle[N];
        rectangles = new Rectangle[N];

        for (int i = 0; i < N; i++)
            circles[i] = new Circle(new Vector2(r.NextSingle() * 100, r.NextSingle() * 100), (r.NextSingle() * 9) + 1);

        for (int i = 0; i < N; i++)
        {
            rectangles[i]
                = new Rectangle(
                    new Vector2(r.NextSingle() * 100, r.NextSingle() * 100),
                    new Vector2((r.NextSingle() * 9) + 1, (r.NextSingle() * 9) + 1));
        }
    }

    internal void TestFuncs()
    {
        GlobalSetup();
        foreach (Circle c in circles)
        {
            foreach (Rectangle r in rectangles)
            {
                bool b = CustomCollide(c, r);
                if (b != BoxCustomCollide(c, r))
                    throw new Exception();

                if (b != StandardCollide(c, r))
                    throw new Exception();
            }
        }
    }

    [Benchmark]
    public bool Custom()
    {
        bool b = false;
        foreach (Circle c in circles)
        {
            foreach (Rectangle r in rectangles)
                b ^= CustomCollide(c, r);
        }
        return b;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static bool CustomCollide(Circle c, Rectangle r)
    {
        return c.Contains(r.Position)
            || c.Contains(new Vector2(r.X + r.Width, r.Y))
            || c.Contains(new Vector2(r.X, r.Y + r.Height))
            || c.Contains(new Vector2(r.X + r.Width, r.Y + r.Height))
            || r.Contains(new Vector2(c.Center.X + c.Radius, c.Center.Y))
            || r.Contains(new Vector2(c.Center.X - c.Radius, c.Center.Y))
            || r.Contains(new Vector2(c.Center.X, c.Center.Y + c.Radius))
            || r.Contains(new Vector2(c.Center.X, c.Center.Y - c.Radius));
    }

    [Benchmark]
    public bool BoxCustom()
    {
        bool b = false;
        foreach (Circle c in circles)
        {
            foreach (Rectangle r in rectangles)
                b ^= BoxCustomCollide(c, r);
        }
        return b;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static bool BoxCustomCollide(Circle c, Rectangle r)
    {
        return r.Visit(new Rectangle(c.Center - new Vector2(c.Radius, c.Radius), new Vector2(c.Diameter, c.Diameter)))
            && (c.Contains(r.Position)
            || c.Contains(new Vector2(r.X + r.Width, r.Y))
            || c.Contains(new Vector2(r.X, r.Y + r.Height))
            || c.Contains(new Vector2(r.X + r.Width, r.Y + r.Height))
            || r.Contains(new Vector2(c.Center.X + c.Radius, c.Center.Y))
            || r.Contains(new Vector2(c.Center.X - c.Radius, c.Center.Y))
            || r.Contains(new Vector2(c.Center.X, c.Center.Y + c.Radius))
            || r.Contains(new Vector2(c.Center.X, c.Center.Y - c.Radius)));
    }

    [Benchmark]
    public bool Standard()
    {
        bool b = false;
        foreach (Circle c in circles)
        {
            foreach (Rectangle r in rectangles)
                b ^= StandardCollide(c, r);
        }
        return b;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static bool StandardCollide(Circle c, Rectangle r)
    {
        return r.Visit(new Rectangle(c.Center - new Vector2(c.Radius, c.Radius), new Vector2(c.Diameter, c.Diameter)))
            && (c.Contains(r.Position)
            || c.Contains(new Vector2(r.X + r.Width, r.Y))
            || c.Contains(new Vector2(r.X, r.Y + r.Height))
            || c.Contains(new Vector2(r.X + r.Width, r.Y + r.Height))
            || r.Contains(c.Center)
            || (c.Center.X >= r.Position.X && c.Center.X <= r.Position.X + r.Size.X)
            || (c.Center.Y >= r.Position.Y && c.Center.Y <= r.Position.Y + r.Size.Y));
    }

    private Circle[] circles = [];
    private Rectangle[] rectangles = [];

    private const int N = 100;
}
#pragma warning restore CA1515 // Consider making public types internal