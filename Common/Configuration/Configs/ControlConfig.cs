using MRL.SSL.Common.Math;

namespace MRL.SSL.Common.Configuration
{
    public class ControlConfig : ConfigBase
    {
        public new static ControlConfig Default { get => (ControlConfig)_default[(int)ConfigType.Control]; }
        public override ConfigType Id => ConfigType.Control;

        public int MaxFrames { get; set; }
        public int Latency { get; set; }
        public int Prediction { get; set; }
        public int PIDModuleCount { get; set; }
        public float PosCoefResetValue { get; set; }
        public float AngleCoefResetValue { get; set; }
        public float AngleMinVelocityTresh { get; set; }
        public float PosMinVelocityTresh { get; set; }
        public float MinAngleCoef { get; set; }
        public float MinPosCoef { get; set; }
        public float AngleResetCoefTresh { get; set; }
        public float PosResetCoefTresh { get; set; }
        public float Kp { get; set; }
        public float Ki { get; set; }
        public float Kd { get; set; }
        public float AngleKp { get; set; }
        public float AngleKi { get; set; }
        public float AngleKd { get; set; }
        public VectorF2D MaxAccel { get; set; }
        public VectorF2D MaxSpeed { get; set; }
        public float MaxAlfa { get; set; }
        public float MaxW { get; set; }
        public float AccelFactor { get; set; }
        public float AlfaFactor { get; set; }
        public float Accuercy { get; set; }
        public float WAccuercy { get; set; }
        public float TunningDist { get; set; }
        public float TunningAngle { get; set; }
        public float MaxPathLength { get; set; }
        public float DistanceInterpolationCoef { get; set; }
        public float DisInterpolationMax { get; set; }
        public float DisInterpolationMin { get; set; }
        public float PathLengthInterpolationCoef { get; set; }
        public int PathTrajectorySpeedLength { get; set; }
        public float MinPIDDistanceTresh { get; set; }
        public float MinPIDAngleTresh { get; set; }
        public float MinAngleTresh { get; set; }
        public float MinDistanceTresh { get; set; }
        public int MaxSearchPosIterations { get; set; }
        public int MaxSearchAngleIterations { get; set; }
        public float MaxPosTunningAccel { get; set; }
        public float MaxAngleTunningAccel { get; set; }
        public bool EnableMotion1DOp2 { get; set; }
    }
}