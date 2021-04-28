using System;
using ProtoBuf;

namespace MRL.SSL.Common.Math
{
    [ProtoContract]
    [ProtoInclude(1, typeof(VectorF3D))]
    public abstract class Vector3D<T>
    {
        protected T z;
        protected T x;
        protected T y;

        [ProtoMember(1)]
        public T X { get => x; set => x = value; }

        [ProtoMember(2)]
        public T Y { get => y; set => y = value; }

        [ProtoMember(3)]
        public T Z { get => z; set => z = value; }

        protected Vector3D() { }
        protected Vector3D(T X, T Y, T Z) { x = X; y = Y; z = Z; }

        public abstract T Dot(Vector3D<T> v);
        public abstract T TripleProdcut(Vector2D<T> v, Vector2D<T> u);
        public abstract Vector3D<T> Norm();
        public abstract Vector3D<T> Cross(Vector3D<T> p);
        public abstract Vector3D<T> RotateX(T angle);
        public abstract Vector3D<T> RotateY(T angle);
        public abstract Vector3D<T> RotateZ(T angle);
        public abstract T ClosestPointTime(Vector3D<T> v1, Vector3D<T> x2, Vector3D<T> v2);
        public abstract T DistanceSegToSeg(Vector3D<T> s1b, Vector3D<T> s2a, Vector3D<T> s2b);
        public abstract T Distance(Vector3D<T> v);
        public abstract T SqDistance(Vector3D<T> v);
        public abstract T DistanceToLine(Vector3D<T> lHead, Vector3D<T> lTail);
        public abstract Vector3D<T> Interpolate(Vector3D<T> end, T amount);
        public abstract Vector3D<T> PointOnSegment(Vector3D<T> x1, Vector3D<T> p);
        public abstract Vector3D<T> Extend(T X, T Y, T Z);
        public abstract Vector3D<T> Abs();
        public abstract Vector3D<T> Max(Vector3D<T> v);
        public abstract Vector3D<T> Bound(T low, T high);
        public abstract Vector3D<T> Sub(Vector3D<T> v);
        public abstract Vector3D<T> Add(Vector3D<T> v);
        public abstract Vector3D<T> Scale(T s);
        public abstract Vector3D<T> Divide(T s);
        public abstract Vector3D<T> Reverse();

        public abstract T Length();
        public abstract T SqLength();
        public abstract void Normalize();
        public abstract void NormTo(T newLength);


        public static Vector3D<T> Interpolate(Vector3D<T> start, Vector3D<T> end, T amount) { return start.Interpolate(end, amount); }
        public static T Dot(Vector3D<T> v1, Vector3D<T> v2) { return v1.Dot(v2); }
        public static T ClosestPointTime(Vector3D<T> x1, Vector3D<T> v1, Vector3D<T> x2, Vector3D<T> v2) { return x1.ClosestPointTime(v1, x2, v2); }   // returns time of closest point of approach of two points
        public static Vector3D<T> Cross(Vector3D<T> v1, Vector3D<T> v2) { return v1.Cross(v2); }
        public static T DistanceSegToSeg(Vector3D<T> s1a, Vector3D<T> s1b, Vector3D<T> s2a, Vector3D<T> s2b) { return s1a.DistanceSegToSeg(s1b, s2a, s2b); }     // return distnace between segments s1a-s1b and s2a-s2b
        public static T Distance(Vector3D<T> v1, Vector3D<T> v2) { return v1.Distance(v2); }
        public static T SqDistance(Vector3D<T> v1, Vector3D<T> v2) { return v1.SqDistance(v2); }
        public static T DistanceToLine(Vector3D<T> p, Vector3D<T> lHead, Vector3D<T> lTail) { return p.DistanceToLine(lHead, lTail); }
        public static Vector3D<T> Abs(Vector3D<T> v) { return v.Abs(); }
        public static Vector3D<T> Max(Vector3D<T> v1, Vector3D<T> v2) { return v1.Max(v2); }
        public static Vector3D<T> Bound(Vector3D<T> v, T low, T high) { return v.Bound(low, high); }
        public static Vector3D<T> PointOnSegment(Vector3D<T> x0, Vector3D<T> x1, Vector3D<T> p) { return x0.PointOnSegment(x1, p); }      // returns nearest point on line segment x0-x1 to point p
        public static Vector3D<T> operator -(Vector3D<T> v) { return v.Reverse(); }
        public static Vector3D<T> operator -(Vector3D<T> v1, Vector3D<T> v2) { return v1.Sub(v2); }
        public static Vector3D<T> operator +(Vector3D<T> v1, Vector3D<T> v2) { return v1.Add(v2); }
        public static Vector3D<T> operator *(Vector3D<T> v1, Vector3D<T> v2) { return v1.Cross(v2); }
        public static Vector3D<T> operator *(T p, Vector3D<T> v) { return v.Scale(p); }
        public static Vector3D<T> operator *(Vector3D<T> v, T p) { return v.Scale(p); }
        public static Vector3D<T> operator /(T p, Vector3D<T> v) { return v.Divide(p); }
        public static Vector3D<T> operator /(Vector3D<T> v, T p) { return v.Divide(p); }
        public static bool operator ==(Vector3D<T> v1, Vector3D<T> v2)
        {
            if (v1 is null && v2 is null) { return true; }
            if (v1 is null) { return false; }
            return v1.Equals(v2);
        }
        public static bool operator !=(Vector3D<T> v1, Vector3D<T> v2)
        {
            if (v1 is null && v2 is null) { return false; }
            if (v1 is null) { return true; }
            return !v1.Equals(v2);
        }
        public override string ToString() { return string.Format("({0},{1},{2})", x, y, z); }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}