using System.Text;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        NetMgr.Ins.Init();
    }

    private void Update()
    {
        NetMgr.Ins.NetUpData();
    }


    public void OnApplicationQuit()
    {
        NetMgr.Ins.AsySend(MessageId.CS_CLOSE_APP, new byte[0]);
    }
}
