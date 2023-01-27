using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerState : MonoBehaviour
{
    PlayerMovement playerMovement;
    public State currentState;
    private float idleTimer;
    private bool countDownIdleTimer = false;
    public enum State { 
        idle,
        moving,
        dashAttack
    }
    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        currentState = new State();
        idleTimer = 1;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            idleTimer = -.55f;
            currentState = State.dashAttack;
        }

        if (playerMovement.moving)
        {
            currentState = State.moving;
        }
        if(!playerMovement.moving && idleTimer > 0)
        {
            currentState = State.idle;
        }

        print(currentState);

        idleTimer += Time.deltaTime;
        
    }
}
