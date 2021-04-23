namespace MRL.SSL.Common.Math.Helpers
{
    public class DoubleMathHelper : IGenericMathHelper<double>
    {
        System.Random random = new System.Random(System.DateTime.Now.Millisecond);

        public double Zero => 0d;
        public double One => 1d;
        public double NegativeOne => -1d;

        public double Random() => random.Next();
        public double Random(double min, double max) => random.NextDouble() * (max - min) + min;
        public double Abs(double a) => System.Math.Abs(a);

        public double Sum(double a, double b) => a + b;
        public double Sub(double a, double b) => a - b;
        public double Dvide(double a, double b) => a / b;
        public double Multi(double a, double b) => a * b;

        public bool Equal(double a, double b) => System.Math.Abs(a - b) <= MathHelper.EpsilonF;
        public bool Greater(double a, double b) => a > b;
        public bool GreaterOrEqual(double a, double b) => a >= b;
        public bool Less(double a, double b) => a < b;
        public bool LessOrEqual(double a, double b) => a <= b;
    }
}