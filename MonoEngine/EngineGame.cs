using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace MonoEngine;

public sealed class EngineGame : Game
{
    private readonly RenderEngine renderer;

    public Scene Scene { get; set; }

    public EngineGame(Scene scene)
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

        Scene = Scene.UpdateScene(gameTime);

        float deltaTime = gameTime.DeltaTime;
        PhysicEngine.Update(Scene, deltaTime);
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