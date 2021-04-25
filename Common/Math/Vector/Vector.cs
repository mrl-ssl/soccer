using System.Collections;
using System.Collections.Generic;
using MRL.SSL.Common.Math.Helpers;

namespace MRL.SSL.Common.Math
{
    public class Vector<T> : IEnumerable<T>
    {
        /// <summary>
        /// Instance of IGenericMathHelper wich do operations like add,sub,multy,...
        /// </summary>
        protected static readonly IGenericMathHelper<T> th = MathHelper.GetGenericMathHelper<T>();
        protected T[] values;

        public int Length { get => values.Length; }
        public T[] ValuesAsArray { get => values; }
        public T this[int index]
        {
            get => values[index];
            set => values[index] = value;
        }

        /// <param name="length">Length of vector</param>
        public Vector(int length)
        {
            values = new T[length];
        }

        public Vector(params T[] vals)
        {
            values = vals;
        }

        /// <param name="data">src array</param>
        /// <param name="deepCopy">if true create new array and copy values from src</param>
        public Vector(T[] data, bool deepCopy = false)
        {
            if (deepCopy)
            {
                values = new T[data.Length];
                for (int i = 0; i < Length; i++)
                    values[i] = data[i];
            }
            else
                values = data;
            values = data;
        }

        public Vector<T> Clone() => new Vector<T>(values, true);

        public Vector2D<T> ToVector2D()
        {
            if (this is Vector2D<T> v) return v;
            return new Vector2D<T>(values);
        }

        public Vector3D<T> ToVector3D()
        {
            if (this is Vector3D<T> v) return v;
            return new Vector3D<T>(values);
        }

        /// <summary>
        /// Fill vector by given value
        /// </summary>
        public void FillBy(T x)
        {
            for (int i = 0; i < Length; i++)
                values[i] = x;
        }

        /// <summary>
        /// Fill vector by given values
        /// </summary>
        public void FillFrom(params T[] x)
        {
            for (int i = 0; i < System.Math.Min(Length, x.Length); i++)
                values[i] = x[i];
        }

        /// <returns>sqrt(x0^2 + x1^2 + ...)</returns>
        public T Size() => th.Sqrt(SquareSize());

        /// <summary>
        /// vector size without radical (size^2)
        /// </summary>
        /// <returns>size^2</returns>
        public T SquareSize()
        {
            T result = th.Zero;
            for (int i = 0; i < Length; i++)
                result = th.Sum(result, th.Square(values[i]));
            return result;
        }

        /// <summary>
        /// Normalized of this vector
        /// if size be zero return zero vector
        /// </summary>
        /// <returns>Normalized of this vector </returns>
        public Vector<T> Norm()
        {
            Vector<T> r = Clone();
            r.NormSelf();
            return r;
        }

        /// <summary>
        /// Normalized of this vector with new length
        /// if size be zero return zero vector
        /// </summary>
        /// <returns>Normalized of this vector with new length</returns>
        public Vector<T> NormTo(T newLength)
        {
            Vector<T> r = Clone();
            r.NormSelf(newLength);
            return r;
        }

        /// <summary>
        /// Normalized this vector
        /// if size be zero this vector become zero vector
        /// </summary>
        public void NormSelf()
        {
            T size = Size();
            if (th.EqualZero(size))
                FillBy(th.Zero);
            else
                for (int i = 0; i < Length; i++)
                    values[i] = th.Divide(values[i], size);
        }

        /// <summary>
        /// Normalized this vector to new length
        /// if size be zero this vector become zero vector
        /// </summary>
        public void NormSelf(T newLength)
        {
            T size = Size();
            if (th.EqualZero(size))
                FillBy(th.Zero);
            else
                for (int i = 0; i < Length; i++)
                    values[i] = th.Multi(values[i], th.Divide(newLength, size));
        }

        /// <summary>
        /// if length of src != dist min of those two is used
        /// </summary>
        public T Dot(Vector<T> v)
        {
            T result = th.Zero;
            for (int i = 0; i < System.Math.Min(Length, v.Length); i++)
                result = th.Sum(result, th.Multi(values[i], v.values[i]));
            return result;
        }

        /// <summary>
        /// Distance of this matrix from another
        /// its limited to this matrix dimention and higher dimentions ignored
        /// </summary>
        public T Distance(Vector<T> v) => th.Sqrt(SquareDistance(v));

        /// <summary>
        /// return distance without radical (distance^2)
        /// its limited to this matrix dimention and higher dimentions ignored
        /// </summary>
        public T SquareDistance(Vector<T> v)
        {
            T result = th.Zero;
            for (int i = 0; i < System.Math.Min(Length, v.Length); i++)
            {
                T diff = th.Sub(values[i], v.values[i]);
                result = th.Sum(result, th.Square(diff));
            }
            return result;
        }

        /// <summary>
        /// Create new vector wich is result of extending this vector by given scales (x0 + scales0, x1 + scales1, ...)
        /// if scales length != this vector length it use min(this length, scales length)
        /// </summary>
        public Vector<T> Extend(params T[] scales)
        {
            Vector<T> r = Clone();
            r.ExtendSelf(scales);
            return r;
        }

        /// <summary>
        /// Extend this vector by given scales (x0 + scales0, x1 + scales1, ...)
        /// if scales length != this vector length it use min(this length, scales length)
        /// </summary>
        public void ExtendSelf(params T[] scales)
        {
            for (int i = 0; i < System.Math.Min(Length, scales.Length); i++)
                values[i] = th.Sum(values[i], scales[i]);
        }

        /// <summary>
        /// Create new vector wich is result of extending this vector by given scales (x0 + scales0, x1 + scales1, ...)
        /// if scales length != this vector length it use min(this length, scales length)
        /// </summary>
        public Vector<T> Extend(Vector<T> scales) => Extend(scales.values);

        /// <summary>
        /// Extend this vector by given scales (x0 + scales0, x1 + scales1, ...)
        /// if scales length != this vector length it use min(this length, scales length)
        /// </summary>
        public void ExtendSelf(Vector<T> scales) => ExtendSelf(scales.values);

        public Vector<T> Interpolate(Vector<T> end, T amount)
        {
            Vector<T> r = new Vector<T>(Length);
            for (int i = 0; i < System.Math.Min(Length, end.Length); i++)
            {
                var t1 = th.Multi(values[i], th.Sub(th.One, amount)); //v[i]*(1-amount)
                var t2 = th.Multi(end.values[i], amount); //end[i] * amount
                r.values[i] = th.Sum(t1, t2);
            }
            return r;
        }

        public Vector<T> Abs()
        {
            Vector<T> r = Clone();
            r.AbsSelf();
            return r;
        }
        public void AbsSelf()
        {
            for (int i = 0; i < Length; i++)
                values[i] = th.Abs(values[i]);
        }

        public Vector<T> Max(Vector<T> v)
        {
            int l = System.Math.Min(Length, v.Length);
            Vector<T> r = new Vector<T>(l);
            for (int i = 0; i < l; i++)
                r.values[i] = th.Max(values[i], v.values[i]);
            return r;
        }

        public Vector<T> Bound(T low, T high)
        {
            Vector<T> r = Clone();
            r.BoundSelf(low, high);
            return r;
        }

        public void BoundSelf(T low, T high)
        {
            for (int i = 0; i < Length; i++)
                values[i] = th.Bound(values[i], low, high);
        }

        /// <returns>(x0 * s, x1 * s, ...)</returns>
        public Vector<T> Scale(T s)
        {
            Vector<T> r = Clone();
            r.ScaleSelf(s);
            return r;
        }

        /// <summary>
        /// this vector become (x0 * s, x1 * s, ...)
        /// </summary>
        public void ScaleSelf(T s)
        {
            for (int i = 0; i < Length; i++)
                values[i] = th.Multi(values[i], s);
        }

        /// <returns>this + v</returns>
        public Vector<T> Sum(Vector<T> v)
        {
            int l = System.Math.Min(Length, v.Length);
            Vector<T> r = new Vector<T>(l);
            for (int i = 0; i < l; i++)
                r.values[i] = th.Sum(values[i], v.values[i]);
            return r;
        }

        /// <returns>this - v</returns>
        public Vector<T> Sub(Vector<T> v)
        {
            int l = System.Math.Min(Length, v.Length);
            Vector<T> r = new Vector<T>(l);
            for (int i = 0; i < l; i++)
                r.values[i] = th.Sub(values[i], v.values[i]);
            return r;
        }

        /// <returns>(x0/c, x1/c, ...)</returns>
        public Vector<T> Divide(T c)
        {
            if (th.EqualZero(c)) throw new System.DivideByZeroException();
            Vector<T> r = new Vector<T>(Length);
            for (int i = 0; i < Length; i++)
                r.values[i] = th.Divide(values[i], c);
            return r;
        }

        /// <returns>(-x0, -x1, ...)</returns>
        public Vector<T> Reverse()
        {
            Vector<T> r = Clone();
            r.ReverseSelf();
            return r;
        }

        /// <summary>
        /// this vector become (-x0, -x1, ...)
        /// </summary>
        public void ReverseSelf()
        {
            for (int i = 0; i < Length; i++)
                values[i] = th.Negative(values[i]);
        }

        /// <summary>
        /// returns nearest point on line segment this-x1 to point p
        /// </summary>
        /// <returns>nearest point on line segment this-x1 to point p</returns>
        public Vector<T> PointOnSegment(Vector<T> x1, Vector<T> p)
        {
            Vector<T> sx, sp, r;
            T f, l;

            sx = x1 - this;
            sp = p - this;

            f = sx.Dot(sp);
            if (th.LessOrEqualThanZero(f)) return this;         // also handles this=x1 case

            l = sx.SquareSize();
            if (th.GreaterOrEqual(f, l)) return x1;

            r = this + sx * th.Divide(f, l);

            return r;
        }

        public T ClosestPointTime(Vector<T> v1, Vector<T> x2, Vector<T> v2)
        {
            Vector<T> v = v1 - v2;
            T sl = v.SquareSize();
            T t;

            if (th.EqualZero(sl)) return th.Zero; // parallel tracks, any time is ok.

            t = th.Divide(th.Negative(v.Dot(this - x2)), sl);
            if (th.LessThanZero(t)) return th.Zero; // nearest time was in the past, now is closest point from now on.
            return t;
        }

        public T DistanceSegToSeg(Vector<T> s1b, Vector<T> s2a, Vector<T> s2b)
        {
            Vector<T> dp;
            Vector<T> u = s1b - this;
            Vector<T> v = s2b - s2a;
            Vector<T> w = this - s2a;
            T a = Dot(u, u);        // always >= 0
            T b = Dot(u, v);
            T c = Dot(v, v);        // always >= 0
            T d = Dot(u, w);
            T e = Dot(v, w);
            T D = th.Sub(th.Multi(a, c), th.Square(b));  // a * c - b * b | always >= 0
            T sc, sN, sD = D;      // sc = sN / sD, default sD = D >= 0
            T tc, tN, tD = D;      // tc = tN / tD, default tD = D >= 0

            // compute the line parameters of the two closest points
            if (th.LessThanZero(D))
            {    // the lines are almost parallel
                sN = th.Zero;
                tN = e;
                tD = c;
            }
            else
            {   // get the closest points on the infinite lines
                sN = th.Sub(th.Multi(b, e), th.Multi(c, d)); // b * e - c * d
                tN = th.Sub(th.Multi(a, e), th.Multi(b, d)); // a * e - b * d
                if (th.LessThanZero(sN))
                {   // sc < 0 => the s=0 edge is visible
                    sN = th.Zero;
                    tN = e;
                    tD = c;
                }
                else if (th.Greater(sN, sD))
                {  // sc > 1 => the s=1 edge is visible
                    sN = sD;
                    tN = th.Sum(e, b);
                    tD = c;
                }
            }

            if (th.LessThanZero(tN))
            {   // tc < 0 => the t=0 edge is visible
                tN = th.Zero;
                // recompute sc for this edge
                if (th.LessThanZero(th.Negative(d)))
                {
                    sN = th.Zero;
                }
                else if (th.Less(th.Negative(d), a))
                {
                    sN = sD;
                }
                else
                {
                    sN = th.Negative(d);
                    sD = a;
                }
            }
            else if (th.Greater(tN, tD))
            {   // tc > 1 => the t=1 edge is visible
                tN = tD;
                // recompute sc for this edge
                if (th.LessThanZero(th.Sum(th.Negative(d), b)))
                {
                    sN = th.Zero;
                }
                else if (th.Greater(th.Sum(th.Negative(d), b), a))
                {
                    sN = sD;
                }
                else
                {
                    sN = th.Sum(th.Negative(d), b); //(-d + b)
                    sD = a;
                }
            }
            // finally do the division to get sc and tc
            sc = th.Divide(sN, sD);
            tc = th.Divide(tN, tD);

            // get the difference of the two closest points
            dp = w + u * sc - v * tc; // = S1(sc) - S2(tc)

            return dp.Size(); // return the closest distance
        }

        public IEnumerator<T> GetEnumerator() => ((IEnumerable<T>)values).GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => values.GetEnumerator();

        public static T Dot(Vector<T> v1, Vector<T> v2) { return v1.Dot(v2); }
        /// <summary>
        /// returns nearest point on line segment x0-x1 to point p
        /// </summary>
        /// <returns>nearest point on line segment x0-x1 to point p</returns>
        public static Vector<T> PointOnSegment(Vector<T> x0, Vector<T> x1, Vector<T> p) { return x0.PointOnSegment(x1, p); }
        /// <summary>
        /// returns time of closest point of approach of two points
        /// </summary>
        /// <returns>time of closest point of approach of two points</returns>
        public static T ClosestPointTime(Vector<T> x1, Vector<T> v1, Vector<T> x2, Vector<T> v2) { return x1.ClosestPointTime(v1, x2, v2); }
        /// <summary>
        /// returns distnace between segments s1a-s1b and s2a-s2b
        /// </summary>
        /// <returns>distnace between segments s1a-s1b and s2a-s2b</returns>
        public static T DistanceSegToSeg(Vector<T> s1a, Vector<T> s1b, Vector<T> s2a, Vector<T> s2b) { return s1a.DistanceSegToSeg(s1b, s2a, s2b); }
        public static T Distance(Vector<T> v1, Vector<T> v2) { return v1.Distance(v2); }
        public static Vector<T> Interpolate(Vector<T> start, Vector<T> end, T amount) { return start.Interpolate(end, amount); }

        public static Vector<T> operator -(Vector<T> v) { return v.Reverse(); }
        public static Vector<T> operator -(Vector<T> v1, Vector<T> v2) { return v1.Sub(v2); }
        public static Vector<T> operator +(Vector<T> v1, Vector<T> v2) { return v1.Sum(v2); }
        public static Vector<T> operator *(T p, Vector<T> v) { return v.Scale(p); }
        public static Vector<T> operator *(Vector<T> v, T p) { return v.Scale(p); }
        public static Vector<T> operator /(T p, Vector<T> v) { return v.Divide(p); }
        public static Vector<T> operator /(Vector<T> v, T p) { return v.Divide(p); }
        public static bool operator ==(Vector<T> v1, Vector<T> v2)
        {
            if (v1 is null && v2 is null) { return true; }
            if (v1 is null) { return false; }
            return v1.Equals(v2);
        }
        public static bool operator !=(Vector<T> v1, Vector<T> v2) => !(v1 == v2);

        public override string ToString()
        {
            string str = string.Empty;
            foreach (var item in values)
                str += values.ToString() + ',';
            str = str.Remove(str.Length - 2);
            return $"({str})";
        }

        public override bool Equals(object obj)
        {
            if (obj != null && obj is Vector<T> v && v.Length == Length)
            {
                for (int i = 0; i < Length; i++)
                    if (!th.Equal(values[i], v.values[i])) return false;
                return true;
            }
            return false;
        }

        public override int GetHashCode() => System.HashCode.Combine(Length, values);
    }
}