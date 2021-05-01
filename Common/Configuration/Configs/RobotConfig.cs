namespace MRL.SSL.Common.Configuration
{
    public class RobotConfig : ConfigBase
    {
        public new static RobotConfig Default { get => (RobotConfig)_default[(int)ConfigType.Robot]; }
        public override ConfigType Id => ConfigType.Robot;

        public float Radius { get; set; }
        public float Heigth { get; set; }
    }
}