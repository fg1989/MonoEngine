using Common;
using Common.Figure;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Engine;

internal static class PhysicEngine
{
    internal static void Update(PhysicalScene scene, float deltaTime)
    {
        UpdateSpeed(scene, deltaTime);
        UpdateLinks(scene);
        MoveObjects(scene, deltaTime);
    }

    private static void UpdateSpeed(PhysicalScene scene, float deltaTime)
    {
        UpdateSpeed(scene.Circles, deltaTime);
        UpdateSpeed(scene.Rectangles, deltaTime);
        UpdateSpeed(scene.Polygones, deltaTime);
    }

    private static void UpdateSpeed<T>(List<PhysicalObject<T>> obj, float deltaTime) where T : IFigure<T>
    {
        if (obj.Count > 10000)
        {
            Parallel.ForEach(obj, x => x.UpdateSpeed(deltaTime));
        }
        else
        {
            foreach (PhysicalObject<T> item in obj)
                item.UpdateSpeed(deltaTime);
        }
    }

    private static void UpdateLinks(PhysicalScene scene)
    {
        // Pas de parallelisation, car il y a générallement peu de lien dans une scène + les liens sont interdépendants
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

    private static void MoveObjects(PhysicalScene scene, float deltaTime)
    {
        if (scene.FixedRectangles.Count > ParallelMove)
        {
            Parallel.ForEach(scene.Circles, x => MoveObject(scene, x, deltaTime));
            Parallel.ForEach(scene.Rectangles, x => MoveObject(scene, x, deltaTime));
            Parallel.ForEach(scene.Polygones, x => MoveObject(scene, x, deltaTime));
            return;
        }

        MoveObjects(scene, scene.Circles, deltaTime);
        MoveObjects(scene, scene.Rectangles, deltaTime);
        MoveObjects(scene, scene.Polygones, deltaTime);
    }

    private static void MoveObjects<T>(PhysicalScene scene, List<PhysicalObject<T>> items, float deltaTime)
        where T : IFigure<T>
    {
        if (scene.FixedRectangles.Count * items.Count > ParallelMove)
        {
            Parallel.ForEach(items, x => MoveObject(scene, x, deltaTime));
        }
        else
        {
            foreach (PhysicalObject<T> item in items)
                MoveObject(scene, item, deltaTime);
        }
    }

    private static void MoveObject<T>(PhysicalScene scene, PhysicalObject<T> item, float deltaTime)
        where T : IFigure<T>
    {
        Vector2 decal = (item.Velocity * deltaTime) + item.TempVelocityCorrection;
        item.TempVelocityCorrection = new();

        foreach (Rectangle subItem in scene.FixedRectangles)
        {
            if (item.Figure.Collide(subItem))
            {
                item.Velocity = new Vector2(0, 0);
                return;
            }
        }

        if (decal != Vector2.Null)
            item.MoveBy(decal);
    }

    private const int ParallelMove = 100000;
}