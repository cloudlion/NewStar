using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GameNetWork;
using System;
using GameEngine;
using Google.Protobuf;

namespace MVC
{
    public class ProxyMgr : Singleton<ProxyMgr> {

        public EventMgr EventManager = new EventMgr();

        protected Dictionary<string, Proxy> proxyMap;
		protected List<Proxy> proxyList;
        NetworkModule network = null;
        // Use this for initialization
        public override void Awake()
        {
            base.Awake();
            network = new NetworkModule(new GameNetWork.HttpMgr());
            network.Start();
            network.SockConnect("127.0.1", 3250, 0, () => { OnConnectedToServer(); }, null);
        }
        public virtual void Start()
        {

			proxyMap = new Dictionary<string, Proxy> ();
			proxyList = new List<Proxy> ();
            CutoffNetwork = false;
            
        }
        public bool IsReady()
        {
            return proxyMap != null;
        }
             
        public void Tranverse(Action<string,Proxy> action)
        {
            if (action == null) return;
            foreach (KeyValuePair<string, Proxy> pair in proxyMap)
            {
                action(pair.Key, pair.Value);
            }
        }

		// Update is called once per frame
		public virtual void Update () 
        {
            network.Update();
            EventManager.Update();
            for (int i = 0; i< proxyList.Count;i++) {
				var p = proxyList [i];
				p.Update ();
			}
        }


		public virtual void Register(Proxy proxy)
		{
            Register(proxy.GetType().Name,proxy);
            //if (proxyMap.ContainsKey (proxy.GetType().Name)) 
            //{
            //    Logger.Log("Proxy id is existed: " + proxy.GetType().Name);
            //    return;
            //}

            //proxyMap.Add(proxy.GetType().Name, proxy);
            //proxy.OnRegister (this);
            //proxy.Start ();

		}

        public virtual void Register(string id, Proxy proxy)
        {
            if (proxyMap.ContainsKey(id))
            {
                Logger.Log("Proxy id is existed: " + id);
                return;
            }

            proxyMap.Add(id, proxy);
			proxyList.Add (proxy);
            if (proxy != null)
                proxy.OnRegister(this);
            proxy.Start();
        }
        public void UnRegister<T>()
        {
            UnRegister(typeof(T).Name);
        }

		public void UnRegister(string id)
		{
            if (!proxyMap.ContainsKey(id)) return;
            proxyMap[id].OnRemove();
			Proxy p = proxyMap [id];
			proxyMap.Remove (id);
			proxyList.Remove (p);
		}
        private Proxy GetProxy(Type type)
        {
            return GetProxy(type.Name);
        }
		private Proxy GetProxy(string id)
		{
			Proxy p = null;
			proxyMap.TryGetValue(id, out p);
			
			return p;
		}
        public T GetProxy<T>() where T : Proxy
        {
            return GetProxy(typeof(T).Name) as T;
        }

		public void SendEvent(GameEngine.Event e)
		{
            EventManager.SendEvent(e);
		}
        public void RegisterSockHandler<T>(SockMsgRouter.MsgHandler handler, int channel = 0) where T : IProtocolHead
		{
            network.RegisterSockHandler<T>(handler, channel);
		}
		
		public void UnRegisterSockHandler<T>(SockMsgRouter.MsgHandler handler, int channel = 0) where T : IProtocolHead
        {
            network.UnRegisterSockHandler<T>(handler, channel);
            
        }

        #region operationErrorHandle
        public void RegisterOperationErrorSockHandler<T>(SockMsgRouter.OperateErrorHandler handler, int channel = 0) where T : IProtocolHead
        {
            network.RegisterOperationErrorSockHandler<T>(handler, channel);
        }

        public void UnRegisterOperationErrorSockHandler<T>(SockMsgRouter.OperateErrorHandler handler, int channel = 0) where T : IProtocolHead
        {
            network.UnRegisterOperationErrorSockHandler<T>(handler, channel);
        }
        #endregion


        public void RegisterHttpHandler<T>(HttpMsgRouter.MsgHandler handler) where T : IProtocolHead
        {
            network.RegisterHttpHandler<T>(handler);
        }
        public void UnRegisterHttpHandler<T>(HttpMsgRouter.MsgHandler handler) where T : IProtocolHead
        {
            network.UnRegisterHttpHandler<T>(handler);
        }


        public void SendSockMsg<T>(T msg, int channel = 0, bool blockGame = false,Action<IProtocolHead,Operation> callback = null) where T : IProtocolHead, IMessage<T>
        {
			if (CutoffNetwork)
				return;
            network.SendSockMsg<T>(msg, channel, blockGame, callback);
        }

        public void ShutDwon()
		{
			foreach (IProxy p in proxyMap.Values) 
			{
				p.OnRemove();
			}
			proxyMap.Clear ();
		}

		public bool CutoffNetwork {
			get;
			set;
		}

        private void OnConnectedToServer()
        {
            Debug.Log("connected");
            network.SendHandShake();
        }
    }
}
