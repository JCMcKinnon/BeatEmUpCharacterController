using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class AnimationEventHelper : MonoBehaviour
{
    private PlayerState ps;
    void Awake()
    {
        ps = GetComponentInParent<PlayerState>();
    }
    public void SetDash()
    {
        ps.currentState = PlayerState.State.dashAttack;
    }
    public void SetWalk()
    {
        ps.currentState = PlayerState.State.moving;
    }
    public void SetIdle()
    {
        ps.currentState = PlayerState.State.idle;
    }
    public void SetAttack3()
    {
        ps.currentState = PlayerState.State.attack3;
    }
    public void SetAttack2()
    {
        ps.currentState = PlayerState.State.attack2;
    }
    public void SetAttack1()
    {
        ps.currentState = PlayerState.State.attack1;
    }
    public void IsAcceptingInput()
    {
        ps.AcceptInput();
    }
    public void IsNotAcceptingInput() { 
        ps.DoNotAcceptInput();
    }
    public void AttackFinished()
    {
        ps.AttackFinished();
    }
}
