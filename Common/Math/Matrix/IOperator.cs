using System;
using System.Runtime.CompilerServices;

namespace MRL.SSL.Common.Math
{
    public interface IOperator<T>
    {
        T Zero { get; }
        T One { get; }
        T NegativeOne { get; }
        bool IsClass { get; }

        T RandomValue(T minVal, T maxVal);

        T Abs(T a);
        T Add(T a, T b);
        T Sub(T a, T b);
        T Multiply(T a, T b);
        T Dvide(T a, T b);
        bool Equal(T a, T b);
        bool Greater(T a, T b);
        bool GreaterOrEqual(T a, T b);
        bool Less(T a, T b);
        bool LessOrEqual(T a, T b);
    }
    public class MatrixOperators
    {
        public static FloatOperator FloatOperator = new FloatOperator();
        public static DoubleOperator DoubleOperator = new DoubleOperator();
        public static IntOperator IntOperator = new IntOperator();

    }
    public class IntOperator : IOperator<int>
    {
        private Random random;
        public int Zero { get; }
        public int One { get; }
        public int NegativeOne { get; }
        public bool IsClass { get; }

        public IntOperator() { Zero = 0; One = 1; NegativeOne = -1; IsClass = true; }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int RandomValue(int minVal, int maxVal)
        {
            if (random == null)
                random = new Random(DateTime.Now.Millisecond);
            return random.Next(minVal, maxVal);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int Abs(int a) => System.Math.Abs(a);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int Add(int a, int b) => a + b;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int Sub(int a, int b) => a - b;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int Multiply(int a, int b) => a * b;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int Dvide(int a, int b) => a / b;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equal(int a, int b) => a.Equals(b);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Greater(int a, int b) => a > b;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool GreaterOrEqual(int a, int b) => a >= b;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Less(int a, int b) => a < b;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool LessOrEqual(int a, int b) => a <= b;
    }

    public class FloatOperator : IOperator<float>
    {
        private Random random;
        public float Zero { get; }
        public float One { get; }
        public float NegativeOne { get; }
        public bool IsClass { get; }

        public FloatOperator() { Zero = 0F; One = 1F; NegativeOne = -1F; IsClass = true; random = new Random(); }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public float RandomValue(float minVal, float maxVal)
        {
            if (random == null)
                random = new Random(DateTime.Now.Millisecond);
            return (float)(random.NextDouble() * (maxVal - minVal) + minVal);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public float Abs(float a) => MathF.Abs(a);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public float Add(float a, float b) => a + b;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public float Sub(float a, float b) => a - b;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public float Multiply(float a, float b) => a * b;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public float Dvide(float a, float b) => a / b;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equal(float a, float b) => MathF.Abs(a - b) < MathHelper.EpsilonF;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Greater(float a, float b) => a > b;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool GreaterOrEqual(float a, float b) => a >= b;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Less(float a, float b) => a < b;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool LessOrEqual(float a, float b) => a <= b;
    }

    public class DoubleOperator : IOperator<double>
    {
        private Random random;
        public double Zero { get; }
        public double One { get; }
        public double NegativeOne { get; }
        public bool IsClass { get; }

        public DoubleOperator() { Zero = 0D; One = 1D; NegativeOne = -1D; IsClass = true; random = new Random(); }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public double RandomValue(double minVal, double maxVal)
        {
            if (random == null)
                random = new Random(DateTime.Now.Millisecond);
            return random.NextDouble() * (maxVal - minVal) + minVal;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public double Abs(double a) => System.Math.Abs(a);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public double Add(double a, double b) => a + b;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public double Sub(double a, double b) => a - b;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public double Multiply(double a, double b) => a * b;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public double Dvide(double a, double b) => a / b;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equal(double a, double b) => System.Math.Abs(a - b) < MathHelper.EpsilonF;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Greater(double a, double b) => a > b;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool GreaterOrEqual(double a, double b) => a >= b;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Less(double a, double b) => a < b;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool LessOrEqual(double a, double b) => a <= b;
    }

}