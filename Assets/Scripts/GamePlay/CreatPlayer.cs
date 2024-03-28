using PlayerInfo;
using System;
using System.Collections.Generic;
using UnityEngine;

public class CreatPlayer : MonoBehaviour
{
    public static CreatPlayer Ins;
    //记录其他玩家
    List<OtherPlayer> otherPlayers = new List<OtherPlayer>();
    public GameObject player;

    private void Awake()
    {
        Ins = this;
        //检测在此之前上线的玩家
        MessageCenter<byte[]>.Ins.AddListener(MessageId.SC_GET_BEFORE_ONLINE_PLAYERCALL, GetBeforeOnlinePlayer);
        MessageCenter<byte[]>.Ins.AddListener(MessageId.SC_GET_ONLINE_PLAYERCALL, GetOnlinePlayer);
        MessageCenter<byte[]>.Ins.AddListener(MessageId.SC_CLOSE_APP_RECALL, OhterPlayerDown);
        MessageCenter<byte[]>.Ins.AddListener(MessageId.SC_HIT_PLYER_CALL, OnSetSelfHp);
        MessageCenter<byte[]>.Ins.AddListener(MessageId.SC_PLAYER_UPDATE_CALL, OnOtherPlayerDataUpdate);
    }

    private void OnOtherPlayerDataUpdate(byte[] obj)
    {
        PlayerData palyer = PlayerData.Parser.ParseFrom(obj);

        for (int i = otherPlayers.Count - 1; i >= 0; i--)
        {
            if (otherPlayers[i].otherPlayer.UserId == palyer.UserId)
            {
                otherPlayers[i].SetPlayerState(palyer);
                otherPlayers[i].SetHp();
            }
        }
    }

    /// <summary>
    /// 别人伤害的玩家
    /// </summary>
    /// <param name="obj"></param>
    private void OnSetSelfHp(byte[] obj)
    {
        PlayerData hitPlayer = PlayerData.Parser.ParseFrom(obj);

        PlayerModel.Ins.myPlayerData = hitPlayer;
        player.GetComponent<HPSlider>().SetHp();
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

    public OtherPlayer GetMinDisPlayer()
    {
        if (otherPlayers.Count == 0) return null;

        OtherPlayer minPlayer = otherPlayers[0];
        float minDis = Vector3.Distance(player.transform.position, minPlayer.transform.position);

        for (int i = 1; i < otherPlayers.Count; i++)
        {
            float Dis = Vector3.Distance(player.transform.position, otherPlayers[i].transform.position);
            if (minDis > Dis)
            {
                minDis = Dis;
                minPlayer = otherPlayers[i];
            }
        }
        return minPlayer;
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
        player = Instantiate(Resources.Load<GameObject>("Role/" + PlayerModel.Ins.myPlayerData.Path));
        player.transform.position = new Vector3(PlayerModel.Ins.myPlayerData.Posx, 0, 0);
        player.AddComponent<PlayerMove>();
        player.AddComponent<HPSlider>();
        player.tag = "Player";

        //请求获取已经上线的信息
        NetMgr.Ins.AsySend(MessageId.CS_GET_BEFORE_ONLINE_PLAYER, new byte[0]);
    }
}
