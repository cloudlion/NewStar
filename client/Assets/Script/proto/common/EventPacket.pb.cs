//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// Generated from: proto/common/EventPacket.proto
namespace ProtoVO
{
    namespace common
    {
        [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"EventPacket")]
        public partial class EventPacket : global::ProtoBuf.IExtensible
        {
            public EventPacket() {}

            private string _proxyNode;
            [global::ProtoBuf.ProtoMember(1, IsRequired = true, Name=@"proxyNode", DataFormat = global::ProtoBuf.DataFormat.Default)]
            public string proxyNode
            {
                get
                {
                    return _proxyNode;
                }
                set
                {
                    _proxyNode = value;
                }
            }
            private byte[] _clientPid = null;
            [global::ProtoBuf.ProtoMember(2, IsRequired = false, Name=@"clientPid", DataFormat = global::ProtoBuf.DataFormat.Default)]
            [global::System.ComponentModel.DefaultValue(null)]
            public byte[] clientPid
            {
                get
                {
                    return _clientPid;
                }
                set
                {
                    _clientPid = value;
                }
            }
            private int _funcode = default(int);
            [global::ProtoBuf.ProtoMember(3, IsRequired = false, Name=@"funcode", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
            [global::System.ComponentModel.DefaultValue(default(int))]
            public int funcode
            {
                get
                {
                    return _funcode;
                }
                set
                {
                    _funcode = value;
                }
            }
            private byte[] _packetData = null;
            [global::ProtoBuf.ProtoMember(4, IsRequired = false, Name=@"packetData", DataFormat = global::ProtoBuf.DataFormat.Default)]
            [global::System.ComponentModel.DefaultValue(null)]
            public byte[] packetData
            {
                get
                {
                    return _packetData;
                }
                set
                {
                    _packetData = value;
                }
            }
            private global::ProtoBuf.IExtension extensionObject;
            global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
            {
                return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
            }
        }

    }
}