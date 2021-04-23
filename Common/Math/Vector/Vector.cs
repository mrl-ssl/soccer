using System.Collections;
using System.Collections.Generic;
using MRL.SSL.Common.Math.Helpers;

namespace MRL.SSL.Common.Math
{
    public abstract class Vector<T> : IEnumerable<T>
    {
        protected readonly IGenericMathHelper<T> type_helper;
        protected T[] values;

        public int Length { get => values.Length; }
        public T this[int index]
        {
            get => values[index];
            set => values[index] = value;
        }

        public Vector(int length)
        {
            type_helper = MathHelper.CreateGenericMathHelper<T>();
            values = new T[length];
        }
        public Vector(T[] data)
        {
            type_helper = MathHelper.CreateGenericMathHelper<T>();
            values = data;
        }
        public Vector(int length, IGenericMathHelper<T> type_helper)
        {
            this.type_helper = type_helper;
            values = new T[length];
        }
        public Vector(T[] data,IGenericMathHelper<T> type_helper)
        {
            this.type_helper = type_helper;
            values = data;
        }

        public abstract T Dot(Vector<T> v);


        public IEnumerator<T> GetEnumerator() => ((IEnumerable<T>)values).GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => values.GetEnumerator();
    }
}