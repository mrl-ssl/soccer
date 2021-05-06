
using MRL.SSL.Ai.Utils;
using MRL.SSL.Common;
using MRL.SSL.Common.Configuration;
using MRL.SSL.Common.Math;
using MRL.SSL.Common.SSLWrapperCommunication;
using MRL.SSL.Common.Utils.Extensions;
using System.Collections.Generic;
using System.Linq;


namespace MRL.SSL.Ai.MergerTracker
{
    internal class Merger
    {
        bool[] camerasSeen;
        double[] times;
        int numCameras, numCamerasSeen;
        uint frames;
        byte[] lastAvailableCameras;
        ObjectMerger[,] robots;
        ObjectMerger ball;
        private IDictionary<uint, List<SSLDetectionBall>> balls;
        private double lastCaptureTime;

        public Merger()
        {
            camerasSeen = new bool[MergerTrackerConfig.Default.MaxCameraCount];
            times = new double[MergerTrackerConfig.Default.MaxCameraCount];
            numCameras = 0;
            numCamerasSeen = 0;
            frames = 0;
            ball = new ObjectMerger();
            balls = new Dictionary<uint, List<SSLDetectionBall>>();
            robots = new ObjectMerger[MergerTrackerConfig.Default.TeamsCount, MergerTrackerConfig.Default.MaxRobotId].Populate();
        }
        private bool ValidateCameraSeen(SSLWrapperPacket packet)
        {
            var camera = packet.Detection.CameraId;
            var availableCameras = MergerTrackerConfig.Default.AvailableCameras;

            //if configurations changed reset available camera detection params
            if (lastAvailableCameras != null && (!lastAvailableCameras.All(a => availableCameras.Contains(a))
                || !availableCameras.All(a => lastAvailableCameras.Contains(a))))
            {
                numCameras = 0;
                frames = 0;
                for (int i = 0; i < camerasSeen.Length; i++)
                {
                    camerasSeen[i] = false;
                }
                numCamerasSeen = 0;
            }

            lastAvailableCameras = MergerTrackerConfig.Default.AvailableCameras;

            if (!availableCameras.Contains((byte)camera))
                return false;

            if (numCameras <= 0)
            {
                if (!camerasSeen[camera])
                {
                    numCamerasSeen++;
                    camerasSeen[camera] = true;
                }

                if (frames++ >= MergerTrackerConfig.Default.FramesToDetectCameras)
                {
                    numCameras = numCamerasSeen;

                    for (int i = 0; i < camerasSeen.Length; i++)
                    {
                        camerasSeen[i] = false;
                    }
                    numCamerasSeen = 0;
                }
                else
                {
                    return false;
                }
            }

            //detect seen cameras
            if (!camerasSeen[camera])
            {
                numCamerasSeen++;
                camerasSeen[camera] = true;
            }
            return true;
        }
        private void UpdateObservations(SSLDetectionFrame d, VectorF2D selectedBall, bool isReverse, bool selectedBallChanged)
        {
            var camera = d.CameraId;
            times[camera] = d.CaptureTime;
            for (int i = 0; i < d.BlueRobots.Count; i++)
            {
                var r = d.BlueRobots[i];
                var obs = robots[0, r.Id.HasValue ? r.Id.Value : i].Observations[camera];
                obs.IsValid = true;
                obs.Confidence = r.Confidence;
                obs.Location = new VectorF2D(r.X, r.Y);
                obs.Angle = r.Orientation.HasValue ? r.Orientation.Value : 0;
            }
            for (int i = 0; i < d.YellowRobots.Count; i++)
            {
                var r = d.YellowRobots[i];
                var obs = robots[1, r.Id.HasValue ? r.Id.Value : i].Observations[camera];
                obs.IsValid = true;
                obs.Confidence = r.Confidence;
                obs.Location = new VectorF2D(r.X, r.Y);
                obs.Angle = r.Orientation.HasValue ? r.Orientation.Value : 0;
            }

            // take the ball from this camera that is closest to the last position of the
            // ball and not too far away
            float maxDist = float.MaxValue;
            VectorF2D lastBallLoc = new();

            if (ball.Affinity >= 0)
            {
                var lastBall = ball.Observations[ball.Affinity];
                if (lastBall.LastValid < MergerTrackerConfig.Default.AffinityPersist)
                {
                    maxDist = MergerTrackerConfig.Default.LastBallMaxDistance
                                * MergerTrackerConfig.Default.FramePeriod * lastBall.LastValid;

                    lastBallLoc = lastBall.Location;
                }
            }
            if (d.Balls.Count > 0)
            {
                bool found = false;
                float minDist = float.MaxValue;
                SSLDetectionBall closest = new();
                foreach (var b in d.Balls)
                {
                    if (!balls.ContainsKey(camera))
                        balls[camera] = new List<SSLDetectionBall>();
                    balls[camera].Add(b);
                    if (selectedBallChanged)
                        lastBallLoc = (VectorF2D)selectedBall.ToVisionCoordinate(isReverse);

                    float dist = (float)lastBallLoc.Distance(new VectorF2D(b.X, b.Y));
                    if (dist < minDist && dist < maxDist)
                    {
                        if (GameHelpers.IsInField((VectorF2D)(new VectorF2D(b.X, b.Y).ToAiCoordinate(isReverse)), 0.4f))
                        {
                            closest = b;
                            minDist = dist;
                            found = true;
                        }
                    }
                }
                if (found)
                {
                    var obs = ball.Observations[camera];
                    obs.IsValid = true;
                    obs.Confidence = closest.Confidence;
                    obs.Location = new VectorF2D(closest.X, closest.Y);
                }

            }

        }
        private void UpdateSelectedBall(VectorF2D selectedBall, bool isReverse)
        {
            float min_dist = float.MaxValue;
            SSLDetectionBall closest = new();
            int cam = -1;
            foreach (var item in balls.Keys)
            {
                foreach (var b in balls[item])
                {
                    float dist = selectedBall.ToVisionCoordinate(isReverse).Distance(new VectorF2D(b.X, b.Y));
                    if (dist < min_dist)
                    {
                        closest = b;
                        min_dist = dist;
                        cam = (int)item;
                    }
                }
            }
            ball.Affinity = cam;
            ball.Observations[ball.Affinity].IsValid = true;
            ball.Observations[ball.Affinity].LastValid = 1;
            for (int i = 0; i < ball.Observations.Length; i++)
            {
                if (i != cam)
                {
                    var obs = ball.Observations[i];
                    obs.IsValid = false;
                    obs.LastValid = 0;
                }
            }
        }
        private ObservationModel MakeModel(bool isYellow)
        {
            ObservationModel model = new();

            if (ball.MergeObservations() >= 0)
            {
                var obs = ball.Observations[ball.Affinity];
                model.Ball = new BallObservationMeta(obs, times[ball.Affinity]);
            }

            for (int team = 0; team < MergerTrackerConfig.Default.TeamsCount; team++)
            {
                for (int id = 0; id < MergerTrackerConfig.Default.MaxRobotId; id++)
                {
                    ObjectMerger robot = robots[team, id];
                    if (robot.MergeObservations(ball.Affinity) >= 0)
                    {
                        var obs = robot.Observations[robot.Affinity];
                        if ((team == 0 && !isYellow) || (team == 1 && isYellow))
                        {
                            model.Teammates[id] = new RobotObservationMeta(obs, times[robot.Affinity]);
                        }
                        else
                        {
                            model.Opponents[id] = new RobotObservationMeta(obs, times[robot.Affinity]);
                        }
                    }
                }
            }
            foreach (var item in balls.Keys)
            {
                foreach (var b in balls[item])
                {
                    model.OtherBalls.Add(new Observation(new VectorF2D(b.X, b.Y), 0, b.Confidence, item));
                }
            }
            return model;
        }
        private void ForgetObservations()
        {
            balls = new Dictionary<uint, List<SSLDetectionBall>>();
            for (int i = 0; i < robots.GetLength(0); i++)
            {
                for (int j = 0; j < robots.GetLength(1); j++)
                {
                    var robot = robots[i, j];
                    for (int k = 0; k < robot.Observations.Length; k++)
                    {
                        var obs = robot.Observations[k];
                        if (obs.IsValid)
                        {
                            obs.IsValid = false;
                            obs.LastValid = 1;
                        }
                        else
                        {
                            obs.LastValid++;
                        }
                    }
                }
            }
            for (int i = 0; i < ball.Observations.Length; i++)
            {
                var obs = ball.Observations[i];
                if (obs.IsValid)
                {
                    obs.IsValid = false;
                    obs.LastValid = 1;
                }
                else
                {
                    obs.LastValid++;
                }
            }
        }

        public ObservationModel Merge(SSLWrapperPacket packet, bool isReverse, bool isYellow, VectorF2D selectedBall, ref bool selectedBallChanged)
        {
            if (packet == null || packet.Detection == null)
                return null;

            var isValid = ValidateCameraSeen(packet);
            if (!isValid) return null;

            UpdateObservations(packet.Detection, selectedBall, isReverse, selectedBallChanged);

            //not ready
            if (numCamerasSeen != numCameras)
                return null;

            if (selectedBallChanged)
            {
                UpdateSelectedBall(selectedBall, isReverse);
                selectedBallChanged = false;
            }

            var model = MakeModel(isYellow);
            ForgetObservations();

            //reset seen camera detection for next frame
            for (int i = 0; i < camerasSeen.Length; i++)
            {
                camerasSeen[i] = false;
            }
            numCamerasSeen = 0;

            return model;
        }
    }
}