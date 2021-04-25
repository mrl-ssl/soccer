namespace MRL.SSL.Common.Math.Helpers
{
    public class DoubleMathHelper : IGenericMathHelper<double>
    {
        System.Random random = new System.Random(System.DateTime.Now.Millisecond);

        public double Zero => 0d;
        public double One => 1d;
        public double NegativeOne => -1d;
        public double PI => System.Math.PI;

        public double Random() => random.NextDouble();
        public double Random(double min, double max) => random.NextDouble() * (max - min) + min;
        public double Max(double a, double b) => System.Math.Max(a, b);
        public double Bound(double x, double min, double max) => MathHelper.Bound(x, min, max);
        public double Abs(double a) => System.Math.Abs(a);
        public double Pow(double x, double y) => System.Math.Pow(x, y);
        public double Square(double x) => x * x;
        public double Times(int times, double x) => times * x;
        public double Times(float times, double x) => times * x;
        public double Sqrt(double a) => System.Math.Sqrt(a);
        public double Cos(double a) => System.Math.Cos(a);
        public double ACos(double a) => System.Math.Acos(a);
        public double Sin(double a) => System.Math.Sin(a);
        public double Atan2(double y, double x) => System.Math.Atan2(y, x);
        public double Radian2Degree(double radian) => radian * 180d / PI;
        public double Sign(double x) => System.Math.Sign(x);
        public double Negative(double x) => -x;

        public double Sum(double a, double b) => a + b;
        public double Sub(double a, double b) => a - b;
        public double Divide(double a, double b) => a / b;
        public double Multi(double a, double b) => a * b;

        public bool Equal(double a, double b) => System.Math.Abs(a - b) <= MathHelper.Epsilon;
        public bool EqualZero(double a) => System.Math.Abs(a) <= MathHelper.Epsilon;
        public bool Greater(double a, double b) => a > b && !Equal(a, b);
        public bool GreaterThanZero(double a) => a > 0d && !EqualZero(a);
        public bool GreaterOrEqual(double a, double b) => a > b || Equal(a, b);
        public bool GreaterOrEqualThanZero(double a) => a > 0d || EqualZero(a);
        public bool Less(double a, double b) => a < b && !Equals(a, b);
        public bool LessThanZero(double a) => a < 0d && !EqualZero(a);
        public bool LessOrEqual(double a, double b) => a < b || Equal(a, b);
        public bool LessOrEqualThanZero(double a) => a < 0d || EqualZero(a);
    }
}