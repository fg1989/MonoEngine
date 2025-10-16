using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Diagnostics;
using System.Globalization;

namespace MonoEngine;

internal struct FPSRenderer
{
    public FPSRenderer()
    {
        sw.Start();
    }

    internal void Draw(SpriteBatch spriteBatch, SpriteFont font1)
    {
        counter++;
        if (counter == nextUpdate || sw.ElapsedMilliseconds > 1000)
        {
            double fps = 1000d * counter / sw.ElapsedMilliseconds;
            drawFPS = $"{RoundFPS(fps)} FPS";
            counter = 0;
            sw.Restart();
            nextUpdate = Math.Max(5, Convert.ToInt32(fps) / 2);
        }

        spriteBatch.DrawString(font1, drawFPS, new Vector2(0, 0), Color.Black);
    }

    private static string RoundFPS(double fps)
    {
        return fps switch
        {
            >= 60 => string.Format(CultureInfo.CurrentCulture, "{0,4:####}", fps),
            >= 10 => string.Format(CultureInfo.CurrentCulture, "{0,4:##.#}", fps),
            >= 1 => string.Format(CultureInfo.CurrentCulture, "{0,4:#.##}", fps),
            _ => string.Format(CultureInfo.CurrentCulture, "{0,4:0.##}", fps),
        };
    }

    private int nextUpdate = 1;
    private int counter;
    private string drawFPS = "";
    private readonly Stopwatch sw = new();
}