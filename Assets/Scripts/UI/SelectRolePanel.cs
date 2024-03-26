using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Google.Protobuf;
using PlayerInfo;
using UnityEngine.SceneManagement;

public class SelectRolePanel : MonoBehaviour
{
    public List<Toggle> toggles = new List<Toggle>();
    public Button btn_enterGame;
    public InputField ipt_username;
    int rolePath = 1;

    private void Awake()
    {
        MessageCenter<byte[]>.Ins.AddListener(MessageId.SC_PlayerInfo, GetPlayerInfo);
    }

    private void GetPlayerInfo(byte[] obj)
    {
        PlayerModel.Ins.SetPlayerData(PlayerData.Parser.ParseFrom(obj));

        SceneManager.LoadScene("Game");
    }

    private void Start()
    {
        for (int i = 0; i < toggles.Count; i++)
        {
            int index = i;
            toggles[i].onValueChanged.AddListener((flag) =>
            {
                if (flag)
                {
                    rolePath = index + 1;
                }
            });
        }

        btn_enterGame.onClick.AddListener(() =>
        {
            //通过protocol 传递数据
            PlayerData playerData = new PlayerData();
            playerData.Username = ipt_username.text;
            playerData.Path = rolePath.ToString();

            //发送服务器
            NetMgr.Ins.AsySend(MessageId.CS_PlayerInfo, playerData.ToByteArray());


        });
    }
}
