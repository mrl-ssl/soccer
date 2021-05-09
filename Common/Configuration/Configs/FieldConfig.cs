using MRL.SSL.Common.Math;
using ProtoBuf;

namespace MRL.SSL.Common.Configuration
{
    [ProtoContract]
    public class FieldConfig : ConfigBase
    {
        public new static FieldConfig Default { get => (FieldConfig)_default[(int)ConfigType.Field]; }
        public override ConfigType Id => ConfigType.Field;

        public float FieldLength { get; set; }
        public float FieldWidth { get; set; }
        public float GoalWidth { get; set; }
        public float GoalDepth { get; set; }
        public float PenaltyAreaDepth { get; set; }
        public float PenaltyAreaWidth { get; set; }

        [ProtoMember(1, IsRequired = true)]
        public float BoundaryWidth { get; set; }

        [ProtoMember(2, IsRequired = true)]
        public float Thickness { get; set; }

        [ProtoMember(3, IsRequired = true)]
        public float CenterCircleRadius { get; set; }

        [ProtoMember(4)]
        public VectorF2D OurLeftCorner { get; set; }

        [ProtoMember(5)]
        public VectorF2D OurRightCorner { get; set; }

        [ProtoMember(6)]
        public VectorF2D OppLeftCorner { get; set; }

        [ProtoMember(7)]
        public VectorF2D OppRightCorner { get; set; }

        [ProtoMember(8)]
        public VectorF2D OurGoalCenter { get; set; }

        [ProtoMember(9)]
        public VectorF2D OurGoalRight { get; set; }

        [ProtoMember(10)]
        public VectorF2D OurGoalLeft { get; set; }

        [ProtoMember(11)]
        public VectorF2D OurGoalDepthRight { get; set; }

        [ProtoMember(12)]
        public VectorF2D OurGoalDepthLeft { get; set; }

        [ProtoMember(13)]
        public VectorF2D OurPenaltyBackRight { get; set; }

        [ProtoMember(14)]
        public VectorF2D OurPenaltyBackLeft { get; set; }

        [ProtoMember(15)]
        public VectorF2D OurPenaltyRearRight { get; set; }

        [ProtoMember(16)]
        public VectorF2D OurPenaltyRearLeft { get; set; }

        [ProtoMember(17)]
        public VectorF2D OppGoalCenter { get; set; }

        [ProtoMember(18)]
        public VectorF2D OppGoalRight { get; set; }

        [ProtoMember(19)]
        public VectorF2D OppGoalLeft { get; set; }

        [ProtoMember(20)]
        public VectorF2D OppGoalDepthRight { get; set; }

        [ProtoMember(21)]
        public VectorF2D OppGoalDepthLeft { get; set; }

        [ProtoMember(22)]
        public VectorF2D OppPenaltyBackRight { get; set; }

        [ProtoMember(23)]
        public VectorF2D OppPenaltyBackLeft { get; set; }

        [ProtoMember(24)]
        public VectorF2D OppPenaltyRearRight { get; set; }

        [ProtoMember(25)]
        public VectorF2D OppPenaltyRearLeft { get; set; }

        public FieldConfig()
        {
        }

    }
}