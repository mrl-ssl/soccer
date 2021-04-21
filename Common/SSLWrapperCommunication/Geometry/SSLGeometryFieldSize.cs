using System.Collections.Generic;
using ProtoBuf;

namespace MRL.SSL.Common.SSLWrapperCommunication
{
    [ProtoContract]
    public class SSLGeometryFieldSize
    {
        [ProtoMember(1)]
        public int FieldLength { get; set; }

        [ProtoMember(2)]
        public int FieldWidth { get; set; }

        [ProtoMember(3)]
        public int GoalWidth { get; set; }

        [ProtoMember(4)]
        public int GoalDepth { get; set; }

        [ProtoMember(5)]
        public int BoundaryWidth { get; set; }

        [ProtoMember(6)]
        public List<SSLFieldLineSegment> FieldLines { get; set; }

        [ProtoMember(7)]
        public List<SSLFieldCircularArc> FieldArcs { get; set; }

        [ProtoMember(8)]
        public int PenaltyAreaDepth { get; set; }

        [ProtoMember(9)]
        public int PenaltyAreaWidth { get; set; }

    }
}