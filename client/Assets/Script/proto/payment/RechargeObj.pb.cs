//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// Generated from: proto/payment/RechargeObj.proto
namespace ProtoVO
{
    namespace payment
    {
        [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"RechargeObj")]
        public partial class RechargeObj : global::ProtoBuf.IExtensible
        {
            public RechargeObj() {}

            private int _rechargeId;
            [global::ProtoBuf.ProtoMember(1, IsRequired = true, Name=@"rechargeId", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
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
            private int _buyTimes = default(int);
            [global::ProtoBuf.ProtoMember(2, IsRequired = false, Name=@"buyTimes", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
            [global::System.ComponentModel.DefaultValue(default(int))]
            public int buyTimes
            {
                get
                {
                    return _buyTimes;
                }
                set
                {
                    _buyTimes = value;
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