using Engine;
using Engine.Collider.Figure;
using Engine.MathStuff;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Globalization;
using Rectangle = Engine.Collider.Figure.Rectangle;
using Vector2 = Engine.MathStuff.Vector2;

namespace Project;

internal sealed class BasicScene : Scene
{
    internal BasicScene()
    {
        c = new(new Circle(new(200, 50), 5), 5);
        Objects.Add(c);
        c.AddContinusForce(new Vector2(0, -5 * Constants.Gravity));

        PhysicalObject r1 = new(new Rectangle(new(100, 50), new(80, 50)), 5);
        Objects.Add(new(new Rectangle(new(100, 250), new(80, 50)), 5));
        Objects.Add(r1);
        FixedRectangles.Add(new(new(0, 400), new(800, 20)));
        Links.Add(new(c, new(), r1, new()));

        Rectangle r = new(new(100, 50), new(80, 50));
        r1 = new(r, 5);
        Objects.Add(new(new Rectangle(new(100, 250), new(80, 50)), 5));
        Objects.Add(r1);
        FixedRectangles.Add(new(new(0, 400), new(800, 20)));
        Links.Add(new RigidLink(c, new(), r1, r.Size / 2));

        poly = new(new Polygone(new(500, 300), 55, 5));
        Objects.Add(poly);

        link = new(new Circle(new(350, 50), 25), 5);
        Objects.Add(link);
        PhysicalObject pc2 = new(new Circle(new(350, 250), 25), 5);
        Objects.Add(pc2);
        Links.Add(new RigidLink(link, new(), pc2, new()));
        FixedRectangles.Add(new(new(300, 100), new(100, 50)));

        r1 = new(new Circle(new(450, 50), 25), 5);
        pc2 = new(new Circle(new(650, 50), 25), 5);
        Objects.Add(r1);
        Objects.Add(pc2);
        Links.Add(new RigidLink(r1, new(), pc2, new()));
        FixedRectangles.Add(new(new(400, 100), new(200, 50)));

        c = new(new Circle(new(200, 50), 55), 5);
        Objects.Add(c);
    }

    protected override Scene UpdateScene(GameTime gameTime)
    {
        if (Keyboard.GetState().IsKeyDown(Keys.Q))
            link.Collison = new Circle(new(350, 40), 25);

        if (Keyboard.GetState().IsKeyDown(Keys.D))
            c.AddPointForce(new(800 * gameTime.DeltaTime, 0));

        if (Keyboard.GetState().IsKeyDown(Keys.A))
            c.AddPointForce(new(-800 * gameTime.DeltaTime, 0));

        if (Keyboard.GetState().IsKeyDown(Keys.W))
            c.AddPointForce(new(0, -800 * gameTime.DeltaTime));

        if (Keyboard.GetState().IsKeyDown(Keys.S))
            c.AddPointForce(new(0, 800 * gameTime.DeltaTime));

        if (Keyboard.GetState().IsKeyDown(Keys.Space))
        {
            c.AddContinusForce(-c.Force);
            c.AddPointForce(-c.Velocity * c.Mass);
        }

        MouseState mouse = Mouse.GetState();
        bool currClic = mouse.LeftButton == ButtonState.Pressed;

        if (currClic && !clic)
        {
            Vector2 pos = new(mouse.X, mouse.Y);
            Texts.Clear();
            cnt++;
            if (poly.Collison.Contains(pos))
                Texts.Add(new VisualText(new Vector2(0, 20), $"{cnt.ToString(CultureInfo.InvariantCulture)}: In"));
            else
                Texts.Add(new VisualText(new Vector2(0, 20), $"{cnt.ToString(CultureInfo.InvariantCulture)}: Out"));
        }

        clic = currClic;

        return Keyboard.GetState().IsKeyDown(Keys.Back) ? new MenuScene() : this;
    }

    private readonly PhysicalObject poly;
    private readonly PhysicalObject c;
    private readonly PhysicalObject link;
    private bool clic = true;
    private int cnt;
}