using System.Collections.Generic;
using ProtoBuf;

namespace MRL.SSL.Common.SSLWrapperCommunication
{
    [ProtoContract]
    public class SSLDetectionFrame
    {
        [ProtoMember(1)]
        public uint FrameNumber { get; set; }

        [ProtoMember(2)]
        public double CaptureTime { get; set; }

        [ProtoMember(3)]
        public double SentTime { get; set; }

        [ProtoMember(4)]
        public uint CameraId { get; set; }

        [ProtoMember(5)]
        public List<SSLDetectionBall> Balls { get; set; } = new List<SSLDetectionBall>();

        [ProtoMember(6)]
        public List<SSLDetectionRobot> YellowRobots { get; set; } = new List<SSLDetectionRobot>();

        [ProtoMember(7)]
        public List<SSLDetectionRobot> BlueRobots { get; set; } = new List<SSLDetectionRobot>();
    }
}