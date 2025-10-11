using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoEngine.Engine;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using _Vector2 = MonoEngine.Engine.MathStuff.Vector2;

namespace MonoEngine
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;


        List<Edge> edges = new List<Edge>();
        Bridge bridge;
        Texture2D EdgeTexture;
        Texture2D WhiteRect;
        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            edges.Add(new Edge(150, 250, 2, 25));
            edges.Add(new Edge(300, 250, 5, 55));

            bridge = new Bridge(edges[0], edges[1],150,100,10f);


            EdgeTexture = CreateCircleTexture(_graphics.GraphicsDevice, 128);
            WhiteRect = new Texture2D(GraphicsDevice, 1, 1);
            Color[] colorData = { Color.White };
            WhiteRect.SetData(colorData);
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            // TODO: use this.Content to load your game content here
        }

        List<Func<bool>> remove = new List<Func<bool>>();
        protected override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (Keyboard.GetState().IsKeyDown(Keys.D))
            {
                remove.Add(edges[0].ApplyForce(new _Vector2(8, 0)));
                
            }
             if (Keyboard.GetState().IsKeyDown(Keys.A))
            {
                remove.Add( edges[0].ApplyForce(new _Vector2(-8, 0)));
                
            }

            if (Keyboard.GetState().IsKeyDown(Keys.W))
            {
                remove.Add( edges[0].ApplyForce(new _Vector2(0, -8)));

            }
            if (Keyboard.GetState().IsKeyDown(Keys.S))
            {
                remove.Add( edges[0].ApplyForce(new _Vector2(0, 8)));

            }

            if (Keyboard.GetState().IsKeyDown(Keys.Space))
            {
                while (remove.Count > 0)
                {
                    remove.Take(1).ToArray()[0]();
                    remove.RemoveAt(0);
                }
            }


            if (Keyboard.GetState().IsKeyDown(Keys.P))
            {
                remove.Add(edges[0].ApplyForce(new _Vector2(-25, 0)));
                remove.Add(edges[1].ApplyForce(new _Vector2(25, 0)));
            }



            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            bridge.Update(deltaTime);

            foreach (var item in edges)
            {
                item.Update(deltaTime);
            }


            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();
            DrawBridge(bridge, WhiteRect);
            foreach (var item in edges)
            {
                DrawEdge(item, EdgeTexture);
            }
            _spriteBatch.End();
            base.Draw(gameTime);
        }

        private void DrawBridge(Bridge bridge, Texture2D whiteRect)
        {
            const int sideWidth = 16;
            _Vector2 vectorBetween = bridge.Edges[0].Position - bridge.Edges[1].Position;
            Vector2 monoVectorBetween = vectorBetween;
            _spriteBatch.Draw(
                WhiteRect,
                new Rectangle(
                    new Point ((int)bridge.Edges[vectorBetween.X < 0 ? 0 : 1].Position.X, (int)bridge.Edges[vectorBetween.X < 0 ? 0 : 1].Position.Y),
                    new Point((int)monoVectorBetween.Length(), sideWidth)),
                null, 
                Color.Gray,
                vectorBetween.AngleRad ,
                new Vector2(0, 0.5f),
                SpriteEffects.None, 0);
        }

        private void DrawEdge(Edge e, Texture2D t)
        {
            _spriteBatch.Draw(t,
                new Rectangle((int)(e.Position.X- e.Radius/2), (int)(e.Position.Y - e.Radius / 2), (int)e.Radius, (int)e.Radius),
                Color.Red);
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
                        data[y * diameter + x] = Color.White;
                    else
                        data[y * diameter + x] = Color.Transparent;
                }
            }

            texture.SetData(data);
            return texture;
        }

        private Vector2 ToMonoVector(_Vector2 v)
        {
            return new Vector2(v.X, v.Y);
        }
    }
}
