﻿using PlayerInfo;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Google.Protobuf;
public class OtherPlayer : MonoBehaviour
{
    public PlayerData otherPlayer;
    Animator animator;
    public GameObject hpSli;
    Slider slider;
    Text Hptext;

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

    public void SetHp()
    {
        slider.value = otherPlayer.NowHp;
        Hptext.text = otherPlayer.NowHp + "/" + otherPlayer.AllHp;
    }

    public void SetHit(int hit)
    {
        slider.value -= hit;
        otherPlayer.NowHp -= hit;
        Hptext.text = otherPlayer.NowHp + "/" + otherPlayer.AllHp;

        NetMgr.Ins.AsySend(MessageId.CS_HIT_PLYER, otherPlayer.ToByteArray());
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
                animator.SetTrigger("Atk");
                break;
            case AniState.Death:
                break;
            default:
                break;
        }
    }

    void Atk()
    {

    }

    private void OnDestroy()
    {
        Destroy(hpSli);
    }
}
