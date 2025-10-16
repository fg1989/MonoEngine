using MonoEngine.Engine;
using MonoEngine.Engine.Collider.Figure;
using MonoEngine.Engine.MathStuff;
using System.Runtime.CompilerServices;

namespace MonoEngine;

internal static class PhysicEngine
{
    internal static void Update(Scene scene, float deltaTime)
    {
        UpdateSpeed(scene, deltaTime);
        UpdateLinks(scene);
        MoveObjects(scene, deltaTime);
    }

    private static void UpdateSpeed(Scene scene, float deltaTime)
    {
        foreach (PhysicalObject item in scene.Objects)
            item.UpdateSpeed(deltaTime);
    }

    private static void UpdateLinks(Scene scene)
    {
        foreach (RigidLink item in scene.Links)
        {
            Vector2 firstSpeed = item.StartObject.Velocity;
            Vector2 secondSpeed = item.EndObject.Velocity;
            Vector2 firstPos = item.StartPoint;
            Vector2 secondPos = item.EndPoint;

            // Le vecteur de direction  du déplacement (depuis end vers start)
            Vector2 dir = firstPos - secondPos;

            // Correct Velocity
            if (firstSpeed != secondSpeed)
            {
                // Parallel Speed
                Vector2 firstParallelSpeed = firstSpeed.ProjectionOn(dir);
                Vector2 secondParallelSpeed = secondSpeed.ProjectionOn(dir);
                float firstMass = item.StartObject.Mass;
                float secondMass = item.EndObject.Mass;

                Vector2 finalParrallelSpeed
                    = ((firstParallelSpeed * firstMass) + (secondParallelSpeed * secondMass)) / (firstMass + secondMass);

                // Perpendicular Speed
                Vector2 perp = dir.Orthogonal;

                Vector2 firstPerpSpeed = firstSpeed.ProjectionOn(perp);
                Vector2 secondPerpSpeed = secondSpeed.ProjectionOn(perp);

                item.StartObject.Velocity = finalParrallelSpeed + firstPerpSpeed;
                item.EndObject.Velocity = finalParrallelSpeed + secondPerpSpeed;
            }

            // Correct Position

            // La différence de distance entre la distance attendu et la distance réelle
            float norm = dir.Norm;
            float dist = norm - item.Length;
            if (!float.IsNull(dist))
            {
                dir /= norm;

                /* On multiplie par la distance a parcourir,
                on divise par 2 car chaque objet fait la moitié du chemin
                car le mouvement sera multiplié par deltaTime ensuite*/
                dir *= dist / 2;

                item.EndObject.TempVelocityCorrection += dir;
                item.StartObject.TempVelocityCorrection -= dir;
            }
        }
    }

    private static void MoveObjects(Scene scene, float deltaTime)
    {
        foreach (PhysicalObject item in scene.Objects)
            MoveObject(scene, item, deltaTime);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void MoveObject(Scene scene, PhysicalObject item, float deltaTime)
    {
        Vector2 decal = (item.Velocity * deltaTime) + item.TempVelocityCorrection;
        item.TempVelocityCorrection = new();

        foreach (Rectangle subItem in scene.FixedRectangles)
        {
            if (item.Collison.Visit(subItem))
            {
                item.Velocity = new Vector2(0, 0);
                return;
            }
        }

        if (decal != Vector2.Null)
            item.MoveBy(decal);
    }
}