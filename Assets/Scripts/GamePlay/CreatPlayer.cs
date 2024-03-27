using PlayerInfo;
using System;
using System.Collections.Generic;
using UnityEngine;

public class CreatPlayer : MonoBehaviour
{
    List<OtherPlayer> otherPlayers = new List<OtherPlayer>();
    private void Awake()
    {
        //检测在此之前上线的玩家
        MessageCenter<byte[]>.Ins.AddListener(MessageId.SC_GET_BEFORE_ONLINE_PLAYERCALL, GetBeforeOnlinePlayer);
        MessageCenter<byte[]>.Ins.AddListener(MessageId.SC_GET_ONLINE_PLAYERCALL, GetOnlinePlayer);
        MessageCenter<byte[]>.Ins.AddListener(MessageId.SC_CLOSE_APP_RECALL, OhterPlayerDown);
        MessageCenter<byte[]>.Ins.AddListener(MessageId.SC_PLAYER_MOVE_CALL, OhterPlayerMove);
        MessageCenter<byte[]>.Ins.AddListener(MessageId.SC_SHOW_PLAYER_HP_CALL, OnSetHp);

    }
    private void OnSetHp(byte[] obj)
    {
        PlayerData palyer = PlayerData.Parser.ParseFrom(obj);

        for (int i = otherPlayers.Count - 1; i >= 0; i--)
        {
            if (otherPlayers[i].otherPlayer.UserId == palyer.UserId)
            {
                otherPlayers[i].otherPlayer = palyer;
                otherPlayers[i].SetHp();
            }
        }

    }
    private void OhterPlayerMove(byte[] obj)
    {
        PlayerData playerData = PlayerData.Parser.ParseFrom(obj);
        for (int i = otherPlayers.Count - 1; i >= 0; i--)
        {
            if (otherPlayers[i].otherPlayer.UserId == playerData.UserId)
            {
                otherPlayers[i].SetAniState(playerData.AniState);
                otherPlayers[i].SetPlayerState(playerData);
            }
        }
    }

    private void OhterPlayerDown(byte[] obj)
    {
        PlayerData playerData = PlayerData.Parser.ParseFrom(obj);
        for (int i = otherPlayers.Count - 1; i >= 0; i--)
        {
            if (otherPlayers[i].otherPlayer.UserId == playerData.UserId)
            {
                Destroy(otherPlayers[i].hpSli);
                Destroy(otherPlayers[i].gameObject);
                otherPlayers.RemoveAt(i);
            }
        }
    }

    //上线之前 上线的人 
    private void GetBeforeOnlinePlayer(byte[] obj)
    {
        OnlinePlayer onlinePlayer = OnlinePlayer.Parser.ParseFrom(obj);
        for (int i = 0; i < onlinePlayer.AllPlyer.Count; i++)
        {
            GameObject player = Instantiate(Resources.Load<GameObject>("Role/" + onlinePlayer.AllPlyer[i].Path));
            player.transform.position = new Vector3(onlinePlayer.AllPlyer[i].Posx, 0, 0);

            player.AddComponent<OtherPlayer>();    //不进行赋值
            player.GetComponent<OtherPlayer>().otherPlayer = onlinePlayer.AllPlyer[i];
            otherPlayers.Add(player.GetComponent<OtherPlayer>());
        }
    }

    //单个人上线
    private void GetOnlinePlayer(byte[] obj)
    {
        PlayerData playerData = PlayerData.Parser.ParseFrom(obj);
        GameObject player = Instantiate(Resources.Load<GameObject>("Role/" + playerData.Path));
        player.transform.position = new Vector3(playerData.Posx, 0, 0);

        player.AddComponent<OtherPlayer>();    //不进行赋值
        player.GetComponent<OtherPlayer>().otherPlayer = playerData;
        otherPlayers.Add(player.GetComponent<OtherPlayer>());
    }

    private void Start()
    {
        GameObject player = Instantiate(Resources.Load<GameObject>("Role/" + PlayerModel.Ins.myPlayerData.Path));
        player.transform.position = new Vector3(PlayerModel.Ins.myPlayerData.Posx, 0, 0);
        player.AddComponent<PlayerMove>();
        player.AddComponent<HPSlider>();
        player.tag = "Player";
        //请求获取已经上线的信息
        NetMgr.Ins.AsySend(MessageId.CS_GET_BEFORE_ONLINE_PLAYER, new byte[0]);
    }
}
