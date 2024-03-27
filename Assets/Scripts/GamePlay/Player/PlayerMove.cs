using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Google.Protobuf;
public class PlayerMove : MonoBehaviour
{
    Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }
    bool isOne = false;
    private void Update()
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
            NetMgr.Ins.AsySend(MessageId.CS_PLAYER_MOVE, PlayerModel.Ins.myPlayerData.ToByteArray());
        }
        else if (pos == Vector3.zero && isOne)
        {
            PlayerModel.Ins.myPlayerData.AniState = PlayerInfo.AniState.Idle;
            animator.SetBool("Move", false);

            NetMgr.Ins.AsySend(MessageId.CS_PLAYER_MOVE, PlayerModel.Ins.myPlayerData.ToByteArray());
            isOne = false;
        }


    }
}
