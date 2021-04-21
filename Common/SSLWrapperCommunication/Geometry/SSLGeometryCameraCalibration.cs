using ProtoBuf;

namespace MRL.SSL.Common.SSLWrapperCommunication
{
    [ProtoContract]
    public class SSLGeometryCameraCalibration
    {
        [ProtoMember(1)]
        public uint CameraId { get; set; }

        [ProtoMember(2)]
        public float FocalLength { get; set; }

        [ProtoMember(3)]
        public float PrincipalPointX { get; set; }

        [ProtoMember(4)]
        public float PrincipalPointY { get; set; }

        [ProtoMember(5)]
        public float Distortion { get; set; }

        [ProtoMember(6)]
        public float Q0 { get; set; }

        [ProtoMember(7)]
        public float Q1 { get; set; }

        [ProtoMember(8)]
        public float Q2 { get; set; }

        [ProtoMember(9)]
        public float Q3 { get; set; }

        [ProtoMember(10)]
        public float Tx { get; set; }

        [ProtoMember(11)]
        public float Ty { get; set; }

        [ProtoMember(12)]
        public float Tz { get; set; }

        [ProtoMember(13)]
        public float DerivedCameraWorldTx { get; set; }

        [ProtoMember(14)]
        public float DerivedCameraWorldTy { get; set; }

        [ProtoMember(15)]
        public float DerivedCameraWorldTz { get; set; }

    }
}