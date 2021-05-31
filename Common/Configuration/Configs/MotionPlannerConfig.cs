using ProtoBuf;

namespace MRL.SSL.Common.Configuration
{
    [ProtoContract]
    public class MotionPlannerConfig : ConfigBase
    {
        public new static MotionPlannerConfig Default { get => (MotionPlannerConfig)_default[(int)ConfigType.MotionPlanner]; }
        public override ConfigType Id => ConfigType.MotionPlanner;
        public bool UseERRT { get; set; }
        public float GoalProbability { get; set; }
        public float WayPointProbability { get; set; }
        public int MaxNodes { get; set; }
        public int MaxTries { get; set; }
        public int MaxRepulseTries { get; set; }
        public int NumWayPoints { get; set; }
        public float ExtendSize { get; set; }
        public float SqNearDistTresh { get; set; }
        public float AngleWeight { get; set; }
        public float SpeedWeight { get; set; }
        public float CountWeight { get; set; }
        public float LengthWeight { get; set; }
        public float MetZoneBaseWeight { get; set; }
        public float MetBaseWeight { get; set; }
        public float NotReachedBaseWeight { get; set; }
        public float NotReachedTresh { get; set; }
        public float DistanceWeight { get; set; }
        public float RefindPathWeightTresh { get; set; }
    }
}