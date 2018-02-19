using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace GameNetWork
{
    public abstract class HttpRouter
    {
        protected class ErrorWrap
        {
            public HttpRequest req;
            public HttpResponse rsp;

            public ErrorWrap(HttpRequest req, HttpResponse rsp)
            {
                this.req = req;
                this.rsp = rsp;
            }
        }

        protected class MsgWrap
        {
            public HttpRequest req;
            public HttpResponse rsp;

            public MsgWrap(HttpRequest req, HttpResponse rsp)
            {
                this.req = req;
                this.rsp = rsp;
            }
        }

        protected Queue<ErrorWrap> errorQ = new Queue<ErrorWrap>();
        protected Queue<MsgWrap> msgQ = new Queue<MsgWrap>();

        public virtual void Start() {}

        public virtual void Shutdown()
        {
            lock (((ICollection)msgQ).SyncRoot)
            {
                msgQ.Clear();
            }
            lock (((ICollection)errorQ).SyncRoot)
            {
                errorQ.Clear();
            }
        }

        public virtual void Dispatch()
        {
            //dispatch msg
            while (true)
            {
                if (msgQ.Count <= 0)
                {
                    break;
                }

                MsgWrap mwrap = null;
                lock (((ICollection)msgQ).SyncRoot)
                {
                    mwrap = msgQ.Dequeue();
                }
                if (mwrap == null)
                {
                    Logger.LogWarning("empty data for msg parse");
                    continue;
                }

                HandleMsg(mwrap.req.url, mwrap);
            }

            //dispatch error
            while (true)
            {
                if (errorQ.Count <= 0)
                {
                    break;
                }
                ErrorWrap wrap = null;
                lock (((ICollection)errorQ).SyncRoot)
                {
                    wrap = errorQ.Dequeue();
                }
                if (wrap == null)
                {
                    Logger.LogWarning("empty error");
                    continue;
                }
                HandleError(wrap.req.url, wrap.rsp.status, wrap.rsp.httpStatusCode);
            }
        }

        public virtual void OnRecvMsg(HttpRequest req, HttpResponse rsp)
        {
            if (rsp.status == HttpResponse.ST_OK)
            {
                lock (((ICollection)msgQ).SyncRoot)
                {
                    msgQ.Enqueue(new MsgWrap(req, rsp));
                }
            }
            else
            {
                lock (((ICollection)errorQ).SyncRoot)
                {
                    errorQ.Enqueue(new ErrorWrap(req, rsp));
                }
            }
        }

		protected virtual void HandleMsg(string url, MsgWrap mwrap)
        {
            Logger.LogWarning("not implenented handle msg");
        }

        protected virtual void HandleError(string url, int status, int httpStatusCode)
        {
            Logger.LogWarning("not implemented handle error");
        }
    }
}