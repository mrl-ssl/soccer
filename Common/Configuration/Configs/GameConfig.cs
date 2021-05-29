namespace MRL.SSL.Common.Configuration
{
    public class GameConfig : ConfigBase
    {
        public new static GameConfig Default { get => (GameConfig)_default[(int)ConfigType.Game]; }
        public override ConfigType Id => ConfigType.Game;
        public bool Debug { get; set; }
        public bool IsFieldInverted { get; set; }
        public bool OurMarkerIsYellow { get; set; }
        public float StopBallRadi { get; set; }

        public GameConfig()
        {

        }
    }
}