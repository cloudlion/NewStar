using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MVC;

public class Player : MonoBehaviour
{
    private int id;
    private string userName;
    bool localUser = true;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Init(int uid, string userName, bool local)
    {
        id = uid;
        this.userName = userName;
        gameObject.name = uid.ToString();
        localUser = local;
    }

    // Update is called once per frame
    void Update()
    {
        if (!localUser)
            return;
        Vector3 position = transform.position;
        ProxyMgr.Instance.GetProxy<RoomProxy>().Move(id, position);
    }

    public void SyncPosition(Vector3 pos)
    {
        transform.position = pos;
    }
}
