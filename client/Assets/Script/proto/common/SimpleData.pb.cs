//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// Generated from: proto/common/SimpleData.proto
namespace ProtoVO
{
    namespace common
    {
        [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"SimpleData")]
        public partial class SimpleData : global::ProtoBuf.IExtensible
        {
            public SimpleData() {}

            private byte[] _data = null;
            [global::ProtoBuf.ProtoMember(1, IsRequired = false, Name=@"data", DataFormat = global::ProtoBuf.DataFormat.Default)]
            [global::System.ComponentModel.DefaultValue(null)]
            public byte[] data
            {
                get
                {
                    return _data;
                }
                set
                {
                    _data = value;
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