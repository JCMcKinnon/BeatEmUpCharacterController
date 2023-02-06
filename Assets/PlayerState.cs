using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class PlayerState : MonoBehaviour
{
    //Dependencies--------
    PlayerMovement playerMovement;
    //--------------------
    
    //State---------------
    public State currentState;
    public State requestState;
    //--------------------
   
    //private fields------
    private bool isAcceptingInput;
    private bool currentlyAttacking;
    private bool confirmNextAttack;
    //--------------------

    //input---------------
    private PlayerControls controls;
    private InputAction regularAttack;
    private InputAction move;
    private InputAction ranged;
    //--------------------

    //buffer--------------
    [SerializeField] public Queue<State> inputQueue;
    int inputbuffer;
    private bool requestingStateChange;
    float queueTimer;
    //--------------------


    public enum State { 
        idle,
        moving, 
        dashAttack,
        rangedAttack,
        attack1,
        attack2,
        attack3
    }
    private void Awake()
    {
        controls = new PlayerControls();
        playerMovement = GetComponent<PlayerMovement>();
        currentState = new State();
        requestState = new State();
        requestState = State.idle;
        inputQueue = new Queue<State>();
    }
    private void OnEnable()
    {
        regularAttack = controls.Player.regularAttack;
        regularAttack.Enable();

        move = controls.Player.Move;
        move.Enable();

        ranged = controls.Player.Fire;
        ranged.Enable();

        move.performed += SetMoving;
        regularAttack.performed += BasicAttack;
        ranged.started += SetRangedAttack;
    }
    private void OnDisable()
    {
        regularAttack.Disable();
        move.Disable();
    }

    void Update()
    {
        if(inputQueue.Count > 4)
        {
            inputQueue.Clear();
        }
        switch (currentState)
        {
            case State.idle:
                isAcceptingInput = true;
                if (move.IsInProgress())
                {
                    currentState = State.moving;
                }
                VerifyStateRequest();
                break;
            case State.moving:
                isAcceptingInput = true;
                if(!move.IsInProgress())
                {
                    currentState = State.idle;
                }
                VerifyStateRequest();

                break;
            case State.dashAttack:
                if (move.IsInProgress() && currentlyAttacking != true)
                {
                    currentState = State.moving;
                }
                if (!currentlyAttacking && isAcceptingInput == false)
                {
                    currentState = State.idle;
                }

               
                    VerifyStateRequest();
                
                break;
            case State.rangedAttack:
                if (move.IsInProgress() && currentlyAttacking != true)
                {
                    currentState = State.moving;
                }
                if (!currentlyAttacking && isAcceptingInput == false)
                {
                    currentState = State.idle;
                }
                if (inputQueue.Count == 0 && !currentlyAttacking)
                {
                    inputbuffer = 0;
                }

                VerifyStateRequest();
                break;
            case State.attack1:
                if (move.IsInProgress() && currentlyAttacking != true)
                {
                    currentState = State.moving;
                }
                if (!currentlyAttacking && isAcceptingInput == false)
                {
                    currentState = State.idle;
                }
                if (inputQueue.Count == 0 && !currentlyAttacking)
                {
                    inputbuffer = 0;
                }

                VerifyStateRequest();
                
                break;
            case State.attack2:
                if (move.IsInProgress() && currentlyAttacking != true)
                {
                    currentState = State.moving;
                }
                if (!currentlyAttacking && isAcceptingInput == false)
                {
                    currentState = State.idle;
                }
                if (inputQueue.Count == 0 && !currentlyAttacking)
                {
                    inputbuffer = 0;
                }

                VerifyStateRequest();
                
                break;
            case State.attack3:
                if (move.IsInProgress() && currentlyAttacking != true)
                {
                    currentState = State.moving;
                }
                if (!currentlyAttacking)
                {
                    currentState = State.idle;
                }
                if (inputQueue.Count == 0 && !currentlyAttacking)
                {
                    inputbuffer = 0;
                }
                VerifyStateRequest();
                

                break;
            default:
                break;
        }

    }


    public void SetIdle()
    {
        currentState = State.idle;
    }
    public void SetMoving(InputAction.CallbackContext context)
    {
        if (!currentlyAttacking)
        {
            currentState = State.moving;
        }
    }
    public void SetDashAttack(InputAction.CallbackContext context)
    {
        if (isAcceptingInput)
            inputQueue.Enqueue(State.dashAttack);

        /*        currentlyAttacking = true;
                requestState = State.dashAttack;*/
    }
    public void SetRangedAttack(InputAction.CallbackContext context)
    {

        if (isAcceptingInput)
            
            inputQueue.Enqueue(State.rangedAttack);

        /* currentlyAttacking = true;
        requestState = State.rangedAttack;*/
    }
    public void SetAttack1(InputAction.CallbackContext context)
    {
        if (isAcceptingInput)
        inputQueue.Enqueue(State.attack1);
    }
    public void BasicAttack(InputAction.CallbackContext context)
    {

        inputbuffer++;
        print(inputbuffer);
        if (isAcceptingInput)
        {
            if (inputbuffer==1)
            {

                    inputQueue.Enqueue(State.attack1);
                    return;
                
            }
        
            if(inputbuffer==2)
            {
                    inputQueue.Enqueue(State.attack2);
                    return;

                
            }

            if(inputbuffer==3)
            {                
                    inputQueue.Enqueue(State.attack3);
                    print(inputbuffer);

                return;
                
            }

        }


    }
    public void SetAttack2(InputAction.CallbackContext context)
    {

        if (isAcceptingInput)
            inputQueue.Enqueue(State.attack2);
        /* currentlyAttacking = true;       
        requestState = State.attack2;*/

    }
    public void SetAttack3(InputAction.CallbackContext context)
    {
        if (isAcceptingInput)
            inputQueue.Enqueue(State.attack3);
            inputbuffer = 0;
        /*currentlyAttacking = true;
        requestState = State.attack3;*/
    }
    public void VerifyStateRequest()
    {
        if (!currentlyAttacking) //if called during idle, walking or at the very end of an animation
        {
            if (inputQueue.Count != 0)
            {
                currentlyAttacking = true; //preemptive currentlyattacking flag
                currentState = inputQueue.Dequeue();
            }
        }
        
    }

    //Called from animation events----------------
    public void AcceptInput()
    {
        //as long as animation is playing, accept input
        isAcceptingInput= true;
        currentlyAttacking = true;
    }
    public void DoNotAcceptInput()
    {
        //at the 3/4 point of an animation, stop accepting input for next attack
        isAcceptingInput= false;
    }
    public void AttackFinished()
    {
        //end of the attack happened, used for initiating the next attack in a sequence.
        currentlyAttacking = false;
    }
    //--------------------------------------------
}
