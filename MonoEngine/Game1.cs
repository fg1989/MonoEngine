using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoEngine.Engine;
using MonoEngine.Engine.Collider;
using MonoEngine.Engine.Collider.Figure;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using MonoRectangle = Microsoft.Xna.Framework.Rectangle;
using MonoVector2 = Microsoft.Xna.Framework.Vector2;
using Rectangle = MonoEngine.Engine.Collider.Figure.Rectangle;
using Vector2 = MonoEngine.Engine.MathStuff.Vector2;

namespace MonoEngine
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;


        List<PhysicalCircle> circles = new List<PhysicalCircle>();
        List<PhysicalRectangle> rectangles = new List<PhysicalRectangle>();
        List<PhysicalPolygone> polygones = new List<PhysicalPolygone>();

        RigidLink bridge;
        Texture2D EdgeTexture;
        Texture2D WhiteRect;
        Rectangle rectangle;
        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            _graphics.PreferredBackBufferWidth = 800;
            _graphics.PreferredBackBufferHeight = 500;
            IsMouseVisible = true;
            IsFixedTimeStep = false;
        }

        protected override void Initialize()
        {

            rectangle = new Rectangle(0,400,800,20);
            circles.Add(new PhysicalCircle(500, 50, 5, 55));
            rectangles.Add(new PhysicalRectangle(100, 250, 80, 50, 5));
            polygones.Add(
                new PhysicalPolygone(
                    new Polygone(
                        new Vector2(500,200),
                        55,5)
                    , 5));
            //bridge = new RigidLink(circles[0], rectangles[0]);


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
                circles[0].ApplyForce(new Vector2(8, 0));
                
            }
             if (Keyboard.GetState().IsKeyDown(Keys.A))
            {
                circles[0].ApplyForce(new Vector2(-8, 0));
                
            }

            if (Keyboard.GetState().IsKeyDown(Keys.W))
            {
                circles[0].ApplyForce(new Vector2(0, -8));

            }
            if (Keyboard.GetState().IsKeyDown(Keys.S))
            {
                circles[0].ApplyForce(new Vector2(0, 8));

            }

            if (Keyboard.GetState().IsKeyDown(Keys.Space))
            {
                circles[0].ApplyForce(-circles[0].Force);
            }


            if (Keyboard.GetState().IsKeyDown(Keys.P))
            {
                circles[0].ApplyForce(new Vector2(-25, 0));
            }

            

            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            var physcalObjects = circles.Cast<PhysicalObject>().Concat(rectangles.Cast<PhysicalObject>()).Concat(polygones.Cast<PhysicalObject>());
            
            foreach (var item in physcalObjects)
            {
                item.Update(deltaTime);
                if (item.Collison.Intersects(rectangle))
                {
                    item.RedirectMovement(deltaTime,new Vector2(0,1));
                }
            }
            if (circles[0].Collison.Intersects(polygones[0].Collison))
            {
                circles[0].RedirectMovement(deltaTime, new Vector2(0, 1));
            }


            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();
            //DrawBridge(bridge, WhiteRect);
            foreach (var item in circles)
            {
                DrawCircle(item.Circle, EdgeTexture);
            }
            foreach (var item in rectangles)
            {
                DrawRectangle(item.Rectangle, WhiteRect);
            }
            foreach (var item in polygones)
            {
                DrawPolygone(item.Polygone, WhiteRect);
            }
            DrawRectangle(rectangle, WhiteRect);
            _spriteBatch.End();
            base.Draw(gameTime);
        }

        private void DrawBridge(RigidLink rigidLink, Texture2D texture)
        {
            const int SIDE_WIDTH = 16;
            DrawBridge(rigidLink.Edges[0].Position, rigidLink.Edges[1].Position, SIDE_WIDTH, texture, Color.Gray);
        }

        private void DrawCircle(Circle circle, Texture2D texture)
        {
            _spriteBatch.Draw(texture,
                new MonoRectangle(
                    (int)(circle.Center.X ),
                    (int)(circle.Center.Y ),
                    (int)circle.Radius,
                    (int)circle.Radius),
                Color.Red);
        }
        private void DrawRectangle(Rectangle rectangle, Texture2D texture)
        {
            _spriteBatch.Draw(texture,
                rectangle,
                Color.Blue);
        }
        private void DrawPolygone(Polygone polygone, Texture2D texture)
        {
            const int SIDE_WIDTH = 10;
            Vector2[] points = polygone.Points;
            for (int i = 0; i < points.Length; i++)
            {
                DrawBridge(points[i], points[i < points.Length-1 ? i + 1 :0 ], SIDE_WIDTH , texture, Color.Purple);
            }
        }
        private void DrawBridge(Vector2 start, Vector2 end, int width, Texture2D texture, Color color)
        {
            Vector2 vectorBetween = start - end ;
            MonoVector2 monoVectorBetween = vectorBetween;
            _spriteBatch.Draw(
                WhiteRect,
                new MonoRectangle(
                    new Point((int)end.X, (int)end.Y),
                    new Point((int)monoVectorBetween.Length(), width)),
                null,
                color,
                vectorBetween.AngleRad,
                new MonoVector2(0, 0.5f),
                SpriteEffects.None, 0);
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
