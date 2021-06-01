namespace MRL.SSL.Common.Configuration
{
    public class ConnectionConfig : ConfigBase
    {
        public new static ConnectionConfig Default { get => (ConnectionConfig)_default[(int)ConfigType.Connection]; }
        public override ConfigType Id => ConfigType.Connection;

        public string AiName { get; set; }
        public int AiPort { get; set; }
        public string VisionName { get; set; }
        public int VisionPort { get; set; }
        public string VisualizerName { get; set; }
        public int VisualizerPort { get; set; }
        public string RefName { get; set; }
        public int RefPort { get; set; }
        public bool IgnoreRefbox { get; set; }
        public int SimControlPort { get; set; }
        public int SimStatusPort { get; set; }
        public bool ConnectSim { get; set; }
        public string SimName { get; set; }

        public ConnectionConfig()
        {
        }
    }
}