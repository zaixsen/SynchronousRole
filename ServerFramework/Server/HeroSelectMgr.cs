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
            MessageCenter<MsgData>.Ins.AddListener(MessageId.CS_HIT_PLYER, OnPlayerHit);

            //只用一个消息号更新玩家数据
            MessageCenter<MsgData>.Ins.AddListener(MessageId.CS_PLAYER_UPDATE, OnPlayerUpdate);
        }

        private void OnPlayerUpdate(MsgData obj)
        {
            //更新服务器玩家数据
            PlayerData playerData = PlayerData.Parser.ParseFrom(obj.data);

            obj.client.playerData = playerData;

            NetMgr.Ins.AsyAllSend(playerData.UserId, MessageId.SC_PLAYER_UPDATE_CALL, playerData.ToByteArray());
        }

        private void OnPlayerHit(MsgData obj)
        {
            //受击玩家的数据
            PlayerData playerData = PlayerData.Parser.ParseFrom(obj.data);
            obj.client.playerData.NowHp = playerData.NowHp;
            //找到对应的扣血玩家
            for (int i = 0; i < NetMgr.Ins.clients.Count; i++)
            {
                if (playerData.UserId == NetMgr.Ins.clients[i].playerData.UserId)
                {
                    NetMgr.Ins.AsySend(NetMgr.Ins.clients[i], MessageId.SC_HIT_PLYER_CALL, playerData.ToByteArray());
                }
            }
            //让所有人更新血条
            NetMgr.Ins.AsyAllSend(playerData.UserId, MessageId.SC_PLAYER_UPDATE_CALL, playerData.ToByteArray());
        }

        //发送消息下线 其他玩家不显示此玩家
        private void OnCloseApp(MsgData obj)
        {
            //下线
            NetMgr.Ins.clients.Remove(obj.client);
            //给每个人发送删除信息
            NetMgr.Ins.AsyAllSend(MessageId.SC_CLOSE_APP_RECALL, obj.client.playerData.ToByteArray());
        }

        //谁请求发给谁
        private void OnGetBeforeOnline(MsgData obj)
        {
            //玩家集合
            OnlinePlayer onlinePlayer = new OnlinePlayer();

            //发送之前上线的玩家
            for (int i = 0; i < NetMgr.Ins.clients.Count; i++)
            {
                if (NetMgr.Ins.clients[i].playerData.UserId != obj.client.playerData.UserId)
                {
                    onlinePlayer.AllPlyer.Add(NetMgr.Ins.clients[i].playerData);
                }
            }

            //NetMgr.Ins.AsySend(obj.client, MessageId.SC_GET_BEFORE_ONLINE_PLAYERCALL, onlinePlayer.ToByteArray());
        }

        //玩家数据 初始化
        int indexPos = -5;
        int Uid = 100001;
        int maxhp = 100;
        int hp = 100;
        private void OnGetPlayerInfo(MsgData obj)
        {
            PlayerData playerData = PlayerData.Parser.ParseFrom(obj.data);

            obj.client.playerData = playerData;
            playerData.Posx = indexPos;
            playerData.UserId = Uid++;

            playerData.AllHp = maxhp += 100;
            playerData.NowHp = hp += 100;

            indexPos += 5;
            NetMgr.Ins.clients.Add(obj.client);

            NetMgr.Ins.AsySend(obj.client, MessageId.SC_PlayerInfo, playerData.ToByteArray());

            NetMgr.Ins.AsyAllSend(playerData.UserId, MessageId.SC_GET_ONLINE_PLAYERCALL, playerData.ToByteArray());

            RoomMgr.Ins.SendClientExitRoom();
        }
    }
}