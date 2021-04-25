namespace MRL.SSL.Common.Math.Helpers
{
    public class IntMathHelper : IGenericMathHelper<int>
    {
        System.Random random = new System.Random(System.DateTime.Now.Millisecond);

        public int Zero => 0;
        public int One => 1;
        public int NegativeOne => -1;
        public int PI => 3;

        public int Random() => random.Next();
        public int Random(int min, int max) => random.Next(min, max);
        public int Max(int a, int b) => System.Math.Max(a, b);
        public int Bound(int x, int min, int max) => MathHelper.Bound(x, min, max);
        public int Abs(int a) => System.Math.Abs(a);
        public int Pow(int x, int y) => (int)(System.Math.Pow(x, y));
        public int Square(int x) => x * x;
        public int Times(int times, int x) => times * x;
        public int Times(float times, int x) => (int)(times * x);
        public int Sqrt(int a) => (int)(System.Math.Sqrt(a));
        public int Cos(int a) => (int)(System.Math.Cos(a));
        public int ACos(int a) => (int)(System.Math.Acos(a));
        public int Sin(int a) => (int)(System.Math.Sin(a));
        public int Atan2(int y, int x) => (int)(System.Math.Atan2(y, x));
        public int Radian2Degree(int radian) => radian * 180 / PI;
        public int Sign(int x) => System.Math.Sign(x);
        public int Negative(int x) => -x;

        public int Sum(int a, int b) => a + b;
        public int Sub(int a, int b) => a - b;
        public int Divide(int a, int b) => a / b;
        public int Multi(int a, int b) => a * b;

        public bool Equal(int a, int b) => a == b;
        public bool EqualZero(int a) => a == 0;
        public bool Greater(int a, int b) => a > b;
        public bool GreaterThanZero(int a) => a > 0;
        public bool GreaterOrEqual(int a, int b) => a >= b;
        public bool GreaterOrEqualThanZero(int a) => a >= 0;
        public bool Less(int a, int b) => a < b;
        public bool LessThanZero(int a) => a < 0;
        public bool LessOrEqual(int a, int b) => a <= b;
        public bool LessOrEqualThanZero(int a) => a <= 0;
    }
}