namespace MRL.SSL.Common.Configuration
{
    public class ConnectionConfig : ConfigBase
    {
        public static ConnectionConfig Default { get => (ConnectionConfig)_default; }
        private string aiName;
        private int aiPort;
        private string visionName;
        private int visionPort;
        private string visualizerName;
        private int visualizerPort;

        public string AiName { get => aiName; set => aiName = value; }
        public int AiPort { get => aiPort; set => aiPort = value; }
        public string VisionName { get => visionName; set => visionName = value; }
        public int VisionPort { get => visionPort; set => visionPort = value; }
        public string VisualizerName { get => visualizerName; set => visualizerName = value; }
        public int VisualizerPort { get => visualizerPort; set => visualizerPort = value; }

        public ConnectionConfig()
        {
        }
    }
}