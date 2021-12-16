using System;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using Google.Protobuf;
using GameNetWork.Packet;

namespace GameUtil
{
    public class PackageUtils
    {
        const int PKG_HEAD_BYTES = 4;
        const int MSG_FLAG_BYTES = 1;
        const int  MSG_ROUTE_CODE_BYTES = 2;

        static int opcode = 0;

        public static Int32 GetProtocolID(UInt16 appCode, UInt16 funcCode)
        {
            return (Int32)(((UInt32)appCode << 16) + (UInt32)funcCode);
        }
        
        public static byte[] Serialize2Binary<T>(T proto, UInt16 appCode, UInt16 funcCode, int opCode) where T : IProtocolHead,IMessage<T>
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
            return false;
         //   return funcCode == ProtoVO.common.error.GetFnCode();
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
            byte[] lenArray = new byte[4];
            Array.Copy(package, 1, lenArray, 1, 3);
            int contentLength = IPAddress.NetworkToHostOrder(BitConverter.ToInt32(lenArray, 0));

          //  int netLen = BitConverter.ToInt32 (package, HEAD_OFFSET);
            int len = contentLength + HEAD_LENGTH;
            return len;
        }

        private static byte[] Encode<T>(T data, UInt16 appCode, UInt16 funcCode, int opCode) where T : Google.Protobuf.IMessage<T>, IProtocolHead
        {
            if (null == data)
            {
                Logger.LogError("Invalid data!");
                return null;
            }
            
            if ( typeof(T) == typeof(GameProtos.common.HeartBeat) ) {
			
                return  new byte[]{0, 0, 0 , 0};
            }

            byte[] payload = EncodeMessage(data, MessageType.Notify, funcCode);
            
            byte[] package = EncodePacket(PacketType.Data, payload);
            return package;
        }

        public static byte[] EncodePacket(PacketType type, byte[] data)
        {
            int totalLen = HEAD_LENGTH + (data != null?data.Length:0);

            //// payload
            byte[] package = new byte[totalLen];
            package[0] = (byte)type;
            if (data != null && data.Length > 0)
            {
                byte[] lengthArray = BitConverter.GetBytes(IPAddress.HostToNetworkOrder(data.Length));
                Array.Copy(lengthArray,
                            1,
                            package,
                            1, HEAD_LENGTH - 1);

                Array.Copy(data,
                        0,
                        package,
                        HEAD_LENGTH, data.Length);
            }

            return package;
        }

        public static IProtocolHead Decode(byte[] package, ref UInt16 appCode, ref UInt16 funcCode, ref int opCode, bool hasHead = true)
        {
            Debug.Log("decode message");
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
            if (payloadLen == 0)
                return null;
            byte[] payload = new byte[payloadLen];
            // payload
			Array.Copy(package, hasHead?PAYLOAD_OFFSET:0, payload, 0, payloadLen);
			opCode = 0;
            IProtocolHead ph = DecodeMessage(payload, ref appCode, ref funcCode);
            
            return ph;
        }

 /*       public static IProtocolHead Decode(Packet packet, ref UInt16 appCode, ref UInt16 funcCode, ref int opCode, bool hasHead = true)
        {
            Debug.Log("decode message");
            int payloadLen = packet.length;
            byte[] payload = new byte[payloadLen];

            // payload
            Array.Copy(packet.data, PAYLOAD_OFFSET, payload, 0, payloadLen);
            opCode = 0;
            IProtocolHead ph = DecodeMessage(packet.data, ref appCode,ref funcCode);

            return ph;
        }*/

        static byte[] EncodeMessage<T>(T data, MessageType type, UInt16 code) where T : Google.Protobuf.IMessage<T>, IProtocolHead
        {
            byte[] payload = DataParser.Serialize<T>(data);
            byte flag = Convert.ToByte((int)type);
            byte[] codeArray = BitConverter.GetBytes(IPAddress.HostToNetworkOrder((Int16)code));
            flag <<= 1;
            flag |= 1;
            int idBytes = 0;
            if (type == MessageType.Request || type == MessageType.Response)
                idBytes = 1;

            int msgLen = FLAG_LENGTH + payload.Length + MSG_ROUTE_CODE_BYTES + idBytes;
            byte[] msgData = new byte[msgLen];
            int offset = 0;
            msgData[offset++] = flag;
            if (type == MessageType.Request || type == MessageType.Response)
                msgData[offset++] = 1;
            msgData[offset++] = codeArray[0];
            msgData[offset++] = codeArray[1];
            Array.Copy(payload,
                        0,
                        msgData,
                        offset, payload.Length);
            return msgData;
        }

        private static IProtocolHead DecodeMessage(byte[] data, ref UInt16 appCode, ref UInt16 funcCode)
        {
            IProtocolHead ph = null;

            appCode = 0;
            funcCode = (UInt16)IPAddress.NetworkToHostOrder(BitConverter.ToInt16(data, 1));
            System.Type targetType = GetProtocolType(appCode, funcCode);
            Debug.Log("decode message: " + targetType);
            if (targetType == null)
                return null;
            try
            {
                int len = data.Length - 3;
                byte[] msgData = new byte[len];
                Array.Copy(data,
                        3,
                        msgData,
                        0, len);
                ph = DataParser.Deserialize(msgData, targetType) as IProtocolHead;
            }
            catch (Exception e)
            {
                Debug.LogError("exception ==" + e.Message);
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

