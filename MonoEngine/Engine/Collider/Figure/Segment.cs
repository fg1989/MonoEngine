

using MonoEngine.Engine.MathStuff;

namespace MonoEngine.Engine.Collider.Figure
{
    /// <summary>
    /// Représente un sgement entre 2 points
    /// </summary>
    public struct Segment : ICollider, IColliderVisitor
    {
        Vector2 start;
        Vector2 end;

        public Vector2[] Points
        {
            get => [start, end];
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

        public Vector2 Center => (start + end) / 2;

        public Segment(Vector2 start, Vector2 end)
        {
            this.start = start;
            this.end = end;

        }

        public Vector2 GetNearest(Vector2 to)
        {
            Vector2 fromBase = FromBase;
            Vector2 projection = (to - start).ProjectOn(fromBase);
            return
                projection.Norm >= fromBase.Norm ? end :
                projection.Norm <= 0 ? start :
                projection + start;
        }
        public bool CanProjectOnMe(Vector2 point)
        {
            Vector2 fromBase = FromBase;
            Vector2 projection = (point - start).ProjectOn(fromBase);
            return
                projection.Norm <= fromBase.Norm && projection.Norm >= 0;
        }
        public bool Contains(Vector2 point)
        {
            Vector2 fromBase = FromBase;
            Vector2 fromBasePoint = point - start;
            return fromBasePoint.AngleRad == fromBase.AngleRad && fromBasePoint.Norm < fromBase.Norm && fromBasePoint.Norm >= 0;
        }


        public bool IsAtRight(Vector2 point)
        {
            Vector2 side = start - end;
            Vector2 t = point - end;

            float det = side.X * t.Y - side.Y * t.X;
            return det < 0;
        }
        public bool Intersects(ICollider collider)
        {
            return collider.Accept(this);
        }

        public bool Accept(IColliderVisitor visitor)
        {
            return visitor.Intersects(this);
        }

        public bool Intersects(Segment other)
        {
            if (FromBase.Determinant(other.end - start) * FromBase.Determinant(other.start - start) >= 0)
                return false;
            return other.FromBase.Determinant(end - other.start) * other.FromBase.Determinant(end - other.start) < 0;

        }

        public bool Intersects(Circle circle)
        {
            Vector2 nearest = GetNearest(circle.Center);
            return circle.Contains(nearest);
        }

        public bool Intersects(Rectangle rectangle)
        {
            bool rightOrLeft = IsAtRight(rectangle.Points[0]);
            for (int i = 1; i < rectangle.Points.Length; i++)
            {
                if (rightOrLeft != IsAtRight(rectangle.Points[i]))
                {
                    return true;
                }
            }
            return false;
        }

        public bool Intersects(Polygone polygone)
        {
            bool rightOrLeft = IsAtRight(polygone.Points[0]);
            for (int i = 1; i < polygone.Points.Length; i++)
            {
                if (rightOrLeft != IsAtRight(polygone.Points[i]))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
