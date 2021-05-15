
using MRL.SSL.Ai.Utils;
using MRL.SSL.Common;
using MRL.SSL.Common.Configuration;
using MRL.SSL.Common.Math;
using MRL.SSL.Common.SSLWrapperCommunication;
using System.Linq;

namespace MRL.SSL.Ai.MergerTracker
{
    public class WorldGenerator
    {
        private Merger merger;
        private Tracker tracker;
        public int selectedBallIndex { get; set; }
        private bool ballIndexChanged;
        private VectorF2D selectedBallLoc;
        private ObservationModel lastObsModel;
        private bool lastIsReverse;
        private SSLGeometryFieldSize lastFieldSize;

        public WorldGenerator()
        {
            merger = new Merger();
            tracker = new Tracker();
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
        private ObservationModel UpdateNotSeensHistory(ObservationModel obsModel)
        {
            if (lastObsModel != null)
            {
                foreach (var key in lastObsModel.Teammates.Keys)
                {
                    if (!obsModel.Teammates.ContainsKey(key))
                    {
                        var r = lastObsModel.Teammates[key];
                        if (r.NotSeen < MergerTrackerConfig.Default.MaxNotSeenFrames)
                        {
                            r.NotSeen++;
                            r.Vision = null;
                            r.Time += MergerTrackerConfig.Default.FramePeriod;
                            obsModel.Teammates.Add(key, r);
                        }
                    }
                }
                foreach (var key in lastObsModel.Opponents.Keys)
                {
                    if (!obsModel.Opponents.ContainsKey(key))
                    {
                        var r = lastObsModel.Opponents[key];
                        if (r.NotSeen < MergerTrackerConfig.Default.MaxNotSeenFrames)
                        {
                            r.NotSeen++;
                            r.Vision = null;
                            r.Time += MergerTrackerConfig.Default.FramePeriod;
                            obsModel.Opponents.Add(key, r);
                        }
                    }
                }
                if (obsModel.Ball == null && lastObsModel.Ball != null)
                {
                    var b = lastObsModel.Ball;
                    if (b.NotSeen < MergerTrackerConfig.Default.MaxNotSeenFrames)
                    {
                        b.NotSeen++;
                        b.Vision = null;
                        b.Time += MergerTrackerConfig.Default.FramePeriod;
                        obsModel.Ball = b;
                    }
                }

            }
            return obsModel;
        }
        private void UpdateGeometry(SSLVisionPacket packet, bool isReverse)
        {
            bool updateRequired = isReverse != lastIsReverse;
            lastIsReverse = isReverse;
            if (packet.Geometry != null)
            {
                if (((lastFieldSize == null || GameParameters.ReUpdate) && packet.Geometry.Field != null))
                {
                    lastFieldSize = packet.Geometry.Field;
                    updateRequired = true;
                    GameParameters.ReUpdate = false;
                }
                if (packet.Geometry.Calibs != null)
                    tracker.Cameras = packet.Geometry.Calibs.ToDictionary(k => k.CameraId, v => v);
            }
            if (updateRequired && lastFieldSize != null)
                GameParameters.UpdateParamsFromGeometry(lastFieldSize, isReverse);
        }
        public WorldModel GenerateWorldModel(SSLVisionPacket packet, RobotCommands commands, bool isYellow, bool isReverse)
        {
            UpdateGeometry(packet, isReverse);

            var obsModel = merger.Merge(packet, isReverse, isYellow, selectedBallLoc, ref ballIndexChanged);

            if (obsModel == null)
                return null;

            obsModel = UpdateNotSeensHistory(obsModel);
            tracker.ObserveModel(obsModel, commands);
            var model = tracker.GetEstimations(obsModel);

            model.Tracker = tracker;
            model.Commands = commands;

            lastObsModel = obsModel;
            return model;
        }

    }
}