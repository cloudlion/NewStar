//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// Generated from: proto/payment/PayReward.proto
namespace ProtoVO
{
    namespace payment
    {
        [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"PayReward")]
        public partial class PayReward : global::ProtoBuf.IExtensible
        {
            public PayReward() {}

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
            private int _rechargeId;
            [global::ProtoBuf.ProtoMember(2, IsRequired = true, Name=@"rechargeId", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
            public int rechargeId
            {
                get
                {
                    return _rechargeId;
                }
                set
                {
                    _rechargeId = value;
                }
            }
            private int _diamonds = default(int);
            [global::ProtoBuf.ProtoMember(3, IsRequired = false, Name=@"diamonds", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
            [global::System.ComponentModel.DefaultValue(default(int))]
            public int diamonds
            {
                get
                {
                    return _diamonds;
                }
                set
                {
                    _diamonds = value;
                }
            }
            private readonly global::System.Collections.Generic.List<int> _rewards = new global::System.Collections.Generic.List<int>();
            [global::ProtoBuf.ProtoMember(4, Name=@"rewards", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
            public global::System.Collections.Generic.List<int> rewards
            {
                get
                {
                    return _rewards;
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