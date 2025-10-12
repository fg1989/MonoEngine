using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoEngine.Engine;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using MonoVector2 = Microsoft.Xna.Framework.Vector2;
using Vector2 = MonoEngine.Engine.MathStuff.Vector2;

namespace MonoEngine
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;


        List<PhysicCircle> edges = new List<PhysicCircle>();
        RigidLink bridge;
        Texture2D EdgeTexture;
        Texture2D WhiteRect;
        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            IsFixedTimeStep = false;
        }

        protected override void Initialize()
        {
            edges.Add(new PhysicCircle(150, 250, 2, 25));
            edges.Add(new PhysicCircle(300, 250, 5, 55));

            bridge = new RigidLink(edges[0], edges[1],150);

            EdgeTexture = CreateCircleTexture(_graphics.GraphicsDevice, 128);
            WhiteRect = new Texture2D(GraphicsDevice, 1, 1);
            Color[] colorData = { Color.White };
            WhiteRect.SetData(colorData);
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
        }

        protected override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (Keyboard.GetState().IsKeyDown(Keys.D))
            {
                edges[0].ApplyForce(new Vector2(8, 0));
                
            }
             if (Keyboard.GetState().IsKeyDown(Keys.A))
            {
                edges[0].ApplyForce(new Vector2(-8, 0));
                
            }

            if (Keyboard.GetState().IsKeyDown(Keys.W))
            {
                edges[0].ApplyForce(new Vector2(0, -8));

            }
            if (Keyboard.GetState().IsKeyDown(Keys.S))
            {
                edges[0].ApplyForce(new Vector2(0, 8));

            }

            if (Keyboard.GetState().IsKeyDown(Keys.Space))
            {
                edges[0].ApplyForce(-edges[0].Force);
            }


            if (Keyboard.GetState().IsKeyDown(Keys.P))
            {
                edges[0].ApplyForce(new Vector2(-25, 0));
                edges[1].ApplyForce(new Vector2(25, 0));
            }



            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            

            foreach (var item in edges)
            {
                item.Update(deltaTime);
            }


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

        private void DrawBridge(RigidLink bridge, Texture2D whiteRect)
        {
            const int sideWidth = 16;
            Vector2 vectorBetween = bridge.Edges[0].Position - bridge.Edges[1].Position;
            MonoVector2 monoVectorBetween = vectorBetween;
            _spriteBatch.Draw(
                WhiteRect,
                new Rectangle(
                    new Point ((int)bridge.Edges[1].Position.X, (int)bridge.Edges[1].Position.Y),
                    new Point((int)monoVectorBetween.Length(), sideWidth)),
                null, 
                Color.Gray,
                vectorBetween.AngleRad ,
                new MonoVector2(0, 0.5f),
                SpriteEffects.None, 0);
        }

        private void DrawEdge(PhysicCircle e, Texture2D t)
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
            MonoVector2 center = new MonoVector2(radius, radius);

            for (int y = 0; y < diameter; y++)
            {
                for (int x = 0; x < diameter; x++)
                {
                    MonoVector2 pos = new MonoVector2(x, y);
                    float distance = MonoVector2.Distance(pos, center);

                    if (distance <= radius)
                        data[y * diameter + x] = Color.White;
                    else
                        data[y * diameter + x] = Color.Transparent;
                }
            }

            texture.SetData(data);
            return texture;
        }
    }
}
