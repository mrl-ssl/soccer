using System.Collections.Generic;
using MRL.SSL.Common.Math;
using ProtoBuf;

namespace MRL.SSL.Common.Drawings
{
    [ProtoContract]
    public enum DrawableType
    {
        Circle = 0,
        Line,
        String,
        Path,
        Region
    }

    [ProtoContract]
    public class DrawableObject
    {
        [ProtoMember(1, IsRequired = true)]
        public string StrokeColor { get; set; }

        [ProtoMember(2)]
        public string FillColor { get; set; }

        [ProtoMember(3, IsRequired = true)]
        public float StrokeWidth { get; set; }

        [ProtoMember(4)]
        public string FontSize { get; set; }

        [ProtoMember(5, IsRequired = true)]
        public bool Fill { get; set; }

        [ProtoMember(6, IsRequired = true)]
        public float Opacity { get; set; }



        [ProtoMember(7, IsRequired = true)]
        public DrawableType Type { get; set; }


        private DiscriminatedUnionObject __pbn__event;

        [ProtoMember(8)]
        public Circle Circle
        {
            get => __pbn__event.Is(8) ? ((Circle)__pbn__event.Object) : default;
            set => __pbn__event = new DiscriminatedUnionObject(8, value);
        }
        [ProtoMember(9)]
        public List<VectorF2D> Region
        {
            get => __pbn__event.Is(9) ? ((List<VectorF2D>)__pbn__event.Object) : default;
            set => __pbn__event = new DiscriminatedUnionObject(9, value);
        }

        [ProtoMember(10)]
        public List<VectorF2D> Path
        {
            get => __pbn__event.Is(10) ? ((List<VectorF2D>)__pbn__event.Object) : default;
            set => __pbn__event = new DiscriminatedUnionObject(10, value);
        }

        [ProtoMember(11)]
        public Line Line
        {
            get => __pbn__event.Is(11) ? ((Line)__pbn__event.Object) : default;
            set => __pbn__event = new DiscriminatedUnionObject(11, value);
        }

        [ProtoMember(12)]
        public DrawableString String
        {
            get => __pbn__event.Is(12) ? ((DrawableString)__pbn__event.Object) : default;
            set => __pbn__event = new DiscriminatedUnionObject(12, value);
        }
    }

    [ProtoContract]
    public class DrawableString
    {
        [ProtoMember(1, IsRequired = true)]
        public VectorF2D Position { get; set; }

        [ProtoMember(2, IsRequired = true)]
        public string Text { get; set; }
    }
}