using Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Rectangle = Engine.Collider.Figure.Rectangle;
using Vector2 = Engine.MathStuff.Vector2;

namespace Project;

internal sealed class MenuScene : Scene
{
    private readonly Rectangle empty;
    private readonly Rectangle basic;
    private readonly Rectangle heavy;
    private readonly Rectangle veryHeavy;

    internal MenuScene()
    {
        empty = new Rectangle(new Vector2(200, 10), new Vector2(400, 60));
        basic = new Rectangle(new Vector2(200, 150), new Vector2(400, 60));
        heavy = new Rectangle(new Vector2(200, 290), new Vector2(400, 60));
        veryHeavy = new Rectangle(new Vector2(200, 430), new Vector2(400, 60));
        FixedRectangles.Add(empty);
        FixedRectangles.Add(basic);
        FixedRectangles.Add(heavy);
        FixedRectangles.Add(veryHeavy);
        Texts.Add(new VisualText(new Vector2(200, 10), "Empty"));
        Texts.Add(new VisualText(new Vector2(200, 150), "Basic"));
        Texts.Add(new VisualText(new Vector2(200, 290), "Heavy"));
        Texts.Add(new VisualText(new Vector2(200, 430), "Very Heavy"));
    }

    protected override Scene UpdateScene(GameTime gameTime)
    {
        MouseState mouse = Mouse.GetState();
        bool currClic = mouse.LeftButton == ButtonState.Pressed;

        if (currClic && !clic)
        {
            Vector2 pos = new(mouse.X, mouse.Y);

            if (empty.Contains(pos))
                return new EmptyScene();

            if (basic.Contains(pos))
                return new BasicScene();

            if (heavy.Contains(pos))
                return new HeavyScene(20);

            if (veryHeavy.Contains(pos))
                return new HeavyScene(5);
        }

        clic = currClic;
        return this;
    }

    private bool clic = true;
}