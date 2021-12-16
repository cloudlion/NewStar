using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomEvent : GameEngine.Event
{
    public static string Join = "Join";
    public GameProtos.common.NewUser newUser;
    public RoomEvent(string name, GameProtos.common.NewUser data):base(name)
    {
        newUser = data;
    }
}
