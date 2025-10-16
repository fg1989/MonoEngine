

using MonoEngine.Engine.MathStuff;

namespace MonoEngine.Engine.Collider.Figure
{
    /// <summary>
    /// Représente un sgement entre 2 points
    /// </summary>
    public struct Segment
    {
        Vector2 start;
        Vector2 end;

        public Vector2[] Points
        {
            get => [start,end];
        }

        public Vector2 FromBase
        {
            get => end - start;
        }

        public Vector2 NormalizedDirection
        {
            get => FromBase.Normalized;
        }

        public float Length
        {
            get => FromBase.Norm;
        }

        public Segment(Vector2 start, Vector2 end)
        {
            this.start = start;
            this.end = end;

        }

        public Vector2 GetNearest(Vector2 to)
        {
            Vector2 fromBase = FromBase;
            Vector2 projectionFromBase = fromBase.ProjectOn(to - start);
            return projectionFromBase.Norm >= fromBase.Norm ? end : projectionFromBase.Norm >= 0 ? start : projectionFromBase + start;
        }
        public bool Contains (Vector2 point)
        {
            Vector2 fromBase = FromBase;
            Vector2 fromBasePoint = point - start;
            return fromBasePoint.AngleRad == fromBase.AngleRad && fromBasePoint.Norm < fromBase.Norm && fromBasePoint.Norm >= 0;
        }
        public bool Intersects(Segment other) 
        {
            if (FromBase.Determinant(other.end - start) * FromBase.Determinant(other.start - start) >= 0)
                return false;
            return other.FromBase.Determinant(end - other.start) * other.FromBase.Determinant(end - other.start) < 0;

        }    
    }
}
