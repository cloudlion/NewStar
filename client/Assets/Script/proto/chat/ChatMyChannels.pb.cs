//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// Generated from: proto/chat/ChatMyChannels.proto
// Note: requires additional types generated from: proto/chat/ChatChannelObj.proto
namespace ProtoVO
{
    namespace chat
    {
        [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"ChatMyChannels")]
        public partial class ChatMyChannels : global::ProtoBuf.IExtensible
        {
            public ChatMyChannels() {}

            private readonly global::System.Collections.Generic.List<chat.ChatChannelObj> _channels = new global::System.Collections.Generic.List<chat.ChatChannelObj>();
            [global::ProtoBuf.ProtoMember(1, Name=@"channels", DataFormat = global::ProtoBuf.DataFormat.Default)]
            public global::System.Collections.Generic.List<chat.ChatChannelObj> channels
            {
                get
                {
                    return _channels;
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