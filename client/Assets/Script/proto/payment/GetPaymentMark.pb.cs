//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// Generated from: proto/payment/GetPaymentMark.proto
namespace ProtoVO
{
    namespace payment
    {
        [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"GetPaymentMark")]
        public partial class GetPaymentMark : global::ProtoBuf.IExtensible
        {
            public GetPaymentMark() {}

            private int _platform = default(int);
            [global::ProtoBuf.ProtoMember(1, IsRequired = false, Name=@"platform", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
            [global::System.ComponentModel.DefaultValue(default(int))]
            public int platform
            {
                get
                {
                    return _platform;
                }
                set
                {
                    _platform = value;
                }
            }
            private int _payoutId = default(int);
            [global::ProtoBuf.ProtoMember(2, IsRequired = false, Name=@"payoutId", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
            [global::System.ComponentModel.DefaultValue(default(int))]
            public int payoutId
            {
                get
                {
                    return _payoutId;
                }
                set
                {
                    _payoutId = value;
                }
            }
            private int _rechargeId = default(int);
            [global::ProtoBuf.ProtoMember(3, IsRequired = false, Name=@"rechargeId", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
            [global::System.ComponentModel.DefaultValue(default(int))]
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
            private global::ProtoBuf.IExtension extensionObject;
            global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
            {
                return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
            }
        }

    }
}