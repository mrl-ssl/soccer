using System;
using ProtoBuf;

namespace MRL.SSL.Common.Math
{
    public class Vector2D<T> : Vector<T>
    {
        public T X { get => values[0]; set => values[0] = value; }
        public T Y { get => values[1]; set => values[1] = value; }

        public Vector2D(T x, T y) : base(new T[2] { x, y }) { }

        public T AngleModInRadians(T angle)
        {
            if (type_helper.GreaterOrEqual(angle, type_helper.Zero))
            {
                if (type_helper.LessOrEqual(angle, type_helper.PI)) return angle;
                return type_helper.Sub(angle, type_helper.PI);
            }
            else
            {
                if (type_helper.GreaterOrEqual(angle, type_helper.Multi(type_helper.NegativeOne, type_helper.PI)))
                    return type_helper.Sum(angle, type_helper.PI);
                return type_helper.Multi(type_helper.NegativeOne, angle);
            }
        }
        public T AngleModInDegrees(T angle) => type_helper.Radian2Degree(AngleModInRadians(angle));
        public T AngleInRadians() => type_helper.Atan2(Y, X);
        public T AngleInDegrees() => type_helper.Radian2Degree(AngleInRadians());
        public T Cosine(Vector2D<T> v)
        {
            T l = this.Dot(v);
            T t = type_helper.Multi(Size(), v.Size());
            if (type_helper.Equal(t, type_helper.Zero))
                return type_helper.Zero;
            return type_helper.Divide(l, t);
        }
    }
    /*public abstract class Vector2D<T>
    {
        protected T x;
        protected T y;


        public abstract T X { get; set; }
        public abstract T Y { get; set; }

        protected Vector2D() { }
        protected Vector2D(T X, T Y) { x = X; y = Y; }

        public abstract T Dot(Vector2D<T> v);
        public abstract T AngleModInDegrees(T angle);
        public abstract T AngleModInRadians(T angle);
        public abstract T AngleInRadians();
        public abstract T AngleInDegrees();
        public abstract T Cosine(Vector2D<T> v);
        public abstract T ClosestPointTime(Vector2D<T> v1, Vector2D<T> x2, Vector2D<T> v2);
        public abstract T DistanceSegToSeg(Vector2D<T> s1b, Vector2D<T> s2a, Vector2D<T> s2b);
        public abstract T Distance(Vector2D<T> v);
        public abstract T SqDistance(Vector2D<T> v);
        public abstract T DistanceToLine(Vector2D<T> lHead, Vector2D<T> lTail);
        public abstract Vector2D<T> SegmentNearLine(Vector2D<T> a1, Vector2D<T> b0, Vector2D<T> b1);
        public abstract Vector2D<T> Extend(T x, T y);
        public abstract T OffsetToLine(Vector2D<T> x1, Vector2D<T> p);
        public abstract T OffsetAlongLine(Vector2D<T> x1, Vector2D<T> p);
        public abstract Vector2D<T> GetRotate(T angle);
        public abstract Vector2D<T> Intersection(Vector2D<T> a2, Vector2D<T> b1, Vector2D<T> b2);
        public abstract Vector2D<T> Interpolate(Vector2D<T> end, T amount);
        public abstract void Rotate(T angle);
        public abstract Vector2D<T> Norm();
        public abstract Vector2D<T> GetPerp();
        public abstract Vector2D<T> Abs();
        public abstract Vector2D<T> Max(Vector2D<T> v);
        public abstract Vector2D<T> Bound(T low, T high);
        public abstract Vector2D<T> Scale(T s);
        public abstract Vector3D<T> Cross(Vector2D<T> v);
        public abstract Vector2D<T> Sub(Vector2D<T> v);
        public abstract Vector2D<T> Add(Vector2D<T> v);
        public abstract Vector2D<T> Divide(T p);
        public abstract Vector2D<T> Reverse();
        public abstract T VertexAngle(Vector2D<T> b, Vector2D<T> c);
        public abstract Vector2D<T> PointOnSegment(Vector2D<T> x1, Vector2D<T> p);
        public abstract T AngleBetweenInRadians(Vector2D<T> v);
        public abstract T AngleBetweenInDegrees(Vector2D<T> v);
        public abstract Vector2D<T> PrependecularPoint(Vector2D<T> start, Vector2D<T> from);
        public abstract bool IsBetween(Vector2D<T> other, Vector2D<T> v);

        public abstract T Length();
        public abstract T SqLength();
        public abstract void Normalize();
        public abstract void NormTo(T newLength);

        public static T Dot(Vector2D<T> v1, Vector2D<T> v2) { return v1.Dot(v2); }
        public static Vector2D<T> Interpolate(Vector2D<T> start, Vector2D<T> end, T amount) { return start.Interpolate(end, amount); }
        public static T Cosine(Vector2D<T> v1, Vector2D<T> v2) { return v1.Cosine(v2); }    // equivalent to Dot(v1.Norm(),v2.Norm())
        public static T ClosestPointTime(Vector2D<T> x1, Vector2D<T> v1, Vector2D<T> x2, Vector2D<T> v2) { return x1.ClosestPointTime(v1, x2, v2); }   // returns time of closest point of approach of two points
        public static T AngleBetweenInRadians(Vector2D<T> v1, Vector2D<T> v2) { return v1.AngleBetweenInRadians(v2); }
        public static T AngleBetweenInDegrees(Vector2D<T> v1, Vector2D<T> v2) { return v1.AngleBetweenInDegrees(v2); }
        public static T DistanceSegToSeg(Vector2D<T> s1a, Vector2D<T> s1b, Vector2D<T> s2a, Vector2D<T> s2b) { return s1a.DistanceSegToSeg(s1b, s2a, s2b); }     // return distnace between segments s1a-s1b and s2a-s2b
        public static T Distance(Vector2D<T> v1, Vector2D<T> v2) { return v1.Distance(v2); }
        public static T SqDistance(Vector2D<T> v1, Vector2D<T> v2) { return v1.SqDistance(v2); }
        public static T DistanceToLine(Vector2D<T> p, Vector2D<T> lHead, Vector2D<T> lTail) { return p.DistanceToLine(lHead, lTail); }
        public static Vector2D<T> Intersection(Vector2D<T> a1, Vector2D<T> a2, Vector2D<T> b1, Vector2D<T> b2) { return a1.Intersection(a2, b1, b2); }
        public static Vector2D<T> SegmentNearLine(Vector2D<T> a0, Vector2D<T> a1, Vector2D<T> b0, Vector2D<T> b1) { return a0.SegmentNearLine(a1, b0, b1); }
        public static T OffsetToLine(Vector2D<T> x0, Vector2D<T> x1, Vector2D<T> p) { return x0.OffsetToLine(x1, p); }
        public static T OffsetAlongLine(Vector2D<T> x0, Vector2D<T> x1, Vector2D<T> p) { return x0.OffsetAlongLine(x1, p); }
        public static T VertexAngle(Vector2D<T> a, Vector2D<T> b, Vector2D<T> c) { return a.VertexAngle(b, c); }     // gives counterclockwise angle from <a-b> to <c-b> in radian
        public static Vector2D<T> PointOnSegment(Vector2D<T> x0, Vector2D<T> x1, Vector2D<T> p) { return x0.PointOnSegment(x1, p); }      // returns nearest point on line segment x0-x1 to point p
        public static bool IsBetween(Vector2D<T> Right, Vector2D<T> Left, Vector2D<T> v) { return Right.IsBetween(Left, v); }
        public static Vector2D<T> operator *(T p, Vector2D<T> v) { return v.Scale(p); }
        public static Vector2D<T> operator *(Vector2D<T> v, T p) { return v.Scale(p); }
        public static Vector3D<T> operator *(Vector2D<T> v1, Vector2D<T> v2) { return v1.Cross(v2); }
        public static Vector2D<T> operator +(Vector2D<T> v1, Vector2D<T> v2) { return v1.Add(v2); }
        public static Vector2D<T> operator -(Vector2D<T> v1, Vector2D<T> v2) { return v1.Sub(v2); }
        public static Vector2D<T> operator -(Vector2D<T> v) { return v.Reverse(); }
        public static Vector2D<T> operator /(Vector2D<T> v1, T p) { return v1.Divide(p); }
        public static Vector2D<T> operator /(T p, Vector2D<T> v1) { return v1.Divide(p); }
        public static bool operator ==(Vector2D<T> v1, Vector2D<T> v2)
        {
            if (v1 is null && v2 is null) { return true; }
            if (v1 is null) { return false; }
            return v1.Equals(v2);
        }
        public static bool operator !=(Vector2D<T> v1, Vector2D<T> v2)
        {
            if (v1 is null && v2 is null) { return false; }
            if (v1 is null) { return true; }
            return !v1.Equals(v2);
        }

        public override string ToString()
        {
            return string.Format("({0},{1})", x, y);
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }*/
}