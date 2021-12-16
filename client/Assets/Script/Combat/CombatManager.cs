using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MVC;

public class CombatManager : Mediator
{
    public GameObject playerPrefab;
    private Dictionary<int, Player> players;
    public override void Start()
    {
        base.Start();
        GameMain.Instance.mediatorMgr.Register("CombatManager", this);
    }

    protected override void RegisterEventHandler()
    {
        base.RegisterEventHandler();
        RegisterEventHandler(RoomEvent.Join, OnJoinSucess);
        RegisterEventHandler(PlayerActionEvent.Move, OnPlayerMove);
    }

    protected override void UnregisterEventHandler()
    {
        base.UnregisterEventHandler();
        UnregisterEventHandler(RoomEvent.Join, OnJoinSucess);
    }

    void OnJoinSucess(GameEngine.Event evt)
    {
        GameProtos.common.NewUser newUser = (evt as RoomEvent).newUser;
        Debug.Log("new user: " + newUser.Name);
        GameProtos.common.NewUser playerData = (evt as RoomEvent).newUser;
        Player player = Instantiate(playerPrefab).GetComponent<Player>();
        player.Init(playerData.Uid, playerData.Name);
    }

    void OnPlayerMove(GameEngine.Event evt)
    {
        GameProtos.common.Move move = (evt as PlayerActionEvent).moveData;
        int myID = proxyMgr.GetProxy<AccountProxy>().ID;
        if (myID != move.Id)
            players[move.Id].SyncPosition(new Vector3(move.X, move.Y, move.Z));
    }
}
