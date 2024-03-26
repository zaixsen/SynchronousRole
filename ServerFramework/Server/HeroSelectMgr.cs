using PlayerInfo;
using Google.Protobuf;
using System;

namespace ServerFramework.Server
{
    class HeroSelectMgr : Singleton<HeroSelectMgr>
    {
        public void Init()
        {
            MessageCenter<MsgData>.Ins.AddListener(MessageId.CS_PlayerInfo, OnGetPlayerInfo);
            MessageCenter<MsgData>.Ins.AddListener(MessageId.CS_GET_BEFORE_ONLINE_PLAYER, OnGetBeforeOnline);
            MessageCenter<MsgData>.Ins.AddListener(MessageId.CS_CLOSE_APP, OnCloseApp);
            MessageCenter<MsgData>.Ins.AddListener(MessageId.CS_PLAYER_MOVE, OnPlayerMove);
        }

        private void OnPlayerMove(MsgData obj)
        {
            PlayerData playerData = PlayerData.Parser.ParseFrom(obj.data);
            obj.client.playerData.Posx = playerData.Posx;
            obj.client.playerData.Posz = playerData.Posz;
            obj.client.playerData.Rosy = playerData.Rosy;
            obj.client.playerData.AniState = playerData.AniState;

            //发送在线的人
            for (int i = 0; i < NetMgr.Ins.clients.Count; i++)
            {
                if (NetMgr.Ins.clients[i].playerData.UserId != playerData.UserId)
                {
                    NetMgr.Ins.AsySend(NetMgr.Ins.clients[i], MessageId.SC_PLAYER_MOVE_CALL, playerData.ToByteArray());
                }
            }
        }

        //发送消息下线 其他玩家不显示此玩家
        private void OnCloseApp(MsgData obj)
        {
            //下线
            NetMgr.Ins.clients.Remove(obj.client);
            //给每个人发送删除信息
            for (int i = 0; i < NetMgr.Ins.clients.Count; i++)
            {
                NetMgr.Ins.AsySend(NetMgr.Ins.clients[i], MessageId.SC_CLOSE_APP_RECALL, obj.client.playerData.ToByteArray());
            }
        }

        //谁请求发给谁
        private void OnGetBeforeOnline(MsgData obj)
        {
            OnlinePlayer onlinePlayer = new OnlinePlayer();

            //发送之前上线的玩家
            for (int i = 0; i < NetMgr.Ins.clients.Count; i++)
            {
                if (NetMgr.Ins.clients[i].playerData.UserId != obj.client.playerData.UserId)
                {
                    onlinePlayer.AllPlyer.Add(NetMgr.Ins.clients[i].playerData);
                }
            }

            NetMgr.Ins.AsySend(obj.client, MessageId.SC_GET_BEFORE_ONLINE_PLAYERCALL, onlinePlayer.ToByteArray());
        }

        int indexPos = -5;
        int Uid = 100001;

        private void OnGetPlayerInfo(MsgData obj)
        {
            PlayerData playerData = PlayerData.Parser.ParseFrom(obj.data);

            obj.client.playerData = playerData;
            playerData.Posx = indexPos;
            playerData.UserId = Uid++;
            indexPos += 5;
            NetMgr.Ins.clients.Add(obj.client);

            NetMgr.Ins.AsySend(obj.client, MessageId.SC_PlayerInfo, playerData.ToByteArray());

            //发自己的坐标给已经上线的人
            for (int i = 0; i < NetMgr.Ins.clients.Count; i++)
            {
                if (NetMgr.Ins.clients[i].playerData.UserId != playerData.UserId)
                {
                    NetMgr.Ins.AsySend(NetMgr.Ins.clients[i], MessageId.SC_GET_ONLINE_PLAYERCALL, playerData.ToByteArray());
                }
            }
        }

    }
}