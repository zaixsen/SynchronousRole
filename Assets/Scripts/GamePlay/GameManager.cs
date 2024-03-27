using System.Text;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Ins;
    public Transform trs_Hplay;
    private void Awake()
    {
        Ins = this;
    }
    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        NetMgr.Ins.Init();
    }

    private void Update()
    {
        NetMgr.Ins.NetUpData();

        if (Input.GetKeyDown(KeyCode.Q))
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<HPSlider>().SetHp(10);
        }
    }


    public void OnApplicationQuit()
    {
        NetMgr.Ins.AsySend(MessageId.CS_CLOSE_APP, new byte[0]);
    }
}
