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
    }
}
