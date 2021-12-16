using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MVC;
using UnityEngine.SceneManagement;

public class LoginView : Mediator
{
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        GameMain.Instance.mediatorMgr.Register("LoginView", this);
    }

    protected override void RegisterEventHandler()
    {
        base.RegisterEventHandler();
        RegisterEventHandler(LoginEvent.Login, OnLoginSucess);
    }

    protected override void UnregisterEventHandler()
    {
        base.UnregisterEventHandler();
        UnregisterEventHandler(LoginEvent.Login, OnLoginSucess);
    }

    public void OnLoginClicked()
    {
        string userName = "";
        string password = "";
        int id = 0;
        if (PlayerPrefs.HasKey("MasterID"))
        {
            userName = PlayerPrefs.GetString("username");
            password = PlayerPrefs.GetString("password");
            id = PlayerPrefs.GetInt("MasterID");
            ProxyMgr.Instance.GetProxy<AccountProxy>().Login(id, password);
        }
        else
            ProxyMgr.Instance.GetProxy<AccountProxy>().Register(userName, password);
    }

    void OnLoginSucess(GameEngine.Event loginEvent)
    {
        SceneManager.LoadScene("Game");
    }

    private void OnGUI()
    {
        if (GUI.Button(new Rect(100, 600, 300, 100), "clear"))
            PlayerPrefs.DeleteAll();
    }

}
