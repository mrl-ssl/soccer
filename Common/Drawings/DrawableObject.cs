using System.Collections.Generic;
using MRL.SSL.Common.Math;
using ProtoBuf;

namespace MRL.SSL.Common.Drawings
{
    public class DrawableObject
    {
        [ProtoMember(1)]
        public int StrokeColor { get; set; }

        [ProtoMember(2, IsRequired = true)]
        public int FillColor { get; set; }

        [ProtoMember(3, IsRequired = true)]
        public float StrokeWidth { get; set; }

        [ProtoMember(4)]
        public float? FontSize { get; set; }

        [ProtoMember(5, IsRequired = true)]
        public bool Fill { get; set; }

        [ProtoMember(6, IsRequired = true)]
        public float Opacity { get; set; }


        public enum DrawableType
        {
            Circle = 0,
            Line,
            String,
            Path,
            Region
        }

        public DrawableType Type { get; set; }
        public Circle Circle { get; set; }
        public List<Vector2D<float>> Region { get; set; }
        public List<Vector2D<float>> Path { get; set; }
        public Line Line { get; set; }
        public string Text { get; set; }
    }
}