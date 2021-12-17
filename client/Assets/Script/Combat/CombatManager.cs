using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MVC;

public class CombatManager : Mediator
{
    public GameObject playerPrefab;
    private Dictionary<int, Player> players = new Dictionary<int, Player>();
    public override void Start()
    {
        base.Start();
        GameMain.Instance.mediatorMgr.Register("CombatManager", this);
    }

    protected override void RegisterEventHandler()
    {
        base.RegisterEventHandler();
        RegisterEventHandler(RoomEvent.Join, OnJoinSucess);
        RegisterEventHandler(RoomEvent.AllMembers, OnAllMembers);

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
        int myID = proxyMgr.GetProxy<AccountProxy>().ID;
        player.Init(playerData.Uid, playerData.Name, newUser.Uid == myID);
        players.Add(newUser.Uid, player);
    }

    void OnAllMembers(GameEngine.Event evt)
    {
        var members  = (evt as RoomEvent).allMembers.Members;
        int myID = proxyMgr.GetProxy<AccountProxy>().ID;

        for (int i = 0; i < members.Count; i++)
        {
            int uid = int.Parse(members[i]);
            if (players.ContainsKey(uid))
                continue;
            Player player = Instantiate(playerPrefab).GetComponent<Player>();
            player.Init(uid, members[i], myID == uid);
            players.Add(uid, player);
        }
    }

    void OnPlayerMove(GameEngine.Event evt)
    {
        GameProtos.common.Move move = (evt as PlayerActionEvent).moveData;
        int myID = proxyMgr.GetProxy<AccountProxy>().ID;
        if (myID != move.Id)
            players[move.Id].SyncPosition(new Vector3(move.X, move.Y, move.Z));
    }
}
