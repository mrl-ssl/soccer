using System.Collections.Generic;
using ProtoBuf;

namespace MRL.SSL.Common.SSLWrapperCommunication
{
    [ProtoContract]
    public class SSLGeometryData
    {
        [ProtoMember(1)]
        public SSLGeometryFieldSize Field { get; set; }

        [ProtoMember(2)]
        public List<SSLGeometryCameraCalibration> Calibrations { get; set; } = new List<SSLGeometryCameraCalibration>();
    }
}