//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// Generated from: proto/chat/ChatChannelDismiss.proto
namespace ProtoVO
{
    namespace chat
    {
        [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"ChatChannelDismiss")]
        public partial class ChatChannelDismiss : global::ProtoBuf.IExtensible
        {
            public ChatChannelDismiss() {}

            private long _userId;
            [global::ProtoBuf.ProtoMember(1, IsRequired = true, Name=@"userId", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
            public long userId
            {
                get
                {
                    return _userId;
                }
                set
                {
                    _userId = value;
                }
            }
            private string _channelId;
            [global::ProtoBuf.ProtoMember(2, IsRequired = true, Name=@"channelId", DataFormat = global::ProtoBuf.DataFormat.Default)]
            public string channelId
            {
                get
                {
                    return _channelId;
                }
                set
                {
                    _channelId = value;
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