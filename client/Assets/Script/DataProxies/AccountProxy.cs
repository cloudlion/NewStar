using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameNetWork;
using GameProtos.account;

public class AccountProxy : MVC.Proxy
{
    public int ID { get; private set; }
    public string UserName { get; private set; }
    protected override void RegisterNetworkHandler()
    {
        base.RegisterNetworkHandler();
        RegisterSockHandler<GameProtos.account.LoginResponse>(OnLogin);
    }
    public void Login(int mid, string password)
    {
        Login msg = new Login();
        msg.Id = mid;
        msg.Password = password;
        SendSockMsg<Login>(msg);
    }

    public void Register(string userName, string password)
    {
        Register msg = new Register();
        msg.Username = userName;
        msg.Password = password;
        SendSockMsg<Register>(msg);
    }

    void OnLogin(SockRouter router, IProtocolHead ph)
    {
        Debug.Log("login success");
        LoginResponse data = ph as LoginResponse;
        if(data.Error == 0)
        {
            ID = data.Id;
            UserName = data.Username;
            PlayerPrefs.SetInt("MasterID", ID);
            PlayerPrefs.SetString("username", UserName);
            PlayerPrefs.SetString("password", data.Password);
            PlayerPrefs.Save();
        }
        SendEvent(new LoginEvent(LoginEvent.Login, data));
    }

}
