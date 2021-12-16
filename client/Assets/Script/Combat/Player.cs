using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MVC;

public class Player : MonoBehaviour
{
    private int id;
    private string userName;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Init(int uid, string userName)
    {
        id = uid;
        this.userName = userName;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 position = transform.position;
        ProxyMgr.Instance.GetProxy<RoomProxy>().Move(id, position);
    }

    public void SyncPosition(Vector3 pos)
    {
        transform.position = pos;
    }
}
