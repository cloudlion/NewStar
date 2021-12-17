using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MVC;
using GameProtos.common;
using GameNetWork;



public class RoomProxy : Proxy
{
    public class Player
    {
        public string playerName;
        public Vector3 position;
        public int uid;
    }

    Dictionary<int, Player> players = new Dictionary<int, Player>();
    protected override void RegisterNetworkHandler()
    {
        base.RegisterNetworkHandler();
        RegisterSockHandler<Move>(OnPlayerMove);
        RegisterSockHandler<NewUser>(OnNewUser);
        RegisterSockHandler<AllMembers>(OnAllMembers);
    }

    public void Move(int playerId, Vector3 pos)
    {
        Move data = new Move();
        data.Id = playerId;
        data.X = pos.x;
        data.Y = pos.y;
        data.Z = pos.z;
        SendSockMsg(data);
    }

    public void JoinRoom()
    {
        Debug.Log("join room");
        GameProtos.common.NewUser newUser = new GameProtos.common.NewUser();
        SendSockMsg(newUser);
    }

    void OnNewUser(SockRouter router, IProtocolHead ph)
    {
        NewUser data = ph as NewUser;
        if(!players.ContainsKey(data.Uid))
        {
            players.Add(data.Uid, new Player
            {
                playerName = data.Name,
                uid = data.Uid
            });
        }

        SendEvent(new RoomEvent(RoomEvent.Join, data));
    }

    void OnAllMembers(SockRouter router, IProtocolHead ph)
    {
        AllMembers data = ph as AllMembers;
        for (int i = 0; i < data.Members.Count; i++)
        {
            int uid = int.Parse(data.Members[i]);
            if (players.ContainsKey(uid))
                continue;
            players.Add(uid, new Player
            {
                playerName = data.Members[i],
                uid = uid
            });
        }
        SendEvent(new RoomEvent(RoomEvent.AllMembers, data));
    }

    void OnPlayerMove(SockRouter router, IProtocolHead ph)
    {
        Move data = ph as Move;
        SendEvent(new PlayerActionEvent(PlayerActionEvent.Move, data));
    }
}
