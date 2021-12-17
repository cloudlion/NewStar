using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomEvent : GameEngine.Event
{
    public static string Join = "Join";
    public static string AllMembers = "AllMembers";

    public GameProtos.common.NewUser newUser;
    public GameProtos.common.AllMembers allMembers;
    public RoomEvent(string name, GameProtos.common.NewUser data):base(name)
    {
        newUser = data;
    }

    public RoomEvent(string name, GameProtos.common.AllMembers data) : base(name)
    {
        allMembers = data;
    }
}
