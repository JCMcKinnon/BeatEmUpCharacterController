using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1AnimationEventHelper : MonoBehaviour
{
    public EnemyFSM fsm;

    public void FinishedAnimation()
    {
        fsm.finishedAnim = true;
    }
}
