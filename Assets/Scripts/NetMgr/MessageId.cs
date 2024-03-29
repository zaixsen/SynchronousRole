using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

///消息id
public class MessageId
{
    public const int CS_LOGIN = 1001;        //上行协议
    public const int SC_LOGIN_CALL = 1002;   //下行协议
    //玩家信息
    public const int CS_PlayerInfo = 1003;
    public const int SC_PlayerInfo = 1004;

    //获取之前上线的玩家位置
    public const int CS_GET_BEFORE_ONLINE_PLAYER = 1005;
    public const int SC_GET_BEFORE_ONLINE_PLAYERCALL = 1006;
    //获取上线的玩家位置
    public const int CS_GET_ONLINE_PLAYER = 1007;
    public const int SC_GET_ONLINE_PLAYERCALL = 1008;

    //关闭
    public const int CS_CLOSE_APP = 1009;
    public const int SC_CLOSE_APP_RECALL = 1010;

    //玩家伤害
    public const int CS_HIT_PLYER = 1015;
    public const int SC_HIT_PLYER_CALL = 1016;

    //总体玩家更新
    public const int CS_PLAYER_UPDATE = 1019;
    public const int SC_PLAYER_UPDATE_CALL = 1020;

    //新建房间
    public const int CS_CREAT_ROOM = 1021;
    public const int SC_CREAT_ROOM_CALL = 1022;
    public const int SC_BEFORE_ROOM_CALL = 1023;
}

