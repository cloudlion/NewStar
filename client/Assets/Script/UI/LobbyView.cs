using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MVC;

public class LobbyView : Mediator
{
    public GameObject btnJoin;

    public override void Start()
    {
        base.Start();
        GameMain.Instance.mediatorMgr.Register("LobbyView", this);
    }

    protected override void RegisterEventHandler()
    {
        base.RegisterEventHandler();
        RegisterEventHandler(RoomEvent.Join, OnJoinSucess);
    }

    protected override void UnregisterEventHandler()
    {
        base.UnregisterEventHandler();
        UnregisterEventHandler(RoomEvent.Join, OnJoinSucess);
    }

    public void OnJoinClicked()
    {
        proxyMgr.GetProxy<RoomProxy>().JoinRoom(); 
    }

    void OnJoinSucess(GameEngine.Event loginEvent)
    {
        btnJoin.SetActive(false);
    }
}
