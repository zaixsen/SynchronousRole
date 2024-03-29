using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PlayerInfo;
using Google.Protobuf;
public class HPSlider : MonoBehaviour
{
    GameObject hpSli;
    Slider slider;
    Text Hptext;
    public int hight = 120;

    private void Start()
    {
        hpSli = Instantiate(Resources.Load<GameObject>("Prefabs/HpSlider"), GameObject.Find("Hplay").transform);
        slider = hpSli.GetComponent<Slider>();
        slider.maxValue = PlayerModel.Ins.myPlayerData.AllHp;
        slider.value = PlayerModel.Ins.myPlayerData.AllHp;
        Hptext = hpSli.GetComponentInChildren<Text>();
        Hptext.text = PlayerModel.Ins.myPlayerData.NowHp + "/" + PlayerModel.Ins.myPlayerData.AllHp;
    }

    public void SetHp()
    {
        slider.value = PlayerModel.Ins.myPlayerData.NowHp;
        Hptext.text = PlayerModel.Ins.myPlayerData.NowHp + "/" + PlayerModel.Ins.myPlayerData.AllHp;
    }

    public void SetHp(int hit)
    {
        PlayerModel.Ins.myPlayerData.NowHp -= hit;
        slider.value -= hit;
        Hptext.text = PlayerModel.Ins.myPlayerData.NowHp + "/" + PlayerModel.Ins.myPlayerData.AllHp;
        //发送自身信息
        NetMgr.Ins.AsySend(MessageId.CS_PLAYER_UPDATE, PlayerModel.Ins.myPlayerData.ToByteArray());
    }

    private void Update()
    {
        hpSli.transform.position = Camera.main.WorldToScreenPoint(transform.position) + Vector3.up * hight;
    }
}
