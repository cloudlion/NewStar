using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading;
using UnityEngine;
using GameUtil;
using System.Runtime.InteropServices;

namespace GameNetWork
{
    public class StreamSock
    {
		public Action socketDisconnection = null;
        private bool close;
        private volatile bool interrupted;

        private SockRouter router = new NullSockRouter();

        private Thread recvThread;
        private Thread sendThread;

        private Socket sock = null;
        private object syncRoot = new object();

        private string ip = "";
        private int port;
        
        private byte[] recvBuffer = new byte[32 * 1024];

		public Action OnConnected;

		[DllImport ("__Internal")]
		private static extern string getIPv6 (string host, string port);
		public static string GetIpAddress(string host,string port)
		{
			return getIPv6 (host, port);
		}

        public string IP
        {
            get
            {
                return ip;
            }
        }
        public int Port
        {
            get
            {
                return port;
            }
        }

        public bool Closed
        {
            get
            {
                return close;
            }
        }
        
        public bool Connected
        {
            get
            {
                if (sock == null)
                {
                    return false;
                }
                if (!sock.Connected)
                {
                    return false;
                }
                if (interrupted)
                {
                    return false;
                }
                return true;
            }
        }

        public SockRouter Router
        {
            get
            {
                return router;
            }
            set
            {
                router.Shutdown();
                router = value;
                if (router == null)
                {
                    router = new NullSockRouter();
                }
                router.Owner = this;
                router.Start();
            }
        }

        public void Connect(string ip, int port, Action callback)
        {
            try
            {
				this.ip = ip;
				this.port = port;
				if(Debug.isDebugBuild)
				{
					Debug.Log("host name===="+ip+"===port==="+port);
				}
				Dns.BeginGetHostAddresses(ip, GetHostAddressesCallback, null);
//				IPAddress addr = IPAddress.Parse(ip);
//				IPEndPoint ipe = new IPEndPoint(addr, port);
//				lock (syncRoot)
//				{
//					sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
//					sock.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, true);
//					sock.NoDelay = true;
//					sock.SendBufferSize = 1024 * 32;
//					sock.SendTimeout = 10 * 1000; //10s
//					sock.Blocking = false;
//				}

				OnConnected = callback;
//				sock.BeginConnect(ipe, Connect_Callback, sock);
				
				close = false;
				interrupted = false;
			    

            }
			catch (SocketException ex)
            {
                Logger.LogWarning("socket connect failed!");
                Logger.Log(ex.ToString());
                HandleException(ex);

                //this is used for non-blocking connect
                // if (ex.ErrorCode != 10035)
                // {
                //     //do nothing
                //     Logger.LogException(ex);
                // }
				interrupted = true;
//                Close();
            }
        }

		public void GetHostAddressesCallback(IAsyncResult ar)
		{
			Debug.Log ("GetHostAddresses call back ");

			try
			{
				IPAddress addr = null;
				#if UNITY_EDITOR || UNITY_ANDROID
				IPAddress[] addrs = Dns.EndGetHostAddresses (ar);
				addr = addrs [0];
				#else
				string iosIp = GetIpAddress(ip,port.ToString());
				IPAddress.TryParse(iosIp,out addr);
				#endif
				Debug.Log("ip==="+addr.ToString()+"====port==="+port);
				AddressFamily af = AddressFamily.InterNetwork;
				if(addr.AddressFamily == AddressFamily.InterNetworkV6)
				{
					af = AddressFamily.InterNetworkV6;
				}
				IPEndPoint ipe = new IPEndPoint(addr, port);

				lock (syncRoot)
				{
					sock = new Socket(af, SocketType.Stream, ProtocolType.Tcp);
					sock.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, true);
					sock.NoDelay = true;
					sock.SendBufferSize = 1024 * 32;
					sock.SendTimeout = 30 * 1000; //10s
					sock.Blocking = false;
				}

				sock.BeginConnect(ipe, Connect_Callback, sock);
			
				close = false;
				interrupted = false;
			}
			catch (Exception e)
			{
				Debug.Log ("error===" + e.Message);
				Logger.LogWarning("socket connect failed!" + e.ToString());
				interrupted = true;
			}
		}       
		
		void Connect_Callback(IAsyncResult iar)
		{
			Socket client=(Socket)iar.AsyncState;
			try
			{
				client.EndConnect(iar);
				recvThread = new Thread(new ThreadStart(this.Recv));
				recvThread.Start();
				
				sendThread = new Thread(new ThreadStart(this.Send));
				sendThread.Start();
//				if(OnConnected != null)OnConnected();
			}
			catch (Exception e)
			{
				Logger.LogWarning("socket connect failed!");
				interrupted = true;
			}
			finally
			{
				
			}
		}
		
		public void Close()
        {
            close = true;
            interrupted = false;

            Router = null;

            Clear();

            if (sock == null)
            {
                return;
            }
            try
            {
                lock (syncRoot)
                {
                    sock.Shutdown(SocketShutdown.Both);
                    sock.Close();
                    sock = null;
                }
                ip = "";
                port = 0;
            }
            catch (SocketException ex)
            {
                if (ex.ErrorCode == 10058)
                {
                    Logger.LogWarning("close sock which is not connected");
                }
                else
                {
                    Logger.LogWarning("close error: " + ex.ErrorCode);
                }
            }
        }

        public void Update()
        {
            router.Dispatch();
            if (interrupted && !close)
            {
                Close();
        //        App.Game.EventMgr.HandleEvent(new NetworkEvent(NetworkEvent.SOCKET_INTERRUPTED));
				if(socketDisconnection != null)
					socketDisconnection.Invoke();
				socketDisconnection = null;
            }

			if(Connected && OnConnected != null)
			{
				OnConnected();
				OnConnected = null;
			}


        }

        private void Clear()
        {
            lock (((ICollection)recvBuffer).SyncRoot)
            {
                Array.Clear(recvBuffer, 0, recvBuffer.Length);
            }
        }

        private void HandleException(SocketException ex)
        {
            router.OnRecvData(null, ex.SocketErrorCode);
        }

        private void HandleError(SocketError err)
        {
            router.OnRecvData(null, err);
        }

        private void Recv()
        {
            Logger.Log("sock recv thread start...");
            while (!close && !interrupted)
            {
                try
                {
                    long interval = DateTime.Now.Ticks;

                    if (sock == null)
                    {
						Logger.LogWarning("sock is null by not close");
                        break;
                    }
                    else if (!Connected)
                    {
                        Logger.LogWarning("sock disconnected by not close");
                        break;
                    }
                    else
                    {
                        OnRecv();
                    }

                    interval = System.DateTime.Now.Ticks - interval;
                    int sleep = 10 - (int)SysUtil.TickToMilliSec(interval);
                    if (sleep > 0)
                    {
                        Thread.Sleep(sleep);
                    }
                }
                catch (System.Exception ex)
                {
                    if (!(ex is System.Threading.ThreadAbortException))
                    {
                        Logger.LogError(ex);
                    }
                    else
                    {
                        Logger.Log("recv thread abort");
                    }
                    interrupted = true;
                }
            }
			interrupted = true;
			Logger.Log("sock recv thread end normally");

        }

        private void Send()
        {
			Logger.Log("sock send thread start...");
            while (!close && !interrupted)
            {
                try
                {
                    long interval = DateTime.Now.Ticks;

                    if (sock == null)
                    {
                        Logger.Log("sock is null by not close");
                        break;
                    }
                    else if (!Connected)
                    {
                        Logger.LogWarning("sock disconnected by not close");
                        break;
                    }
                    else
                    {
                        OnSend();
                    }

                    interval = System.DateTime.Now.Ticks - interval;
                    int sleep = 10 - (int)SysUtil.TickToMilliSec(interval);
                    if (sleep > 0)
                    {
                        Thread.Sleep(sleep);
                    }
                }
                catch (System.Exception ex)
                {
                    if (!(ex is System.Threading.ThreadAbortException))
                    {
                        Logger.LogError(ex);
                    }
                    else
                    {
                        Logger.Log("send thread abort");
                    }
                    interrupted = true;
                }
            }
            Logger.Log("sock send thread end normally");
        }


        public virtual void OnRecv()
        {
            int recvSize = 0;
            try
            {
                lock (syncRoot)
                {
                    if (null == sock)
                    {
                        interrupted = true;
                        return;
                    }
                    if (!sock.Poll(10, SelectMode.SelectRead))
                    {
                        return;
                    }
                    
                    int s = sock.Receive(recvBuffer,
                                         0,
                                         recvBuffer.Length,
                                         SocketFlags.None);
                    if (s == 0)
                    {
                        interrupted = true;
                        return;
                    }
                    recvSize = s;
                }
            }
            catch (SocketException ex)
            {
                if (close)
                {
                    if (ex.ErrorCode == 10004)
                    {
                        Logger.Log("recv data interrupted");
                    }
                    else if (ex.ErrorCode == 10038)
                    {
                        Logger.Log("recv data in closed sock");
                    }
                    else
                    {
                        Logger.LogError("recv error: " + ex.ErrorCode);
                    }
                }
                else
                {
                    HandleException(ex);
                }
            }
            
            if (recvSize > 0)
            {
                byte[] value = new byte[recvSize];
                lock (((ICollection)recvBuffer).SyncRoot)
                {
                    Array.Copy(recvBuffer, 0, value, 0, recvSize);
                }
                router.OnRecvData(value, SocketError.Success);
            }
        }

        private void OnSend()
        {
            byte[] msg = this.router.GetPendingSend();
            if (null == msg)
            {
                return;
            }
            try
            {
                bool canWrite = false;
                lock (syncRoot)
                {
                    canWrite = sock.Poll(10, SelectMode.SelectWrite);
                    if (!canWrite)
                    {
                        interrupted = true;
                        return;
                    }
                    
                    int msgLen = msg.Length;
                    int size = sock.Send(msg, 0, msgLen, SocketFlags.None);
                    if (size < msgLen)
                    {
                        int pos = size;
                        while (pos < msgLen)
                        {
                            size = sock.Send(msg, pos, msgLen - pos, SocketFlags.None);
                            pos += size;
                        }
                    }
                }
            }
            catch (SocketException ex)
            {
                if (close)
                {
					Logger.LogWarning("socket send error: " + ex.ErrorCode);
                }
                else
                {
                    HandleException(ex);
                }
                interrupted = true;
            }
        }
    }
}
