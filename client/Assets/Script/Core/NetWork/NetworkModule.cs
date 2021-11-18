using UnityEngine;
using System;
using System.Collections.Generic;
using System.Reflection;
using GameUtil;
using Google.Protobuf;


namespace GameNetWork
{

    public class NetworkModule
    {
		public static int SOCK_GAME_CHANNEL = 0;
		public static int SOCK_CHAT_CHANNEL = 1;
		public static int SOCK_CHANNEL_COUNT = 2;

//        private SockMsgRouter sockMsgRouter = null;
		protected SockMsgRouter[] routers;
        protected SockMgr sockMgr;


		public Action LostConnection = null;
		public Action Timeout = null;
//        public SockMsgRouter SockRouter
//        {
//            get { return sockMsgRouter; }
//        }
		private HttpMgr mHttpMgr;

		public NetworkModule()
		{
		}

		public NetworkModule(HttpMgr http)
		{
			mHttpMgr = http;
		}

        protected HttpMsgRouter httpMsgRouter = null;
        public HttpMsgRouter HttpRouter
        {
            get { return httpMsgRouter; }
        }

        #region common module
        public virtual void Start()
        {
            httpMsgRouter = new HttpMsgRouter(SysDef.HTTP_SERVER_URL);
			mHttpMgr.Router = httpMsgRouter;
//            sockMsgRouter = new SockMsgRouter();
//            sockMsgRouter.Start();
			sockMgr = new SockMgr();
			sockMgr.Start();

			routers = new SockMsgRouter[SOCK_CHANNEL_COUNT];
        }

        public virtual void ShutDown()
        {
            if (null != httpMsgRouter)
            {
                httpMsgRouter.Shutdown();
                httpMsgRouter = null;
            }
            
			for(int i = 0; i < routers.Length;i++)
			{
				SockMsgRouter sockMsgRouter = routers[i];
	            if (null != sockMsgRouter)
	            {
	                SockDisConnect(i);
	                sockMsgRouter.Shutdown();
					routers[i] = null;    
	            }
			}
			sockMgr.Shutdown ();
        }
        #endregion


        #region HTTP
        public virtual void RegisterHttpHandler<T>(HttpMsgRouter.MsgHandler handler) where T : IProtocolHead
        {
            UInt16 appCode = 0, funcCode = 0;
            byte flag = 0;
            GetProtoArgs<T>(out appCode, out funcCode, out flag);
            if (null == httpMsgRouter)
            {
                return;
            }

            httpMsgRouter.RegisterMsgHandler(appCode, funcCode, handler);
        }

        public virtual void UnRegisterHttpHandler<T>(HttpMsgRouter.MsgHandler handler) where T : IProtocolHead
        {
            if (null == httpMsgRouter)
            {
                return;
            }
            
            UInt16 appCode = 0, funcCode = 0;
            byte flag = 0;
            GetProtoArgs<T>(out appCode, out funcCode, out flag);

            httpMsgRouter.UnRegisterMsgHandler(appCode, funcCode, handler);
        }
        
//        public virtual void SendHttpMsg<T>(T obj)
//        {
//            if (null == httpMsgRouter)
//            {
//                return;
//            }
//            
//            string method = (string)GetStaticMethodInfo<T>("GetHttpMethod").Invoke(null, null);
//            
//            string directory = (string)GetStaticMethodInfo<T>("GetUrl").Invoke(null, null);
//            
//            string parameters = "";
//            string body = "";
//            
//            if ("GET" == method.ToUpper() || "DELETE" == method.ToUpper())
//            {
//                parameters = ProtoBufUtil.ToUrlParams<T>(obj);
//            }
//            else
//            {
//                body = ProtoBufUtil.ToJson<T>(obj);
//            }
//            
//            httpMsgRouter.SendMsg(method, directory, body, parameters);
//        }


		public virtual void SendHttpMsg<T>(T obj, string url, string method, bool getOriginData = false, bool needResp = true) where T: IProtocolHead, IMessage<T>
		{
			if (null == httpMsgRouter)
			{
				return;
			}
			string directory = url;
			
			string parameters = "";
			string body = "";

			UInt16 appCode = 0, funcCode = 0;
			byte flag = 0;
			GetProtoArgs<T>(out appCode, out funcCode, out flag);
			byte[] data = PackageUtils.Serialize2Binary<T> (obj, appCode, funcCode, PackageUtils.OpCode);

            if ("GET" == method.ToUpper() || "DELETE" == method.ToUpper())
			{
				parameters = ProtoBufUtil.ToUrlParams<T>(obj);
			}
			else
			{
				body =  "content=" + Convert.ToBase64String(data);//ProtoBufUtil.ToJson<T>(obj);                     
            }
			

			httpMsgRouter.SendMsg(method, directory, body, parameters,null,false,data, needResp);

		}

		public virtual void SendHttpMsg(string data, string url, string method, bool getOriginData = false, bool needResp = true)
		{
			if (null == httpMsgRouter)
			{
				return;
			}
			string directory = url;
			
			string parameters = "";
			string body = "";

			
			if ("GET" == method.ToUpper() || "DELETE" == method.ToUpper())
			{
//				parameters = ProtoBufUtil.ToUrlParams<T>(obj);
			}
			else
			{
				body =  "content=" + data;//ProtoBufUtil.ToJson<T>(obj);                     
			}
			byte[] bytes = null;  
			Compress.CompressByte( System.Text.Encoding.UTF8.GetBytes(data), out bytes);
			httpMsgRouter.SendMsg(method, directory, body, parameters,null,false, bytes, needResp);
			
		}
		public virtual void SendHttpMsg(string url, string method, HttpMsgRouter.MsgHandler callback = null, bool getOriginData = false)
		{
			if (null == httpMsgRouter)
			{
				return;
			}
			string directory = url;
			
			string parameters = "";
			string body = "";
			
//			if ("GET" == method.ToUpper() || "DELETE" == method.ToUpper())
//			{
//				parameters = ProtoBufUtil.ToUrlParams<T>(obj);
//			}
//			else
//			{
//				body = ProtoBufUtil.ToJson<T>(obj);
//			}
			
			httpMsgRouter.SendMsg(method, directory, body, parameters);
		}

        public void SendDebugHttpMsg(string obj, string url, string method, bool getOriginData = false, bool needResp = true)
        {
            if (null == httpMsgRouter)
            {
                return;
            }
            string directory = url;

            string parameters = "";
            string body = obj;
            byte[] data = System.Text.Encoding.UTF8.GetBytes(body);

            httpMsgRouter.SendMsg(method, directory, body, parameters, null, false, data, needResp);
        }

        #endregion


        #region SOCKET
		private SockMsgRouter GetRouter(int channel)
		{
			if (null == routers[channel])
			{
				routers[channel] = new SockMsgRouter();
				routers[channel].Start();
				routers[channel].blockGameMsgBack += RemoveGameMask;
				if(channel == SOCK_GAME_CHANNEL)
				{
					routers[channel].timeoutHandler += Timeout;
				}
			}
			return routers [channel];
		}
        public virtual void SockConnect(string ip, int port, int channel, Action callback, Action disconnected = null)
        {
			SockMsgRouter router = GetRouter (channel);
 
			StreamSock sock = sockMgr.Connect(ip, port, callback);
			sock.Router = router;
			router.ClearBuff (false);
			sock.socketDisconnection += disconnected;
        }

        public virtual void SockDisConnect(int channel = 0)
        {
			if (null == routers[channel])
            {
                return;
            }
			sockMgr.Disconnect(routers[channel].Owner);
			routers[channel].Owner = null;
			routers [channel].ClearBuff (false);
        }

		public bool IsConnected( int channel = 0)
		{
			if (null == routers[channel] || routers[channel].Owner == null)
			{
				return false;
			}

			return routers [channel].Owner.Connected;
		}

		protected virtual void DisConnected()
		{
			SockDisConnect ();
			if(LostConnection != null)
				LostConnection();
		}

        public virtual void RegisterSockHandler<T>(SockMsgRouter.MsgHandler handler, int channel = 0) where T : IProtocolHead
        {
			SockMsgRouter router = GetRouter (channel);
            UInt16 appCode = 0, funcCode = 0;
            byte flag = 0;
            GetProtoArgs<T>(out appCode, out funcCode, out flag);

			router.RegisterMsgHandler(appCode, funcCode, handler);
        }

        public virtual void RegisterChatSockHandler<T>(SockMsgRouter.MsgHandler handler) where T : IProtocolHead
        {
            RegisterSockHandler<T>(handler, SOCK_CHAT_CHANNEL);
        }

        public virtual void UnRegisterSockHandler<T>(SockMsgRouter.MsgHandler handler, int channel = 0) where T : IProtocolHead
        {
			if (null == routers[channel])
            {
                return;
            }
            UInt16 appCode = 0, funcCode = 0;
            byte flag = 0;
            GetProtoArgs<T>(out appCode, out funcCode, out flag);

			routers[channel].UnRegisterMsgHandler(appCode, funcCode, handler);
        }

        public virtual void UnRegisterChatSockHandler<T>(SockMsgRouter.MsgHandler handler) where T : IProtocolHead
        {
            UnRegisterSockHandler<T>(handler, SOCK_CHAT_CHANNEL);
        }

        #region operationErrorHandle
        public virtual void RegisterOperationErrorSockHandler<T>(SockMsgRouter.OperateErrorHandler handler, int channel = 0) where T : IProtocolHead
        {
			SockMsgRouter router = GetRouter (channel);
            UInt16 appCode = 0, funcCode = 0;
            byte flag = 0;
            GetProtoArgs<T>(out appCode, out funcCode, out flag);

            router.RegisterOperationErrorMsgHandler(appCode, funcCode, handler);
        }

        public virtual void UnRegisterOperationErrorSockHandler<T>(SockMsgRouter.OperateErrorHandler handler, int channel = 0) where T : IProtocolHead
        {
            if (null == routers[channel])
            {
                return;
            }
            UInt16 appCode = 0, funcCode = 0;
            byte flag = 0;
            GetProtoArgs<T>(out appCode, out funcCode, out flag);

            routers[channel].UnRegisterOperationErrorMsgHandler(appCode, funcCode, handler);
        }
        #endregion

		public virtual void SendSockMsg<T>(T msg, int channel = 0, bool blockGame = false,Action<IProtocolHead,Operation> callback = null) where T : IProtocolHead, IMessage<T>
        {
			if (null == routers[channel])
            {
                return;
            }
            UInt16 appCode = 0, funcCode = 0;
			int opCode = 0;
            byte flag = 0;
            GetProtoArgs<T>(out appCode, out funcCode, out flag);

			routers[channel].SendMsg<T>(msg, appCode, funcCode, opCode, blockGame,callback);

			if(blockGame)
			{
//				UIManager.Instance.ShowBlockMask();
			}

        }

		public void SendSockRequest(string route)
        {

        }

		public void SendHandShake()
        {
			routers[0].SendHandShake();
		}

        public virtual void SendChatSockMsg<T>(T msg) where T : IProtocolHead,IMessage<T>
        {
            SendSockMsg<T>(msg, SOCK_CHAT_CHANNEL);
        }


        public virtual void SendSockMsg(byte[] data, int channel)
		{
			if (null != data)
				routers[channel].Send (data);
		}

		public void ResyncMsg(int channel = 0)
		{
			routers [channel].ResendAllPendingMsg ();
		}

		public virtual void Update()
		{
			if (sockMgr != null)
				sockMgr.Update ();

			for(int i = 0; i < routers.Length;i++)
			{
				SockMsgRouter sockMsgRouter = routers[i];
				if (null != sockMsgRouter)
				{
					sockMsgRouter.Update();  
				}
			}
		}

		public string HttpUrl
		{
			set
			{
				if(httpMsgRouter != null)
					httpMsgRouter.UrlPrefix = value;
			}
		}

		public void FakeRecieveSockMsg(IProtocolHead ph, int funcCode, int channel = 0)
		{
			routers [channel].FakeRecieveMsg (ph, funcCode);
		}
		
		#endregion
		
		#region internal
		private void GetProtoArgs<T>(out UInt16 appCode, out UInt16 funcCode, out byte flag) where T : IProtocolHead
        {
            appCode = (UInt16)GetStaticMethodInfo<T>("GetAppCode").Invoke(null, null);
            
            funcCode = (UInt16)GetStaticMethodInfo<T>("GetFnCode").Invoke(null, null);
            
            flag = (byte)GetStaticMethodInfo<T>("GetFlag").Invoke(null, null);
        }
        
        private Dictionary<string, System.Reflection.MethodInfo> staticMethodDict = new Dictionary<string, System.Reflection.MethodInfo>();
        
        private System.Reflection.MethodInfo GetStaticMethodInfo<T>(string methodName)
        {
            string key = typeof(T).ToString() + "-" + methodName;
            System.Reflection.MethodInfo ret = null;
            
            if (!staticMethodDict.ContainsKey(key))
            {
                ret = typeof(T).GetMethod(methodName);
                if (null == ret)
                {
                    throw new System.Exception("Can't find static method info! type = " + typeof(T).Name + ", methodName = " + methodName);
                }
                staticMethodDict[key] = ret;
            }
            else
            {
                ret = staticMethodDict[key];
            }
            return ret;
        }

		private void RemoveGameMask()
		{
//			UIManager.Instance.RemoveBlockMask ();
		}

        #endregion
    }
}