using System.Collections;
using System.Collections.Generic;
using MRL.SSL.Common.Math.Helpers;

namespace MRL.SSL.Common.Math
{
    public class Vector<T> : IEnumerable<T>
    {
        protected static readonly IGenericMathHelper<T> type_helper = MathHelper.GetGenericMathHelper<T>();
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

        /// <summary>
        /// Fill vector by given value
        /// </summary>
        public void FillBy(T x)
        {
            for (int i = 0; i < Length; i++)
                values[i] = x;
        }

        /// <returns>sqrt(x0^2 + x1^2 + ...)</returns>
        public T Size() => type_helper.Sqrt(SquareSize());

        /// <summary>
        /// vector size without radical (size^2)
        /// </summary>
        /// <returns>size^2</returns>
        public T SquareSize()
        {
            T result = type_helper.Zero;
            for (int i = 0; i < Length; i++)
                result = type_helper.Sum(result, type_helper.Multi(values[i], values[i]));
            return type_helper.Equal(result, type_helper.Zero) ? type_helper.Zero : result;
        }

        /// <returns>Normalized of this vector</returns>
        public Vector<T> Norm()
        {
            Vector<T> r = Clone();
            r.NormSelf();
            return r;
        }

        /// <returns>Normalized of this vector with new length</returns>
        public Vector<T> NormTo(T newLength)
        {
            Vector<T> r = Clone();
            r.NormSelf(newLength);
            return r;
        }

        /// <summary>
        /// Normalized this vector
        /// </summary>
        public void NormSelf()
        {
            T size = Size();
            if (type_helper.Equal(size, type_helper.Zero))
                FillBy(type_helper.Zero);
            else
                for (int i = 0; i < Length; i++)
                    values[i] = type_helper.Divide(values[i], size);
        }

        /// <summary>
        /// Normalized this vector to new length
        /// </summary>
        public void NormSelf(T newLength)
        {
            T size = Size();
            if (type_helper.Equal(size, type_helper.Zero))
                FillBy(type_helper.Zero);
            else
                for (int i = 0; i < Length; i++)
                    values[i] = type_helper.Multi(values[i], type_helper.Divide(newLength, size));
        }

        /// <summary>
        /// if length of src != dist min of those two is used
        /// </summary>
        public T Dot(Vector<T> v)
        {
            T result = type_helper.Zero;
            for (int i = 0; i < System.Math.Min(Length, v.Length); i++)
                result = type_helper.Sum(result, type_helper.Multi(values[i], v.values[i]));
            return result;
        }

        /// <summary>
        /// its limited to this matrix dimention and higher dimentions ignored
        /// </summary>
        public T Distance(Vector<T> v) => type_helper.Sqrt(SquareDistance(v));

        /// <summary>
        /// return distance without radical (distance^2)
        /// its limited to this matrix dimention and higher dimentions ignored
        /// </summary>
        public T SquareDistance(Vector<T> v)
        {
            T result = type_helper.Zero;
            for (int i = 0; i < System.Math.Min(Length, v.Length); i++)
            {
                T diff = type_helper.Sub(values[i], v.values[i]);
                result = type_helper.Sum(result, type_helper.Square(diff));
            }
            return result;
        }

        /// <summary>
        /// Create new vector wich is result of extending this vector by given scales (x0 + scales0, x1 + scales1, ...)
        /// if scales length != this vector length it use min(this length, scales length)
        /// </summary>
        public Vector<T> Extend(T[] scales)
        {
            Vector<T> r = Clone();
            r.ExtendSelf(scales);
            return r;
        }

        /// <summary>
        /// Extend this vector by given scales (x0 + scales0, x1 + scales1, ...)
        /// if scales length != this vector length it use min(this length, scales length)
        /// </summary>
        public void ExtendSelf(T[] scales)
        {
            for (int i = 0; i < System.Math.Min(Length, scales.Length); i++)
                values[i] = type_helper.Sum(values[i], scales[i]);
        }

        public Vector<T> Interpolate(Vector<T> end, T amount)
        {
            Vector<T> r = new Vector<T>(Length);
            for (int i = 0; i < System.Math.Min(Length, end.Length); i++)
            {
                var t1 = type_helper.Multi(values[i], type_helper.Sub(type_helper.One, amount)); //v[i]*(1-amount)
                var t2 = type_helper.Multi(end.values[i], amount); //end[i] * amount
                r.values[i] = type_helper.Sum(t1, t2);
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
                values[i] = type_helper.Abs(values[i]);
        }

        public Vector<T> Max(Vector<T> v)
        {
            int l = System.Math.Min(Length, v.Length);
            Vector<T> r = new Vector<T>(l);
            for (int i = 0; i < l; i++)
                r.values[i] = type_helper.Max(values[i], v.values[i]);
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
                values[i] = type_helper.Bound(values[i], low, high);
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
                values[i] = type_helper.Multi(values[i], s);
        }

        /// <returns>this + v</returns>
        public Vector<T> Sum(Vector<T> v)
        {
            int l = System.Math.Min(Length, v.Length);
            Vector<T> r = new Vector<T>(l);
            for (int i = 0; i < l; i++)
                r.values[i] = type_helper.Sum(values[i], v.values[i]);
            return r;
        }

        /// <returns>this - v</returns>
        public Vector<T> Sub(Vector<T> v)
        {
            int l = System.Math.Min(Length, v.Length);
            Vector<T> r = new Vector<T>(l);
            for (int i = 0; i < l; i++)
                r.values[i] = type_helper.Sub(values[i], v.values[i]);
            return r;
        }

        /// <returns>(x0/c, x1/c, ...)</returns>
        public Vector<T> Divide(T c)
        {
            Vector<T> r = new Vector<T>(Length);
            for (int i = 0; i < Length; i++)
                r.values[i] = type_helper.Divide(values[i], c);
            return r;
        }

        /// <returns>(-x0, -x1, ...)</returns>
        public Vector<T> Reverse()
        {
            Vector<T> r = Clone();
            for (int i = 0; i < Length; i++)
                r.values[i] = type_helper.Multi(values[i], type_helper.NegativeOne);
            return r;
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
            if (type_helper.LessOrEqual(f, type_helper.Zero)) return this;         // also handles this=x1 case

            l = sx.SquareSize();
            if (type_helper.GreaterOrEqual(f, l)) return x1;

            r = this + sx * (type_helper.Divide(f, l));

            return r;
        }

        public IEnumerator<T> GetEnumerator() => ((IEnumerable<T>)values).GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => values.GetEnumerator();

        public static T Dot(Vector<T> v1, Vector<T> v2) { return v1.Dot(v2); }
        /// <summary>
        /// returns nearest point on line segment x0-x1 to point p
        /// </summary>
        /// <returns>nearest point on line segment x0-x1 to point p</returns>
        public static Vector<T> PointOnSegment(Vector<T> x0, Vector<T> x1, Vector<T> p) { return x0.PointOnSegment(x1, p); }

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
                    if (!type_helper.Equal(values[i], v.values[i])) return false;
                return true;
            }
            return false;
        }

        public override int GetHashCode() => System.HashCode.Combine(Length, values);
    }
}