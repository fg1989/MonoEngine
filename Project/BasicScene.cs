using Common.Figure;
using Engine;
using Microsoft.Xna.Framework.Input;
using System.Globalization;
using Rectangle = Common.Figure.Rectangle;
using Vector2 = Common.Vector2;

namespace Project;

internal sealed class BasicScene : PhysicalScene
{
    internal BasicScene()
    {
        PhysicalObject<Circle> supportCircle = new(new Circle(new(200, 50), 5), 5);
        Circles.Add(supportCircle);
        supportCircle.AddContinusForce(new Vector2(0, -5 * Constants.Gravity));

        PhysicalObject<Rectangle> r1 = new(new Rectangle(new(100, 50), new(80, 50)), 5);
        Rectangles.Add(new(new Rectangle(new(100, 250), new(80, 50)), 5));
        Rectangles.Add(r1);
        FixedRectangles.Add(new(new(0, 400), new(800, 20)));
        Links.Add(new(supportCircle, new(), r1, new()));

        Rectangle r = new(new(100, 50), new(80, 50));
        r1 = new(r, 5);
        Rectangles.Add(new(new Rectangle(new(100, 250), new(80, 50)), 5));
        Rectangles.Add(r1);
        FixedRectangles.Add(new(new(0, 400), new(800, 20)));
        Links.Add(new RigidLink(supportCircle, new(), r1, r.Size / 2));

        poly = new(new Polygone(new(500, 300), 55, 5));
        Polygones.Add(poly);

        linkTop = new(new Circle(new(350, 50), 25), 5);
        Circles.Add(linkTop);
        PhysicalObject<Circle> pc2 = new(new Circle(new(350, 250), 25), 5);
        Circles.Add(pc2);
        Links.Add(new RigidLink(linkTop, new(), pc2, new()));
        FixedRectangles.Add(new(new(300, 100), new(100, 50)));

        PhysicalObject<Circle> r11 = new(new Circle(new(450, 50), 25), 5);
        pc2 = new(new Circle(new(650, 50), 25), 5);
        Circles.Add(r11);
        Circles.Add(pc2);
        Links.Add(new RigidLink(r11, new(), pc2, new()));
        FixedRectangles.Add(new(new(400, 100), new(200, 50)));

        movingCircle = new(new Circle(new(200, 50), 55), 5);
        Circles.Add(movingCircle);
    }

    public override PhysicalScene UpdatePhysicalScene(float deltaTime)
    {
        if (Keyboard.GetState().IsKeyDown(Keys.Q))
            linkTop.Figure = new Circle(new(350, 40), 25);

        if (Keyboard.GetState().IsKeyDown(Keys.D))
            movingCircle.AddPointForce(new(800 * deltaTime, 0));

        if (Keyboard.GetState().IsKeyDown(Keys.A))
            movingCircle.AddPointForce(new(-800 * deltaTime, 0));

        if (Keyboard.GetState().IsKeyDown(Keys.W))
            movingCircle.AddPointForce(new(0, -800 * deltaTime));

        if (Keyboard.GetState().IsKeyDown(Keys.S))
            movingCircle.AddPointForce(new(0, 800 * deltaTime));

        if (Keyboard.GetState().IsKeyDown(Keys.Space))
        {
            movingCircle.AddContinusForce(-movingCircle.Force);
            movingCircle.AddPointForce(-movingCircle.Velocity * movingCircle.Mass);
        }

        MouseState mouse = Mouse.GetState();
        bool currClic = mouse.LeftButton == ButtonState.Pressed;

        if (currClic && !clic)
        {
            Vector2 pos = new(mouse.X, mouse.Y);
            Texts.Clear();
            cnt++;
            if (poly.Figure.Contains(pos))
                Texts.Add(new VisualText(new Vector2(0, 20), $"{cnt.ToString(CultureInfo.InvariantCulture)}: In"));
            else
                Texts.Add(new VisualText(new Vector2(0, 20), $"{cnt.ToString(CultureInfo.InvariantCulture)}: Out"));
        }

        clic = currClic;

        return Keyboard.GetState().IsKeyDown(Keys.Back) ? new MenuScene() : this;
    }

    private readonly PhysicalObject<Polygone> poly;
    private readonly PhysicalObject<Circle> movingCircle;
    private readonly PhysicalObject<Circle> linkTop;
    private bool clic = true;
    private int cnt;
}