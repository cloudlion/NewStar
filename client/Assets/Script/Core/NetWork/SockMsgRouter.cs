using UnityEngine;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Net.Sockets;
using System.Threading;
using System.IO;
using GameUtil;
using Google.Protobuf;
//using MVC;

namespace GameNetWork
{
    public class SockMsgRouter : SockRouter
    {
		public Action timeoutHandler = null;
		public Action blockGameMsgBack = null;
		public Dictionary<int,Action<IProtocolHead,Operation>> errorCallBack = new Dictionary<int,Action<IProtocolHead,Operation>> ();

		private List<Type> noResponseList = new List<Type> (){ };//{typeof(ProtoVO.worldmap.MapPosRequest), typeof(ProtoVO.common.HeartBeat), typeof(ProtoVO.fte.MarkFte), typeof(ProtoVO.alliance.HelpReq), typeof(ProtoVO.alliance.HelpHelp)};
        private abstract class DataStream
        {
            protected const int BUF_LEN = 256 * 1024;
            protected object syncObj = new object();
            protected byte[] buffer = new byte[BUF_LEN];
            protected int dataLen = 0;

            public abstract void Write(byte[] data);
            public abstract byte[] Read();

            public virtual void Clear()
            {
                lock (syncObj)
                {
                    dataLen = 0;
                }
            }
        }

        private class SendStream : DataStream
        {
            //used for router
            public override void Write(byte[] msg)
            {
                lock (syncObj)
                {
                    if (dataLen + msg.Length > BUF_LEN)
                    {
                        throw new Exception("send buffer is full");
                    }
                    Array.Copy(msg, 0, buffer, dataLen, msg.Length);
                    dataLen += msg.Length;
                }
            }

            //used for sock
            public override byte[] Read()
            {
                lock (syncObj)
                {
                    byte[] data = new byte[dataLen];
                    Array.Copy(buffer, data, dataLen);
                    Array.Clear(buffer, 0, dataLen);
                    dataLen = 0;
                    return data;
                }
            }
        }

        private class RecvStream : DataStream
        {
            //used for sock
            public override void Write(byte[] data)
            {
                lock (syncObj)
                {
                    if (dataLen + data.Length > BUF_LEN)
                    {
                        throw new Exception("recieve buffer is full");
                    }
                    Array.Copy(data, 0, buffer, dataLen, data.Length);
                    dataLen += data.Length;
                }
            }

            //used for router
            public override byte[] Read()
            {
                lock (syncObj)
                {
                    if (dataLen < 4)
                    {
                        return null;
                    }
                    byte[] lenArray = new byte[4];
                    Array.Copy(buffer, 1, lenArray, 1,3);
                    int contentLength = IPAddress.NetworkToHostOrder(BitConverter.ToInt32(lenArray, 0));
                    int msgLen = contentLength + GameUtil.PackageUtils.HEAD_LENGTH;
                    if (dataLen < msgLen)
                    {
                        return null;
                    }

                    byte[] msg = new byte[msgLen];
                    Array.Copy(buffer, msg, msgLen);
                    Array.Copy(buffer, msgLen, buffer, 0, Mathf.Min(dataLen, BUF_LEN - msgLen));
                    
                    dataLen -= msgLen;
                    return msg;
                }
            }
        }

        private DataStream recvStream = new RecvStream();
        private DataStream sendStream = new SendStream();

        public delegate void MsgHandler(SockMsgRouter router, IProtocolHead ph);
        public delegate void ErrorHandler(SockMsgRouter router, SocketError error);
        public delegate void OperateErrorHandler(SockMsgRouter router, IProtocolHead ph, IProtocolHead errorPh);

        private Dictionary<int, Delegate> handlerMap = new Dictionary<int, Delegate>();
		private Dictionary<int, IProtocolHead> sentMsgMap = new Dictionary<int, IProtocolHead> ();

        private Dictionary<int, Delegate> oprationErrorHandlerMap = new Dictionary<int, Delegate>();

        private OperationQueue  operationQueue ;

        private ErrorHandler errorHandler;


		public SockMsgRouter()
		{
			operationQueue = new OperationQueue (this);
		}

        public ErrorHandler OnError
        {
            get { return errorHandler; }
            set { errorHandler = value; }
        }
        
        public override void Shutdown()
        {
            sendStream.Clear();
            recvStream.Clear();
			operationQueue.Clear ();
            base.Shutdown();
        }

        public override void Send(byte[] msg)
        {
            sendStream.Write(msg);
        }

		public void ResendAllPendingMsg()
		{
			operationQueue.ResendAll ();
		}

        protected override void DoDispatchMsg ()
        {
            while (true)
            {
                byte[] msg = recvStream.Read();
                if (null == msg)
                {
                    break;
                }
                HandleMsg(msg);
                //if (GameVersion.IsDevelopVersion)
                //    ProxyMgr.Instance.GetProxy<ServerDebugProxy>().SendDebugMessage(msg);
            }
        }

        public override byte[] GetPendingSend ()
        {
            return this.sendStream.Read();
        }

        protected override void DoRecvData (byte[] msg)
        {
            recvStream.Write(msg);
        }

        public void RegisterMsgHandler(UInt16 appCode, UInt16 funcCode, MsgHandler handler)
        {
            int id = PackageUtils.GetProtocolID(appCode, funcCode);
            if (handler == null)
            {
                Logger.LogWarning("msg handler is null: " + id);
                return;
            }
            
            if (!handlerMap.ContainsKey(id))
            {
                handlerMap[id] = null;
            }
            handlerMap[id] = (MsgHandler)handlerMap[id] + handler;
        }

        public void UnRegisterMsgHandler(UInt16 appCode, UInt16 funcCode, MsgHandler handler)
        {
            int id = PackageUtils.GetProtocolID(appCode, funcCode);
            if (!handlerMap.ContainsKey(id))
            {
                Logger.LogError("id not found in handlerDic : " + id.ToString());
                return;
            }
            handlerMap[id] = (MsgHandler)handlerMap[id] - handler;
        }

        #region operationErrorHandle

        public void RegisterOperationErrorMsgHandler(UInt16 appCode, UInt16 funcCode, OperateErrorHandler handler)
        {
            int id = PackageUtils.GetProtocolID(appCode, funcCode);
            if (handler == null)
            {
                Logger.LogWarning("msg handler is null: " + id);
                return;
            }

            if (!oprationErrorHandlerMap.ContainsKey(id))
            {
                oprationErrorHandlerMap[id] = null;
            }
            oprationErrorHandlerMap[id] = (OperateErrorHandler)oprationErrorHandlerMap[id] + handler;
        }

        public void UnRegisterOperationErrorMsgHandler(UInt16 appCode, UInt16 funcCode, OperateErrorHandler handler)
        {
            int id = PackageUtils.GetProtocolID(appCode, funcCode);
            if (!oprationErrorHandlerMap.ContainsKey(id))
            {
                Logger.LogError("id not found in handlerDic : " + id.ToString());
                return;
            }
            oprationErrorHandlerMap[id] = (OperateErrorHandler)oprationErrorHandlerMap[id] - handler;
        }

        #endregion

		public void SendMsg<T>(T proto, UInt16 appCode, UInt16 funcCode, int opCode, bool blockGame = false,Action<IProtocolHead,Operation> callback = null) where T : IProtocolHead,IMessage<T>
        {
//            if(typeof(T) != typeof(ProtoVO.common.HeartBeat))
//                Logger.Log("ming request " + typeof(T).Name.ToString() + "...");
            opCode = PackageUtils.OpCode;
			if (callback != null) {
				errorCallBack.Add (opCode, callback);
			}
			byte[] bytes = PackageUtils.Serialize2Binary<T>(proto, appCode, funcCode, opCode);

            if (null != bytes)
            { 
				if( !noResponseList.Contains (typeof(T)) )
				{
					Operation op = new Operation(opCode, funcCode, proto,bytes, blockGame);
					operationQueue.Add(op);
				}
                Send(bytes);
            }
        }

        public void SendHandShake()
        {
            byte[] bytes = PackageUtils.EncodePacket(Packet.PacketType.HandshakeAck, null);
            Send(bytes);
        }


        public void ClearBuff(bool clearOperations)
		{
			sendStream.Clear ();
			if(clearOperations)
			{
				operationQueue.Clear();
			}
			else
			{
				operationQueue.Stash();
			}
		}

		public void Update()
		{
			operationQueue.Update ();
		}

        protected override void HandleMsg(byte[] data)
        {
            Debug.Log("recieved message");
            UInt16 appCode = 0, funcCode = 0;
			int opCode = 0;
			IProtocolHead ph = PackageUtils.Deserialize2Proto(data, ref appCode, ref funcCode, ref opCode);
            if (null == ph)
            {
                return;
            }

//			if(ph is ProtoVO.common.packet)
//			{
//				ProtoVO.common.packet packet = ph as ProtoVO.common.packet;
//				for(int i = 0; i < packet.batchPackets.Count; i++)
//				{
//					HandleSubMsg(packet.batchPackets[i].packet);
//				}
//				return;
//			}
//			else if(ph is ProtoVO.user.UserSyncReq)
//			{Logger.Log("Reconnect finish");
//
//				ProtoVO.user.UserSyncReq sync = ph as ProtoVO.user.UserSyncReq;
//
//				for(int i = 0; i < sync.jobQueues.Count; i++)
//				{
//					HandleSubMsg(sync.jobQueues[i].packet);
//				}
//				if(sync.userInfo != null && sync.userInfo.packet != null)
//					HandleSubMsg(sync.userInfo.packet);
//				if( blockGameMsgBack != null)
//					blockGameMsgBack.Invoke();
//			}

            Operation op = operationQueue.OnRecieve (opCode);
            int id = PackageUtils.GetProtocolID(appCode, funcCode);
//            if (!handlerMap.ContainsKey(id))
//            {
//                Logger.LogWarning("Can't find handler for " + AppFnMapping.MAPPING[funcCode].Name);
//                return;
//            }
			HandleCallBack (ph, id, op, opCode);
            HandleOperationError(appCode, funcCode, op,ph);
        }

		protected void HandleSubMsg(byte[] data)
		{
			UInt16 appCode = 0, funcCode = 0;
			int opCode = 0;
			IProtocolHead ph = PackageUtils.Deserialize2Proto(data, ref appCode, ref funcCode, ref opCode, false);
			if (null == ph)
			{
				return;
			}
			
			if(ph is ProtoVO.common.packet)
			{
				ProtoVO.common.packet packet = ph as ProtoVO.common.packet;
//				for(int i = 0; i < packet.batchPackets.Count; i++)
//				{
//					HandleSubMsg(packet.batchPackets[i].packet);
//				}
				return;
			}

            Operation op = operationQueue.OnRecieve (opCode);
			int id = PackageUtils.GetProtocolID(appCode, funcCode);
			if (!handlerMap.ContainsKey(id))
			{
//				Logger.LogWarning("Can't find handler for " + AppFnMapping.MAPPING[funcCode].Name);
				return;
			}
			HandleCallBack (ph, id, op, opCode);
            HandleOperationError(appCode, funcCode, op,ph);
        }


		private void HandleCallBack(IProtocolHead ph,int callBackId,Operation op,int opCode)
		{
            Delegate func = null;
            handlerMap.TryGetValue(callBackId, out func);
            if(func != null) {
                ((MsgHandler)func)(this, ph);

//                if(ph.GetType() != typeof(ProtoVO.common.HeartBeat))
//                    Logger.Log("ming receive response " + ph.GetType().Name.ToString() + "!");
            }
		}

        private void HandleOperationError(UInt16 appCode, UInt16 funcCode, Operation op, IProtocolHead errorPh)
        {
            if (PackageUtils.isErrorType(funcCode) && null != op)
            {
                IProtocolHead ph = op.Ph;
                if (null == ph)
                {
                    return;
                }

                int errorId = PackageUtils.GetProtocolID(appCode, op.FuncCode);
                if (!oprationErrorHandlerMap.ContainsKey(errorId))
                {
//                    Logger.LogWarning("Can't find error handler for " + AppFnMapping.MAPPING[funcCode].Name);
                    return;
                }

                Delegate func = oprationErrorHandlerMap[errorId];
                if (func != null)
                {
                    ((OperateErrorHandler)func)(this, ph, errorPh);
                }
            }
        }
		
		protected override void HandleError(SocketError error)
		{
			Logger.LogWarning("sock error" + error);
			if (null != OnError)
			{
				OnError(this, error);
            }
        }

		public void FakeRecieveMsg(IProtocolHead ph, int funcCode)
		{
			Delegate func = handlerMap[funcCode];
			if (func != null)
			{
				((MsgHandler)func)(this, ph);
			}
		}

    }
}



