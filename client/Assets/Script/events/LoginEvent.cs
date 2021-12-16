using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoginEvent : GameEngine.Event
{
    public static string Login = "Login";
    public GameProtos.account.LoginResponse loginData;
    public LoginEvent(string name, GameProtos.account.LoginResponse data):base(name)
    {
        loginData = data;
    }
}
