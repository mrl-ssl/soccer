namespace MRL.SSL.Common.Configuration
{
    public class MergerTrackerConfig : ConfigBase
    {
        public new static MergerTrackerConfig Default { get => (MergerTrackerConfig)_default[(int)ConfigType.MergerTracker]; }
        public override ConfigType Id => ConfigType.MergerTracker;
        public byte[] AvailableCameras { get; set; }
        public MergerTrackerConfig()
        {
        }
    }
}