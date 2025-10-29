using Common.Figure;
using Engine;
using Microsoft.Xna.Framework.Input;
using Rectangle = Common.Figure.Rectangle;
using Vector2 = Common.Vector2;

namespace Project;

internal sealed class MenuScene : PhysicalScene
{
    private readonly Rectangle empty;
    private readonly Rectangle basic;
    private readonly Rectangle normal;
    private readonly Rectangle heavy;
    private readonly Rectangle veryHeavy;

    internal MenuScene()
    {
        empty = new Rectangle(new Vector2(200, 10), new Vector2(400, 60));
        basic = new Rectangle(new Vector2(200, 120), new Vector2(400, 60));
        normal = new Rectangle(new Vector2(200, 230), new Vector2(400, 60));
        heavy = new Rectangle(new Vector2(200, 340), new Vector2(400, 60));
        veryHeavy = new Rectangle(new Vector2(200, 430), new Vector2(400, 60));
        FixedRectangles.Add(empty);
        FixedRectangles.Add(basic);
        FixedRectangles.Add(normal);
        FixedRectangles.Add(heavy);
        FixedRectangles.Add(veryHeavy);
        Texts.Add(new VisualText(new Vector2(200, 10), "Empty"));
        Texts.Add(new VisualText(new Vector2(200, 120), "Basic"));
        Texts.Add(new VisualText(new Vector2(200, 230), "$Normal"));
        Texts.Add(new VisualText(new Vector2(200, 340), "Heavy"));
        Texts.Add(new VisualText(new Vector2(200, 430), "Very heavy"));
    }

    public override PhysicalScene UpdatePhysicalScene(float deltaTime)
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

            if (normal.Contains(pos))
                return new HeavyScene(20);

            if (heavy.Contains(pos))
                return new HeavyScene(5);

            if (veryHeavy.Contains(pos))
                return new HeavyScene(1);
        }

        clic = currClic;
        return this;
    }

    private bool clic = true;
}