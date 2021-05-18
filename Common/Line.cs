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

        [ProtoMember(1, IsRequired = true)]
        public VectorF2D Head { get; set; }

        [ProtoMember(2, IsRequired = true)]
        public VectorF2D Tail { get; set; }

        public float A { get; set; }

        public float B { get; set; }

        public float C { get; set; }


        public float Angle => System.MathF.Atan2(B, A);

        public Line(float a, float b, float c) { A = a; B = b; C = c; }
        public Line(VectorF2D p1, VectorF2D p2)
        {
            A = p2.Y - p1.Y;
            B = p1.X - p2.X;
            C = -(A * p1.X + B * p1.Y);
            Head = p1; Tail = p2;
        }
    }
}