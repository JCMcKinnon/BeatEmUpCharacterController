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
        ps.SetDashAttack();
    }
    public void SetWalk()
    {
        ps.SetMoving();
    }
    public void SetIdle()
    {
        ps.SetIdle();
    }
    public void SetAttack3()
    {
        ps.SetAttack3();
    }
    public void SetAttack2()
    {
        ps.SetAttack2();
    }
    public void SetAttack1()
    {
        ps.SetAttack1();
    }
}
