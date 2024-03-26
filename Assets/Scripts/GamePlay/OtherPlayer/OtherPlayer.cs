using PlayerInfo;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OtherPlayer : MonoBehaviour
{
    public PlayerData otherPlayer;
    Animator animator;
    private void Start()
    {
        animator = GetComponent<Animator>();
    }
    public void SetPlayerState(PlayerData playerData)
    {
        Vector3 pos = new Vector3(playerData.Posx, 0, playerData.Posz);
        Vector3 rotate = new Vector3(0, playerData.Rosy, 0);
        transform.position = pos;
        transform.eulerAngles = rotate;
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
}
