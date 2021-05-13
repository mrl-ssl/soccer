
#region Designer generated code
#pragma warning disable CS0612, CS0618, CS1591, CS3021, IDE0079, IDE1006, RCS1036, RCS1057, RCS1085, RCS1192

namespace MRL.SSL.Common.SSLWrapperCommunication
{
    [global::ProtoBuf.ProtoContract(Name = @"SSL_WrapperPacket")]
    public partial class SSLWrapperPacket : global::ProtoBuf.IExtensible
    {
        private global::ProtoBuf.IExtension __pbn__extensionData;
        global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
            => global::ProtoBuf.Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [global::ProtoBuf.ProtoMember(1, Name = @"detection")]
        public SSLDetectionFrame Detection { get; set; }

        [global::ProtoBuf.ProtoMember(2, Name = @"geometry")]
        public SSLGeometryData Geometry { get; set; }

    }
}
#pragma warning restore CS0612, CS0618, CS1591, CS3021, IDE0079, IDE1006, RCS1036, RCS1057, RCS1085, RCS1192
#endregion
