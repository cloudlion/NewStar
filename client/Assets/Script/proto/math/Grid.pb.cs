//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// Generated from: proto/math/Grid.proto
namespace ProtoVO
{
    namespace math
    {
        [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"Grid")]
        public partial class Grid : global::ProtoBuf.IExtensible
        {
            public Grid() {}

            private int _x;
            [global::ProtoBuf.ProtoMember(1, IsRequired = true, Name=@"x", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
            public int x
            {
                get
                {
                    return _x;
                }
                set
                {
                    _x = value;
                }
            }
            private int _y;
            [global::ProtoBuf.ProtoMember(2, IsRequired = true, Name=@"y", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
            public int y
            {
                get
                {
                    return _y;
                }
                set
                {
                    _y = value;
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