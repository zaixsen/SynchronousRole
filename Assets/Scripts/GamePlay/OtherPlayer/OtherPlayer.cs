using PlayerInfo;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OtherPlayer : MonoBehaviour
{
    public PlayerData otherPlayer;
    Animator animator;
    public GameObject hpSli;
    Slider slider;
    Text Hptext;
    private void Awake()
    {
        MessageCenter<byte[]>.Ins.AddListener(MessageId.SC_SHOW_PLAYER_HP_CALL, OnSetHp);
    }

    private void Start()
    {
        animator = GetComponent<Animator>();
        hpSli = Instantiate(Resources.Load<GameObject>("Prefabs/HpSlider"), GameObject.Find("Hplay").transform);
        slider = hpSli.GetComponent<Slider>();

        slider.maxValue = otherPlayer.AllHp;
        slider.value = otherPlayer.NowHp;
        Hptext = hpSli.GetComponentInChildren<Text>();
        Hptext.text = otherPlayer.NowHp + "/" + otherPlayer.AllHp;
    }

    private void OnSetHp(byte[] obj)
    {
        otherPlayer = PlayerData.Parser.ParseFrom(obj);
        slider.value = otherPlayer.NowHp;
        Hptext.text = otherPlayer.NowHp + "/" + otherPlayer.AllHp;
    }

    public void SetPlayerState(PlayerData playerData)
    {
        Vector3 pos = new Vector3(playerData.Posx, 0, playerData.Posz);
        Vector3 rotate = new Vector3(0, playerData.Rosy, 0);
        transform.position = pos;
        transform.eulerAngles = rotate;
    }

    private void Update()
    {
        hpSli.transform.position = Camera.main.WorldToScreenPoint(transform.position) + Vector3.up * 60;

    }

    public void SetAniState(AniState aniState)
    {
        switch (aniState)
        {
            case AniState.Idle:
                animator.SetBool("Move", false);
                break;
            case AniState.Run:
                animator.SetBool("Move", true);
                break;
            case AniState.Attack:
                break;
            case AniState.Death:
                break;
            default:
                break;
        }
    }

    private void OnDestroy()
    {
        Destroy(hpSli);
    }
}
