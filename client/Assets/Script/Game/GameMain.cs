using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        network.SockConnect("127.0.1",3250, 0, () => { Debug.Log("connected"); }, null);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnGUI()
    {
        if(GUI.Button(new Rect(100, 200, 300, 100), "Join"))
        {
            gameprotos.NewUser newUser = new gameprotos.NewUser();
            newUser.Content = "hello";
            network.SendSockMsg(newUser);
        }
    }
}
