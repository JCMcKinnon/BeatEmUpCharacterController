using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerState : MonoBehaviour
{
    PlayerMovement playerMovement;
    public State currentState;
    public enum State { 
        idle,
        moving,
        dashAttack
    }
    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        currentState = new State();
    }

    void Update()
    {

        switch (currentState)
        {
            case State.idle:
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    SetDashAttack();
                }
                if (playerMovement.moving)
                {
                    SetMoving();
                }
                break;
            case State.moving:
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    SetDashAttack();
                }
                if (!playerMovement.moving)
                {
                    SetIdle();
                }
                break;
            case State.dashAttack:
                break;
            default:
                break;
        }





        
    }


    public void SetIdle()
    {
        currentState = State.idle;
    }
    public void SetMoving()
    {
        currentState = State.moving;
    }
    public void SetDashAttack()
    {
        currentState = State.dashAttack;
    }
}
