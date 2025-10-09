using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoEngine.Engine;
using System;
using _Vector2 = MonoEngine.Engine.MathStuff.Vector2;

namespace MonoEngine
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;


        Edge Edge;
        Texture2D EdgeTexture;
        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            Edge = new Edge(50, 50, 2);
            EdgeTexture = CreateCircleTexture(_graphics.GraphicsDevice, 60);
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            // TODO: use this.Content to load your game content here
        }

        Func<bool> remove;
        protected override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (Keyboard.GetState().IsKeyDown(Keys.D))
            {
                remove = Edge.ApplyForce(new _Vector2(8, 0));
                
            }
             if (Keyboard.GetState().IsKeyDown(Keys.A))
            {
                remove = Edge.ApplyForce(new _Vector2(-8, 0));
                
            }

            if (Keyboard.GetState().IsKeyDown(Keys.W))
            {
                remove = Edge.ApplyForce(new _Vector2(0, -8));

            }
            if (Keyboard.GetState().IsKeyDown(Keys.S))
            {
                remove = Edge.ApplyForce(new _Vector2(0, 8));

            }
            Edge.Update((float)gameTime.ElapsedGameTime.TotalSeconds);
            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();
            DrawEdge(Edge, EdgeTexture);
            _spriteBatch.End();
            base.Draw(gameTime);
        }

        private void DrawEdge(Edge e, Texture2D t)
        {
            _spriteBatch.Draw(t,
                new Rectangle((int)e.Position.X, (int)e.Position.Y, (int)e.Mass * 10, (int)e.Mass * 10), Color.White);
        }

        /// <summary>
        /// créer une texture ciruclaire unicolor (pour débuging)
        /// </summary>
        /// <param name="graphicsDevice"></param>
        /// <param name="diameter"></param>
        /// <param name="color"></param>
        /// <returns></returns>
        public static Texture2D CreateCircleTexture(GraphicsDevice graphicsDevice, int diameter)
        {
            Texture2D texture = new Texture2D(graphicsDevice, diameter, diameter);
            Color[] data = new Color[diameter * diameter];

            int radius = diameter / 2;
            Vector2 center = new Vector2(radius, radius);

            for (int y = 0; y < diameter; y++)
            {
                for (int x = 0; x < diameter; x++)
                {
                    Vector2 pos = new Vector2(x, y);
                    float distance = Vector2.Distance(pos, center);

                    if (distance <= radius)
                        data[y * diameter + x] = Color.Red;
                    else
                        data[y * diameter + x] = Color.Transparent;
                }
            }

            texture.SetData(data);
            return texture;
        }
    }
}
