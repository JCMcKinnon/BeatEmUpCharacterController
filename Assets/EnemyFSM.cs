using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFSM : MonoBehaviour
{
    public enum EnemyState
    {
        persuing,
        attacking,
        patrolling,
        idle,
        inPain
    } 
    void Update()
    {
        
    }
}
