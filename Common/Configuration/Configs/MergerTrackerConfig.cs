namespace MRL.SSL.Common.Configuration
{
    public class MergerTrackerConfig : ConfigBase
    {
        public new static MergerTrackerConfig Default { get => (MergerTrackerConfig)_default[(int)ConfigType.MergerTracker]; }
        public override ConfigType Id => ConfigType.MergerTracker;
        public byte[] AvailableCameras { get; set; }
        public byte MaxCameraCount { get; set; }
        public byte MaxRobotId { get; set; }
        public byte TeamsCount { get; set; }
        public byte AffinityPersist { get; set; }
        public int FrameRate { get; set; }
        public float FramePeriod { get { return 1.0f / FrameRate; } }
        public float LastBallMaxDistance { get; set; }
        public int FramesToDetectCameras { get; set; }
        public float BallRadi { get; set; }
        public MergerTrackerConfig()
        {
        }

    }
}