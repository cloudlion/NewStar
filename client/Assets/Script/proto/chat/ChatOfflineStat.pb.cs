//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// Generated from: proto/chat/ChatOfflineStat.proto
namespace ProtoVO
{
    namespace chat
    {
        [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"ChatOfflineStat")]
        public partial class ChatOfflineStat : global::ProtoBuf.IExtensible
        {
            public ChatOfflineStat() {}

            private int _page = default(int);
            [global::ProtoBuf.ProtoMember(1, IsRequired = false, Name=@"page", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
            [global::System.ComponentModel.DefaultValue(default(int))]
            public int page
            {
                get
                {
                    return _page;
                }
                set
                {
                    _page = value;
                }
            }
            private readonly global::System.Collections.Generic.List<long> _userIds = new global::System.Collections.Generic.List<long>();
            [global::ProtoBuf.ProtoMember(2, Name=@"userIds", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
            public global::System.Collections.Generic.List<long> userIds
            {
                get
                {
                    return _userIds;
                }
            }

            private readonly global::System.Collections.Generic.List<string> _userNicks = new global::System.Collections.Generic.List<string>();
            [global::ProtoBuf.ProtoMember(3, Name=@"userNicks", DataFormat = global::ProtoBuf.DataFormat.Default)]
            public global::System.Collections.Generic.List<string> userNicks
            {
                get
                {
                    return _userNicks;
                }
            }

            private readonly global::System.Collections.Generic.List<int> _offlineTimes = new global::System.Collections.Generic.List<int>();
            [global::ProtoBuf.ProtoMember(4, Name=@"offlineTimes", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
            public global::System.Collections.Generic.List<int> offlineTimes
            {
                get
                {
                    return _offlineTimes;
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