
using PlayerInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
public class PlayerModel : Singleton<PlayerModel>
{
    public PlayerData myPlayerData;

    public void SetPlayerData(PlayerData playerData)
    {
        myPlayerData = playerData;
    }

}

