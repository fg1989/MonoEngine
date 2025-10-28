using Common;
using Common.Figure;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using MonoRectangle = Microsoft.Xna.Framework.Rectangle;
using MonoVector2 = Microsoft.Xna.Framework.Vector2;
using Rectangle = Common.Figure.Rectangle;
using Vector2 = Common.Vector2;

namespace MonoRenderer;

internal sealed class RenderEngine : IDisposable
{
    internal RenderEngine(Game game)
    {
        _graphics = new GraphicsDeviceManager(game);
        game.Content.RootDirectory = "Content";
        _graphics.PreferredBackBufferWidth = 800;
        _graphics.PreferredBackBufferHeight = 500;
        game.IsMouseVisible = true;

        // Enlever ces deux lignes pour fixer le framerate
        game.IsFixedTimeStep = false;
        _graphics.SynchronizeWithVerticalRetrace = false;
    }

    internal void Initialise(GraphicsDevice device)
    {
        edgeTexture = CreateCircleTexture(_graphics.GraphicsDevice, 128);
        whiteRect = new Texture2D(device, 1, 1);
        Color[] colorData = [Color.White];
        whiteRect.SetData(colorData);
    }

    internal void LoadContent(Game game)
    {
        _spriteBatch = new SpriteBatch(game.GraphicsDevice);
        font1 = game.Content.Load<SpriteFont>("MonoSpaceFont");
        fps = new();
    }

    internal void Draw<TLink, TCircle, TRectangle, TPoly>(Scene<TLink, TCircle, TRectangle, TPoly> scene, GraphicsDevice device)
        where TLink : ILink
        where TCircle : IPhysicObject<Circle>
        where TRectangle : IPhysicObject<Rectangle>
        where TPoly : IPhysicObject<Polygone>
    {
        device.Clear(Color.CornflowerBlue);

        _spriteBatch.Begin();

        foreach (Rectangle item in scene.FixedRectangles)
            DrawRectangle(item, Color.Blue);

        foreach (TLink item in scene.Links)
            DrawBridge(item.StartPoint, item.EndPoint, 5, whiteRect, Color.LightBlue);

        foreach (TRectangle item in scene.Rectangles)
            DrawRectangle(item.Figure, Color.GreenYellow);

        foreach (TCircle item in scene.Circles)
            DrawCircle(item.Figure);

        foreach (TPoly item in scene.Polygones)
            DrawPolygone(item.Figure);

        foreach (VisualText item in scene.Texts)
            _spriteBatch.DrawString(font1, item.Text, item.Position.ToMono(), Color.Black);

        fps.Draw(_spriteBatch, font1);

        _spriteBatch.End();
    }

    public void DrawCircle(Circle circle)
    {
        _spriteBatch.Draw(
            edgeTexture,
            new MonoRectangle(
                (int)circle.Center.X - (int)circle.Radius,
                (int)circle.Center.Y - (int)circle.Radius,
                (int)circle.Diameter,
                (int)circle.Diameter),
            Color.Red);
    }

    private void DrawPolygone(Polygone polygone)
    {
        const int SIDE_WIDTH = 10;
        Vector2[] points = polygone.Points;
        for (int i = 0; i < points.Length; i++)
            DrawBridge(points[i], points[i < points.Length - 1 ? i + 1 : 0], SIDE_WIDTH, whiteRect, Color.Purple);
    }

    private void DrawRectangle(Rectangle rectangle, Color color)
    {
        _spriteBatch.Draw(
            whiteRect,
            rectangle.ToMono(),
            color);
    }

    private void DrawBridge(Vector2 start, Vector2 end, int width, Texture2D texture, Color color)
    {
        Vector2 vectorBetween = start - end;
        MonoVector2 monoVectorBetween = vectorBetween.ToMono();
        _spriteBatch.Draw(
            texture,
            new MonoRectangle(
                new Point((int)end.X, (int)end.Y),
                new Point((int)monoVectorBetween.Length(), width)),
            null,
            color,
            vectorBetween.AngleRad,
            new MonoVector2(0, 0.5f),
            SpriteEffects.None,
            0);
    }

    /// <summary>Creates a single-color circular texture (for debug)</summary>
    private static Texture2D CreateCircleTexture(GraphicsDevice graphicsDevice, int diameter)
    {
        Texture2D texture = new(graphicsDevice, diameter, diameter);
        Color[] data = new Color[diameter * diameter];

        int radius = diameter / 2;
        MonoVector2 center = new(radius, radius);

        for (int y = 0; y < diameter; y++)
        {
            for (int x = 0; x < diameter; x++)
            {
                MonoVector2 pos = new(x, y);
                float distance = MonoVector2.Distance(pos, center);
                data[(y * diameter) + x] = distance <= radius ? Color.White : Color.Transparent;
            }
        }

        texture.SetData(data);
        return texture;
    }

    private readonly GraphicsDeviceManager _graphics;

    private SpriteBatch _spriteBatch = null!;
    private Texture2D edgeTexture = null!;
    private Texture2D whiteRect = null!;
    private SpriteFont font1 = null!;

    private FPSRenderer fps;

    public void Dispose()
    {
        try
        {
            _graphics.Dispose();
        }
        finally
        {
            try
            {
                whiteRect.Dispose();
            }
            finally
            {
                try
                {
                    edgeTexture.Dispose();
                }
                finally
                {
                    _spriteBatch.Dispose();
                }
            }
        }
    }
}