using Engine;
using Engine.Collider.Figure;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections;
using System.Globalization;
using Rectangle = Engine.Collider.Figure.Rectangle;
using Vector2 = Engine.MathStuff.Vector2;

namespace Project;

internal sealed class HeavyScene : Scene
{
    internal HeavyScene(float objectSize)
    {
        FixedRectangles.Add(new Rectangle(new Vector2(0, 490), new Vector2(800, 10)));
        FixedRectangles.Add(new Rectangle(new Vector2(0, 0), new Vector2(10, 500)));
        FixedRectangles.Add(new Rectangle(new Vector2(790, 0), new Vector2(10, 500)));

        float demiSize = objectSize / 2f;
        float tripleDemi = demiSize + demiSize + demiSize;
        float doubleSize = objectSize + objectSize;
        float quartSize = demiSize / 2;

        int maxLength = (int)(780 / tripleDemi) + 1;
        int maxHeight = (int)(490 / doubleSize);

        int maxLengthSquare = (int)((780 - demiSize - quartSize) / tripleDemi) + 1;
        int maxHeightSquare = (int)((490 - demiSize) / doubleSize);

        SetMaxPrime(Math.Max(maxLength * maxHeight, maxHeightSquare * maxLengthSquare));

        for (int j = 0; j < maxHeight; j++)
        {
            for (int i = 1; i < maxLength; i++)
            {
                int key = (maxLength * maxHeight) - ((j * maxLength) + i);
                if (!IsPrime(key))
                {
                    Objects.Add(
                        new PhysicalObject(
                            new Circle(new((tripleDemi * i) + 10 - demiSize - demiSize, j * doubleSize), demiSize),
                            key));
                }
            }
        }

        for (int j = 0; j < maxHeightSquare; j++)
        {
            for (int i = 1; i < maxLengthSquare; i++)
            {
                int key = (maxHeightSquare * maxLengthSquare) - ((j * maxLengthSquare) + i);
                if (!IsPrime(key))
                {
                    Objects.Add(
                        new PhysicalObject(
                            new Rectangle(
                                new Vector2((tripleDemi * i) + 10 - demiSize - demiSize + quartSize, (j * doubleSize) + demiSize),
                                new Vector2(objectSize, objectSize)),
                            key));
                }
            }
        }

        Texts.Add(
            new VisualText(
                new Vector2(0, 20),
                $"Objects count : {Objects.Count.ToString(CultureInfo.InvariantCulture)}"));
    }

    protected override Scene UpdateScene(GameTime gameTime)
        => Keyboard.GetState().IsKeyDown(Keys.Back) ? new MenuScene() : this;

    private static bool IsPrime(int i) => primes[i];

    private static BitArray InitPrimes()
    {
        BitArray res = new(3);
        res[0] = false;
        res[1] = false;
        res[2] = true;
        return res;
    }

    private static void SetMaxPrime(int max)
    {
        if (max <= primes.Count)
            return;

        BitArray res = new(max);

        for (int i = 0; i < primes.Count; i++)
            res[i] = primes[i];

        for (int i = primes.Count; i < max; i++)
        {
            int root = ((int)MathF.Sqrt(i)) + 2;
            bool isPrime = true;

            for (int j = 2; j < root; j++)
            {
                if (res[j] && i % j == 0)
                {
                    isPrime = false;
                    break;
                }
            }

            res[i] = isPrime;
        }

        primes = res;
    }

    private static BitArray primes = InitPrimes();
}