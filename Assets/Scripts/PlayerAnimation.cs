using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    public Animator anim;
    public PlayerState playerState;

    void Start()
    {
        
    }

    void Update()
    {
        if(playerState.currentState == PlayerState.State.idle)
        {
            anim.Play("idleSpaceGuy");
        }
        if(playerState.currentState == PlayerState.State.moving)
        {
            anim.Play("improvedRun");
        }
        if (playerState.currentState == PlayerState.State.dashAttack)
        {
            anim.Play("dashAttack");
        }
        if (playerState.currentState == PlayerState.State.rangedAttack)
        {
            anim.Play("Attack4");
        }
        if(playerState.currentState == PlayerState.State.attack1)
        {
            anim.Play("Attack1");
        }
        if (playerState.currentState == PlayerState.State.attack2)
        {
            anim.Play("Attack2");
        }
        if (playerState.currentState == PlayerState.State.attack3)
        {
            anim.Play("Attack3");
        }

    }
}
