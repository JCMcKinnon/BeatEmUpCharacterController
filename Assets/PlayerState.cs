using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class PlayerState : MonoBehaviour
{   
    //State---------------
    public State currentState;
    //--------------------
   
    //private fields------
    private bool isAcceptingInput;
    private bool currentlyAttacking;
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
        currentState = new State();
        inputQueue = new Queue<State>();
    }
    private void OnEnable()
    {
        //initialize controls------------
        regularAttack = controls.Player.regularAttack;
        regularAttack.Enable();
        move = controls.Player.Move;
        move.Enable();
        ranged = controls.Player.Fire;
        ranged.Enable();
        //-------------------------------       
        
        //subscribe events---------------
        move.performed += SetMoving;
        regularAttack.performed += BasicAttack;
        ranged.started += SetRangedAttack;
        //-------------------------------
    }
    private void OnDisable()
    {
        regularAttack.Disable();
        move.Disable();
        ranged.Disable();
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
                UpdateInputDuringAttack();
                VerifyStateRequest();               
                break;
            case State.rangedAttack:
                UpdateInputDuringAttack();
                VerifyStateRequest();
                break;
            case State.attack1:
                UpdateInputDuringAttack();
                VerifyStateRequest();               
                break;
            case State.attack2:
                UpdateInputDuringAttack();
                VerifyStateRequest();              
                break;
            case State.attack3:
                UpdateInputDuringAttack();
                VerifyStateRequest();                
                break;
            default:
                break;
        }
    }
    private void UpdateInputDuringAttack()
    {
        if (move.IsInProgress() && !currentlyAttacking)
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
    }
    public void SetRangedAttack(InputAction.CallbackContext context)
    {
        if (isAcceptingInput)           
            inputQueue.Enqueue(State.rangedAttack);
    }
    public void BasicAttack(InputAction.CallbackContext context)
    {
        //add an attack to the queue. 
        //And for each subsequent input, 
        //increment the attack types for a chain attack.
        //eg: attack 1, attack 2, attack 3.
        inputbuffer++;
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
