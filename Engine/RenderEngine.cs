using Engine.Collider;
using Engine.Collider.Figure;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using MonoRectangle = Microsoft.Xna.Framework.Rectangle;
using MonoVector2 = Microsoft.Xna.Framework.Vector2;
using Rectangle = Engine.Collider.Figure.Rectangle;
using Vector2 = Engine.MathStuff.Vector2;

namespace Engine;

internal sealed class RenderEngine : IDisposable, IColliderVisitor
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

    internal void Draw(Scene scene, GraphicsDevice device)
    {
        device.Clear(Color.CornflowerBlue);

        _spriteBatch.Begin();

        foreach (Rectangle item in scene.FixedRectangles)
            DrawRectangle(item, Color.Blue);

        foreach (RigidLink item in scene.Links)
            DrawBridge(item.StartPoint, item.EndPoint, 5, whiteRect, Color.LightBlue);

        foreach (PhysicalObject item in scene.Objects)
            item.Collison.Accept(this);

        foreach (VisualText item in scene.Texts)
            _spriteBatch.DrawString(font1, item.Text, item.Position, Color.Black);

        fps.Draw(_spriteBatch, font1);

        _spriteBatch.End();
    }

    public void Visit(Circle circle)
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

    public void Visit(Rectangle rectangle) => DrawRectangle(rectangle, Color.GreenYellow);

    public void Visit(Polygone polygone)
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
            rectangle,
            color);
    }

    private void DrawBridge(Vector2 start, Vector2 end, int width, Texture2D texture, Color color)
    {
        Vector2 vectorBetween = start - end;
        MonoVector2 monoVectorBetween = vectorBetween;
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