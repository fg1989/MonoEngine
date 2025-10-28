using Common;
using Common.Figure;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Rectangle = Common.Figure.Rectangle;

namespace MonoRenderer;

public sealed class EngineGame<TLink, TCircle, TRectangle, TPoly> : Game
    where TLink : ILink
    where TCircle : IPhysicObject<Circle>
    where TRectangle : IPhysicObject<Rectangle>
    where TPoly : IPhysicObject<Polygone>
{
    private readonly RenderEngine renderer;

    public Scene<TLink, TCircle, TRectangle, TPoly> Scene { get; set; }

    public EngineGame(Scene<TLink, TCircle, TRectangle, TPoly> scene)
    {
        Scene = scene;
        renderer = new(this);
    }

    protected override void Initialize()
    {
        base.Initialize();
        renderer.Initialise(GraphicsDevice);
    }

    protected override void LoadContent()
    {
        base.LoadContent();
        renderer.LoadContent(this);
    }

    protected override void Update(GameTime gameTime)
    {
        if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        float deltaTime = gameTime.DeltaTime;
        Scene = Scene.UpdateScene(deltaTime);
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        renderer.Draw(Scene, GraphicsDevice);
        base.Draw(gameTime);
    }

    protected override void Dispose(bool disposing)
    {
        renderer.Dispose();
        base.Dispose(disposing);
    }
}

public static class TimeHelper
{
    extension(GameTime gt)
    {
        public float DeltaTime => (float)gt.ElapsedGameTime.TotalSeconds;
    }
}