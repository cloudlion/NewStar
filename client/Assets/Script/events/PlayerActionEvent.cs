using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActionEvent : GameEngine.Event
{
    public static string Move = "Move";
    public GameProtos.common.Move moveData;
    public PlayerActionEvent(string name, GameProtos.common.Move data):base(name)
    {
        moveData = data;
    }
}
