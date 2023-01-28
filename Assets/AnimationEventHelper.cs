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
}
