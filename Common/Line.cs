using MRL.SSL.Common.Math;
using ProtoBuf;

namespace MRL.SSL.Common
{
    /// <summary>
    /// Represent line as Ax + By + C = 0
    /// </summary>
    [ProtoContract]
    public class Line
    {
        public Vector2D<float> Head { get; set; }
        public Vector2D<float> Tail { get; set; }

        [ProtoMember(1, IsRequired = true)]
        public float A { get; set; }

        [ProtoMember(2, IsRequired = true)]
        public float B { get; set; }

        [ProtoMember(3, IsRequired = true)]
        public float C { get; set; }

        public float Angle => System.MathF.Atan2(B, A);

        public Line(float a, float b, float c) { A = a; B = b; C = c; }
        public Line(Vector2D<float> p1, Vector2D<float> p2)
        {
            A = p2.Y - p1.Y;
            B = p1.X - p2.X;
            C = -(A * p1.X + B * p1.Y);
            Head = p1; Tail = p2;
        }
    }
}