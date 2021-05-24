using MRL.SSL.Common.Math;
using ProtoBuf;

namespace MRL.SSL.Common
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
        public uint StrokeColor { get; set; }

        [ProtoMember(2, IsRequired = false)]
        public uint? FillColor { get; set; }

        [ProtoMember(3, IsRequired = true)]
        public float StrokeWidth { get; set; }

        [ProtoMember(4)]
        public int? FontSize { get; set; }

        [ProtoMember(5, IsRequired = true)]
        public DrawableType Type { get; set; }


        private DiscriminatedUnionObject __pbn__event;

        [ProtoMember(6)]
        public Circle Circle
        {
            get => __pbn__event.Is(6) ? ((Circle)__pbn__event.Object) : default;
            set => __pbn__event = new DiscriminatedUnionObject(6, value);
        }
        [ProtoMember(7)]
        public Region Region
        {
            get => __pbn__event.Is(7) ? ((Region)__pbn__event.Object) : default;
            set => __pbn__event = new DiscriminatedUnionObject(7, value);
        }

        [ProtoMember(8)]
        public Region Path
        {
            get => __pbn__event.Is(8) ? ((Region)__pbn__event.Object) : default;
            set => __pbn__event = new DiscriminatedUnionObject(8, value);
        }

        [ProtoMember(9)]
        public Line Line
        {
            get => __pbn__event.Is(9) ? ((Line)__pbn__event.Object) : default;
            set => __pbn__event = new DiscriminatedUnionObject(9, value);
        }

        [ProtoMember(10)]
        public DrawableString String
        {
            get => __pbn__event.Is(10) ? ((DrawableString)__pbn__event.Object) : default;
            set => __pbn__event = new DiscriminatedUnionObject(10, value);
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