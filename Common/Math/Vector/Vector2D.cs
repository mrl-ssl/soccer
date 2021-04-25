using System;

namespace MRL.SSL.Common.Math
{
    public class Vector2D<T> : Vector<T>
    {
        public T X { get => values[0]; set => values[0] = value; }
        public T Y { get => values[1]; set => values[1] = value; }

        public Vector2D(T x, T y) : base(x, y) { }
        /// <param name="data">src array</param>
        /// <param name="deepCopy">if true create new array and copy values from src</param>
        public Vector2D(T[] data, bool deepCopy = false)
        {
            if (data.Length == 2)
            {
                if (deepCopy)
                {
                    values = new T[data.Length];
                    for (int i = 0; i < Length; i++)
                        values[i] = data[i];
                }
                else
                    values = data;
            }
            values = new T[2];
            FillFrom(data);
        }

        public T AngleModInRadians(T angle)
        {
            if (th.GreaterOrEqual(angle, th.Zero))
            {
                if (th.LessOrEqual(angle, th.PI)) return angle;
                return th.Sub(angle, th.PI);
            }
            else
            {
                if (th.GreaterOrEqual(angle, th.Multi(th.NegativeOne, th.PI)))
                    return th.Sum(angle, th.PI);
                return th.Multi(th.NegativeOne, angle);
            }
        }
        public T AngleModInDegrees(T angle) => th.Radian2Degree(AngleModInRadians(angle));
        public T AngleInRadians() => th.Atan2(Y, X);
        public T AngleInDegrees() => th.Radian2Degree(AngleInRadians());
        public T Cosine(Vector2D<T> v)
        {
            T l = this.Dot(v);
            T t = th.Multi(Size(), v.Size());
            if (th.Equal(t, th.Zero))
                return th.Zero;

            return th.Divide(l, t);
        }
        /// <returns>(-y,x)</returns>
        public Vector2D<T> Prep() => new Vector2D<T>(th.Negative(Y), X);
        /// <summary>
        /// Vector become (-y, x)
        /// </summary>
        public void PrepSelf()
        {
            T t = X;
            X = th.Negative(Y);
            Y = t;
        }

        /// <summary>
        /// Distance of this point from line wich defined by two point on it
        /// </summary>
        /// <param name="lp1">first point on the line</param>
        /// <param name="lp2">second point on the line</param>
        /// <returns>Distance from line</returns>
        public T DistanceFromLine(Vector2D<T> lp1, Vector2D<T> lp2)
        {
            T tl = th.Multi(th.Sub(lp2.X, lp1.X), th.Sub(lp1.Y, Y));
            T tr = th.Multi(th.Sub(lp1.X, X), th.Sub(lp2.Y, lp1.Y));
            T numerator = th.Abs(th.Sub(tl, tr));
            T denom = (lp2 - lp1).Size();
            if (th.EqualZero(denom)) throw new DivideByZeroException("two given points of line are same!");
            return th.Divide(numerator, denom);
            // var t1 = th.Sum(th.Sub(X, lHead.X), th.Sub(Y, lHead.Y));
            // var t2 = th.Sum(lTail.X, lTail.Y);
            // if(th.Equal(t2,th.Zero)) return th.Zero;
            // T t = ((x - lHead.X) + (y - lHead.Y)) / (lTail.X + lTail.Y);
            // Vector2D<T> r = lHead + (lTail - lHead) * t;
            // return r.Distance(this);
        }

        public Vector2D<T> SegmentNearLine(Vector2D<T> a1, Vector2D<T> b0, Vector2D<T> b1)
        {
            Vector<T> v, n, p;
            T dn, t;

            v = a1 - this;
            n = (b1 - b0).Norm();
            n.ToVector2D().PrepSelf();

            dn = Dot(v, n);
            if (th.EqualZero(dn)) return this;

            t = th.Divide(th.Negative(Dot(this - b0, n)), dn);
            if (th.LessThanZero(t)) t = th.Zero;
            if (th.Greater(t, th.One)) t = th.One;
            p = this + v * t;
            return p.ToVector2D();
        }

        public Vector2D<T> Rotate(T teta, bool clockwise = false)
        {
            Vector2D<T> r = new Vector2D<T>(values, true);
            r.RotateSelf(teta, clockwise);
            return r;
        }
        public void RotateSelf(T teta, bool clockwise = false)
        {
            if (clockwise) teta = th.Negative(teta);
            X = th.Sub(th.Multi(X, th.Cos(teta)), th.Multi(Y, th.Sin(teta))); //x*cos - y*sin
            Y = th.Sum(th.Multi(X, th.Sin(teta)), th.Multi(Y, th.Cos(teta))); //x*sin + y*cos
        }

        public T OffsetToLine(Vector2D<T> x1, Vector2D<T> p)
        {
            T xt2 = th.Square(th.Sub(x1.X, X));
            T yt2 = th.Square(th.Sub(x1.Y, Y));
            T size = th.Sqrt(th.Sum(xt2, yt2));
            if (th.EqualZero(size)) return th.Zero;
            T tl = th.Multi(th.Negative(Y), th.Sub(p.X, X)); // -y * (p.X - x)
            T tr = th.Multi(X, th.Sub(p.Y, Y)); // x * (p.Y - y)
            return th.Abs(th.Divide(th.Sum(tl, tr), size)); // |(-y * (p.X - x) + x * (p.Y - y)) / size|
        }

        public T OffsetAlongLine(Vector2D<T> x1, Vector2D<T> p)
        {
            Vector<T> n, v;
            // get normal to line
            n = x1 - this;
            n.NormSelf();
            v = p - this;
            return n.Dot(v);
        }

        public Vector2D<T> Intersection(Vector2D<T> a2, Vector2D<T> b1, Vector2D<T> b2)
        {
            Vector2D<T> a = (a2 - this).ToVector2D();
            Vector2D<T> b1r = FromAngle(th.Negative(a.AngleInRadians()), (b1 - this).Size());
            Vector2D<T> b2r = FromAngle(th.Negative(a.AngleInRadians()), (b2 - this).Size());
            Vector2D<T> br = (b1r - b2r).ToVector2D();
            Vector2D<T> t = new Vector2D<T>(th.Multi(th.Sub(b2r.X, b2r.Y), th.Divide(br.X, br.Y)), th.Zero);
            return FromAngle(a.AngleInRadians(), t.Size()).Sum(this).ToVector2D();
        }

        public Vector2D<T> Interpolate(Vector2D<T> end, T amount)
        {
            T tl = th.Sum(th.Multi(X, th.Sub(th.One, amount)), th.Multi(end.X, amount)); //X * (1 - amount) + end.X * amount
            T tr = th.Sum(th.Multi(Y, th.Sub(th.One, amount)), th.Multi(end.Y, amount)); //X * (1 - amount) + end.X * amount
            return new Vector2D<T>(tl, tr);
        }

        public Vector3D<T> Cross(Vector2D<T> v)
        {
            return new Vector3D<T>(th.Zero, th.Zero, th.Sub(th.Multi(X, v.Y), th.Multi(Y, v.X)));
        }

        public static Vector2D<T> SegmentNearLine(Vector2D<T> a0, Vector2D<T> a1, Vector2D<T> b0, Vector2D<T> b1) { return a0.SegmentNearLine(a1, b0, b1); }

        public static T OffsetToLine(Vector2D<T> x0, Vector2D<T> x1, Vector2D<T> p) { return x0.OffsetToLine(x1, p); }
        
        public static T OffsetAlongLine(Vector2D<T> x0, Vector2D<T> x1, Vector2D<T> p) { return x0.OffsetAlongLine(x1, p); }
        
        /// <summary>
        /// Distance of p from line wich defined by two point on it
        /// </summary>
        /// <param name="p">target point</param>
        /// <param name="lp1">first point on the line</param>
        /// <param name="lp2">second point on the line</param>
        /// <returns>Distance of p from line</returns>
        public static T DistanceFromLine(Vector2D<T> p, Vector2D<T> lp1, Vector2D<T> lp2) { return p.DistanceFromLine(lp1, lp2); }
        
        public static Vector2D<T> Intersection(Vector2D<T> a1, Vector2D<T> a2, Vector2D<T> b1, Vector2D<T> b2) { return a1.Intersection(a2, b1, b2); }

        public static Vector2D<T> FromAngle(T angle, T size) => new Vector2D<T>(th.Multi(size, th.Cos(angle)), th.Multi(size, th.Sin(angle)));
        
        public static Vector3D<T> operator *(Vector2D<T> v1, Vector2D<T> v2) { return v1.Cross(v2); }
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