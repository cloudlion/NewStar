using System;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Collections.Generic;
using UnityEngine;

namespace GameUtil
{
    public class PackageUtils
    {
		static int opcode = 0;

        public static Int32 GetProtocolID(UInt16 appCode, UInt16 funcCode)
        {
            return (Int32)(((UInt32)appCode << 16) + (UInt32)funcCode);
        }
        
        public static byte[] Serialize2Binary<T>(T proto, UInt16 appCode, UInt16 funcCode, int opCode) where T : IProtocolHead
        {
            return Encode<T>(proto, appCode, funcCode, opCode);
        }
        
        public static IProtocolHead Deserialize2Proto(byte[] package, ref UInt16 appCode, ref UInt16 funcCode, ref int opCode, bool hasHead = true)
        {
            if (package == null)
            {
                return null;
            }
            
			return Decode(package, ref appCode, ref funcCode, ref opCode, hasHead);
        }

        public static bool isErrorType(UInt16 funcCode)
        {
            return funcCode == ProtoVO.common.error.GetFnCode();
        }


#region internal
        //*****************************************************
        // Package structure:
        //
        //          |<- PROTOCOL_ID ->|
        //      HEAD|APPCODE|FUNCCODE|FLAG|PAYLOAD
        //       4      2       2       1    N
        //      packageLen in HEAD is supposed to be 4 + 2 + 2 + 1 + N
        //
        //*****************************************************
        public const int HEAD_LENGTH = 4;
        const int PROTOCOL_ID_LENGTH = 4;
        const int FLAG_LENGTH = 1;
        
        const int HEAD_OFFSET = 0;
        const int PROTOCOL_ID_OFFSET = HEAD_OFFSET + HEAD_LENGTH;
        const int FLAG_OFFSET = PROTOCOL_ID_OFFSET + PROTOCOL_ID_LENGTH;
		const int PAYLOAD_OFFSET = HEAD_OFFSET + HEAD_LENGTH;
        
        private static Dictionary<string, System.Reflection.MethodInfo> flagProcDict = new Dictionary<string, System.Reflection.MethodInfo>();
        private static byte FlagFromProto(IProtocolHead proto)
        {
            Type t = proto.GetType();
            System.Reflection.MethodInfo flagProc = null;
            
            if (flagProcDict.ContainsKey(t.Name))
            {
                flagProc = flagProcDict[t.Name];
            }
            else
            {
                flagProc = proto.GetType().GetMethod("GetFlag");
                flagProcDict[t.Name] = flagProc;
            }
            
            if (null == flagProc)
            {
                throw new Exception("GetFlag method not found in Type : " + proto.GetType().Name);
            }
            
            return (byte)flagProc.Invoke(null, null);
        }
        
        private static Type GetProtocolType(UInt16 appCode, UInt16 funcCode)
        {
			if (funcCode >= AppFnMapping.MAPPING.Length)
				return null;
            Type ret = AppFnMapping.MAPPING[funcCode];
            if (null == ret)
            {
                throw new Exception("Unregistered appcode = " + appCode + ", funcCode = " + funcCode);
            }
            return ret;
        }

        private static int SupposedPackageLen(byte[] package)
        {
            if ((null == package) || (package.Length < PAYLOAD_OFFSET))
            {
                throw new Exception("Invalid package length!");
            }
            
            // headLen is 4, ToInt32 fits.
			int netLen = BitConverter.ToInt32 (package, HEAD_OFFSET);
            int len = IPAddress.NetworkToHostOrder(BitConverter.ToInt32(package, HEAD_OFFSET)) + HEAD_LENGTH;
            return len;
        }

        private static byte[] Encode<T>(T data, UInt16 appCode, UInt16 funcCode, int opCode) where T : IProtocolHead
        {
            if (null == data)
            {
                Logger.LogError("Invalid data!");
                return null;
            }
            
            if ( typeof(T) == typeof(ProtoVO.common.HeartBeat) ) {
			
                return  new byte[]{0, 0, 0 , 0};
            }

            ProtoVO.common.packet  packet = new ProtoVO.common.packet ();
            packet.funcode = funcCode;
			packet.opcode = opCode;
			packet.data = DataParser.Serialize<T>(data);


			byte[] payload = DataParser.Serialize<ProtoVO.common.packet>(packet);
			byte flag = FlagFromProto(data);
            
        //    ProcessFlag(payload, flag);

            int totalLen = HEAD_LENGTH +  payload.Length;
            
            //// payload
            byte[] package = new byte[totalLen];
            Array.Copy(payload, 
                       0, 
                       package, 
                       PAYLOAD_OFFSET, 
                       payload.Length);            
            
//			Logger.Log ( Convert.ToBase64String(payload) );
            // flag
           // package[FLAG_OFFSET] = flag;
            
            // appCode & funcCode
            //int protocolId = GetProtocolID(appCode, funcCode);
//            Array.Copy (BitConverter.GetBytes(IPAddress.HostToNetworkOrder(protocolId)), 
//                        0, 
//                        package, 
//                        PROTOCOL_ID_OFFSET, 
//                        PROTOCOL_ID_LENGTH);
            
            //// head
            byte[] lengthArray = BitConverter.GetBytes(IPAddress.HostToNetworkOrder(payload.Length));
            Array.Copy (BitConverter.GetBytes(IPAddress.HostToNetworkOrder(payload.Length)), 
                        0, 
                        package, 
                        HEAD_OFFSET, HEAD_LENGTH);
            
            return package;
        }

        public static IProtocolHead Decode(byte[] package, ref UInt16 appCode, ref UInt16 funcCode, ref int opCode, bool hasHead = true)
        {
			int packageLen = hasHead ?SupposedPackageLen(package): package.Length;
            if (packageLen != (package.Length))
            {
                Logger.LogError("response data head mismatch with actual length!");
				Logger.LogError("SupposedPackageLen: "+ packageLen);
				Logger.LogError("Real length: "+ package.Length);
                appCode = 0;
                funcCode = 0;
                return null;
            }


			int payloadLen = packageLen - (hasHead?HEAD_LENGTH:0);
            byte[] payload = new byte[payloadLen];
            
            // payload
			Array.Copy(package, hasHead?PAYLOAD_OFFSET:0, payload, 0, payloadLen);

			ProtoVO.common.packet packet = DataParser.Deserialize<ProtoVO.common.packet>(payload) as ProtoVO.common.packet;
			opCode = packet.opcode;
//                // appCode & funcCode
			funcCode = (UInt16)(packet.funcode & 0xFFFF);
//			UnityEngine.Debug.LogWarning("recieved packet funcode >>>>>>>>>>>>>>>>>>>>>>>" + funcCode);

			if (packet.batchPackets.Count > 0)
			{
//				UnityEngine.Debug.LogWarning("recieved batch packet >>>>>>>>>>>>>>>>>>>>>>>");
				return packet;
			}

//            appCode = (UInt16)(protocolId >> 16);
//            funcCode = (UInt16)(protocolId & 0xFF);

//     //       ProcessFlag(payload, flag);
            
            IProtocolHead ph = null;
            System.Type targetType = GetProtocolType(appCode, funcCode);
			if (targetType == null)
				return null;
            try
            {
				ph = DataParser.Deserialize(packet.data, targetType) as IProtocolHead;
            }
            catch(Exception e)
            {
				//Debug.Log ("exception ==" + e.Message);
                //Logger.LogError("Deserialize protobuf exception! clsName = " + targetType.Name);
//                Logger.LogException(e);
            }
            
            return ph;
        }

        public static void ProcessFlag(byte[] payload, byte flag)
        {
            //TODO : compression, encryption
        }

		public static int OpCode
		{
			get{
				opcode ++;
				return opcode;
			}
		}
#endregion
    }
}

