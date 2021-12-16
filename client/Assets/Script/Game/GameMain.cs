using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameNetWork;
using MVC;

public class GameMain : Singleton<GameMain>
{
    GameNetWork.NetworkModule network = null;
    public MVC.MediatorMgr mediatorMgr { get; private set; }
    public override void Awake()
    {
        base.Awake();
        mediatorMgr = new MVC.MediatorMgr();
        mediatorMgr.Start();
    }

    void Start()
    {
        Math.Grid grid = new Math.Grid()
        {
            X = 12,
            Y = 13,
        };

       //byte[] data = GameUtil.DataParser.Serialize<Math.Grid>(grid);
       // Math.Grid grid2 = GameUtil.DataParser.Deserialize<Math.Grid>(data);
       // Debug.Log(grid2);
        //   network.RegisterSockHandler<gameprotos.UserMessage>(ShowNewMessage);
        Invoke("InitSystem", 0.1f);
    }

    private void OnConnectedToServer()
    {
        Debug.Log("connected");
        network.SendHandShake();
        
    }
    // Update is called once per frame
    void Update()
    {
    }



    void ShowNewMessage(SockMsgRouter router, IProtocolHead ph)
    {
        Debug.Log("get message: " + (ph as GameProtos.common.UserMessage).Content);
    }

    void InitSystem()
    {
        RegisterProxies();
    }

    void RegisterProxies()
    {
        ProxyMgr.Instance.Register(new AccountProxy());
        ProxyMgr.Instance.Register(new RoomProxy());
    }
}
