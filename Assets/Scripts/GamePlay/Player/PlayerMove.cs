using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Google.Protobuf;
using System;

public class PlayerMove : MonoBehaviour
{
    Animator animator;
    bool isOne = false;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        Move();

        Attack();
    }

    private void Attack()
    {
        if (Input.GetMouseButtonDown(0))
        {

            PlayerModel.Ins.myPlayerData.AniState = PlayerInfo.AniState.Attack;
            NetMgr.Ins.AsySend(MessageId.CS_PLAYER_UPDATE, PlayerModel.Ins.myPlayerData.ToByteArray());

            animator.SetTrigger("Atk");
        }
    }

    public void Atk()
    {
        OtherPlayer otherPlayer = CreatPlayer.Ins.GetMinDisPlayer();
        if (otherPlayer == null) return;

        otherPlayer.SetHit(50);
    }

    private void Move()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        Vector3 pos = new Vector3(x, 0, y);

        if (pos != Vector3.zero)
        {
            animator.SetBool("Move", true);
            transform.LookAt(transform.position + pos);
            transform.Translate(Vector3.forward * 4 * Time.deltaTime);

            PlayerModel.Ins.myPlayerData.Posx = transform.position.x;
            PlayerModel.Ins.myPlayerData.Posz = transform.position.z;
            PlayerModel.Ins.myPlayerData.Rosy = transform.eulerAngles.y;
            PlayerModel.Ins.myPlayerData.AniState = PlayerInfo.AniState.Run;

            isOne = true;
            NetMgr.Ins.AsySend(MessageId.CS_PLAYER_UPDATE, PlayerModel.Ins.myPlayerData.ToByteArray());
        }
        else if (pos == Vector3.zero && isOne)
        {
            PlayerModel.Ins.myPlayerData.AniState = PlayerInfo.AniState.Idle;
            animator.SetBool("Move", false);

            NetMgr.Ins.AsySend(MessageId.CS_PLAYER_UPDATE, PlayerModel.Ins.myPlayerData.ToByteArray());
            isOne = false;
        }
    }
}
