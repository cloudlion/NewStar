using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameNetWork;

public class GameMain : MonoBehaviour
{
    GameNetWork.NetworkModule network = null;
    // Start is called before the first frame update
    void Start()
    {
        Math.Grid grid = new Math.Grid()
        {
            X = 12,
            Y = 13,
        };

       byte[] data = GameUtil.DataParser.Serialize<Math.Grid>(grid);
        Math.Grid grid2 = GameUtil.DataParser.Deserialize<Math.Grid>(data);
        Debug.Log(grid2);
        network = new GameNetWork.NetworkModule(new GameNetWork.HttpMgr());
        network.Start();
        network.SockConnect("127.0.1",3250, 0, ()=> { OnConnectedToServer(); }, null);
        network.RegisterSockHandler<gameprotos.UserMessage>(ShowNewMessage);
     //   network.RegisterSockHandler<gameprotos.UserMessage>(ShowNewMessage);
    }

    private void OnConnectedToServer()
    {
        Debug.Log("connected");
        network.SendHandShake();
        
    }
    // Update is called once per frame
    void Update()
    {
        network.Update();
    }

    private void OnGUI()
    {
        if(GUI.Button(new Rect(100, 200, 300, 100), "Join"))
        {
            gameprotos.NewUser newUser = new gameprotos.NewUser();
            newUser.Content = "new user";
            network.SendSockMsg(newUser);
        }

        if (GUI.Button(new Rect(100, 400, 300, 100), "message"))
        {
            gameprotos.UserMessage userMessage = new gameprotos.UserMessage();
            userMessage.Content = "hello";
            network.SendSockMsg(userMessage);
        }
    }

    void ShowNewMessage(SockMsgRouter router, IProtocolHead ph)
    {
        Debug.Log("get message: " + (ph as gameprotos.UserMessage).Content);
    }

}
