//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// Generated from: proto/common/byteObj.proto
namespace ProtoVO
{
    namespace common
    {
        [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"byteObj")]
        public partial class byteObj : global::ProtoBuf.IExtensible
        {
            public byteObj() {}

            private byte[] _packet = null;
            [global::ProtoBuf.ProtoMember(1, IsRequired = false, Name=@"packet", DataFormat = global::ProtoBuf.DataFormat.Default)]
            [global::System.ComponentModel.DefaultValue(null)]
            public byte[] packet
            {
                get
                {
                    return _packet;
                }
                set
                {
                    _packet = value;
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