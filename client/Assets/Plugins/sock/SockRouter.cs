using System;
using System.Net.Sockets;
using System.Collections;
using System.Collections.Generic;
using System.Net;

namespace GameNetWork
{
    public abstract class SockRouter
    {
        private object syncRoot = new object();
        private SocketError sockError = SocketError.Success;

        public  StreamSock Owner
        {
            get;
            set;
        }

        public virtual void Start() {}

        public virtual void Shutdown()
        {
            lock (syncRoot)
            {
                sockError = SocketError.Success;
            }
        }

        public virtual void Send(byte[] msg)
        {
            Logger.LogWarning("unimplemented send");
        }

        public virtual void Dispatch()
        {
            //dispatch msg
            DoDispatchMsg();
            //dispatch error
            if (sockError != SocketError.Success)
            {
                HandleError(sockError);
                lock (syncRoot)
                {
                    sockError = SocketError.Success;
                }
            }
        }

        protected virtual void DoDispatchMsg()
        {
            // this can happen if sock is disconnected.
            //Logger.LogWarning("unimplemented doDispatchMsg");
        }

        public virtual byte[] GetPendingSend()
        {
            return null;
        }

        public virtual void OnRecvData(byte[] msg, SocketError error)
        {
            if (error  != SocketError.Success)
            {
                lock (syncRoot)
                {
                    sockError = error;
                }
            }
            else
            {
                DoRecvData(msg);
            }
        }

        protected virtual void DoRecvData(byte[] msg)
        {
            Logger.LogWarning("unimplemented DoRecvData");
        }

        protected virtual void HandleMsg(byte[] msg)
        {
            Logger.LogWarning("unimplemented handle msg");
        }

        protected virtual void HandleError(SocketError error)
        {
            Logger.LogWarning("unimplemented handle error");
        }
    }
}