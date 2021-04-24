
using MRL.SSL.Common;
using MRL.SSL.Common.Configuration;
using MRL.SSL.Common.Math;
using MRL.SSL.Common.SSLWrapperCommunication;

namespace MRL.SSL.Ai.MergerTracker
{
    public class WorldGenerator
    {
        private Merger merger;
        public int selectedBallIndex { get; set; }
        private bool ballIndexChanged;
        private VectorF2D selectedBallLoc;
        public WorldGenerator()
        {
            merger = new Merger();
        }
        public void setBallIndex(int? ballIndex, VectorF2D pos)
        {
            if (ballIndex.HasValue)
            {
                selectedBallIndex = ballIndex.Value;
                ballIndexChanged = true;
                selectedBallLoc = pos;
            }
        }
        public WorldModel GenerateWorldModel(SSLWrapperPacket packet, bool isYellow, bool isReverse)
        {
            if (packet != null && packet.Geometry != null)
                FieldConfig.Default.UpdateFromGeometry(packet.Geometry);

            var world = merger.Merge(packet, isReverse, isYellow, selectedBallLoc, ref ballIndexChanged);
            if (world != null)
            {

            }

            return world;
        }
    }
}