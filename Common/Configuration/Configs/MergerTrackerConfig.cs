using System;
using MRL.SSL.Common.Math;
using MRL.SSL.Common.SSLWrapperCommunication;

namespace MRL.SSL.Common.Configuration
{
    public class MergerTrackerConfig : ConfigBase
    {
        public new static MergerTrackerConfig Default { get => (MergerTrackerConfig)_default[(int)ConfigType.MergerTracker]; }
        public override ConfigType Id => ConfigType.MergerTracker;
        public byte[] AvailableCameras { get; set; }
        public byte MaxCameraCount { get; set; }
        public byte MaxRobotId { get; set; }
        public byte MaxTeamRobots { get; set; }
        public byte TeamsCount { get; set; }
        public byte AffinityPersist { get; set; }
        public int FrameRate { get; set; }
        public float FramePeriod { get; set; }
        public float LastBallMaxDistance { get; set; }
        public int FramesToDetectCameras { get; set; }
        public float BallRadi { get; set; }
        public bool PrintRobotKalmaError { get; set; }
        public bool PrintBallKalmanError { get; set; }
        public float Latency { get; set; }
        public double MaxPredictionTime { get; set; }
        public float RobotPositionVariance { get; set; }
        public float RobotAngleVariance { get; set; }
        public bool RobotFastPredict { get; set; }
        public float RobotStuckThreshold { get; set; }
        public float RobotVelocityNextStepCovariance { get; set; }
        public float RobotStuckDecay { get; set; }
        public bool RobotUseAverageInPropagation { get; set; }
        public float RobotVelocityVariance { get; set; }
        public float RobotAngularVelocityVariance { get; set; }
        public float RobotStuckVariance { get; set; }
        public float BallPositionVariance { get; set; }
        public float BallFriction { get; set; }
        public float Gravity { get; set; }
        public int MaxNotSeenFrames { get; set; }
        public float ActionDelay { get; set; }
        public float BallConfidenceThreshold { get; set; }
        public float BallLikelihoodThreshold { get; set; }
        public bool BallImprobabilityFiltering { get; set; }
        public float BallOccludeTime { get; set; }
        public float BallTeammateCollisionRadius { get; set; }
        public float BallOpponentCollisionRadius { get; set; }
        public bool BallWalls { get; set; }
        public float WallWidth { get; set; }
        public float OurRobotRadius { get; set; }
        public float OurRobotHeight { get; set; }
        public float OpponentHeight { get; set; }
        public float OpponentRadius { get; set; }
        public float BallVelocityVarianceNearRobot { get; set; }
        public float BallVelocityVarianceNoRobot { get; set; }
        public float OpponentPositionVariance { get; set; }
        public float OpponentAngleVariance { get; set; }
        public float OpponentVelocityNextStepCovariance { get; set; }
        public float OpponentVelocityVariance { get; set; }
        public float OpponentAngularVelocityVariance { get; set; }

        public MergerTrackerConfig()
        {
        }
    }
}