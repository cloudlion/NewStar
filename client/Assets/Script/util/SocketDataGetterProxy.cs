using UnityEngine;
using System.Collections;
#if MVC
using MVC;
using System;

public class SocketDataGetterProxy<Request, Response> : Proxy where Request : IProtocolHead where Response : IProtocolHead
{
    private Action<GameNetWork.SockMsgRouter, IProtocolHead> mAction;

    public void SockRequest(Request proto, Action<GameNetWork.SockMsgRouter, IProtocolHead> action)
    {
        Register();

        SendSockMsg<Request>(proto);
        mAction = action;
    }


    private void Register()
    {
        ProxyMgr.Instance.Register(this.GetHashCode().ToString(),this);
        this.Start();
        RegisterSockHandler<Response>(OnReceive);
    }



    private void UnRegister()
    {
        UnRegisterSockHandler<Response>(OnReceive);
        ProxyMgr.Instance.UnRegister(this.GetHashCode().ToString());
    }


    private void OnReceive(GameNetWork.SockMsgRouter router, IProtocolHead ph)
    {
        if (mAction != null)
            mAction(router, ph);
        UnRegister();
    }
}


#endif 