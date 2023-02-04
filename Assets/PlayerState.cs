using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerState : MonoBehaviour
{
    PlayerMovement playerMovement;
    public State currentState;
    private bool isAcceptingInput;
    private bool currentlyAttacking;
    private bool confirmNextAttack;

    private int inputbuffer = 0;
    private delegate void AttackDelegate();
    private AttackDelegate nextAttack;
    Dictionary<KeyCode, AttackDelegate> InputsByAttack = new Dictionary<KeyCode, AttackDelegate>();
    public enum Inputs { 
        space,
        left,
        right,
        up,
        down,
        shift
    }

    public enum State { 
        idle,
        moving, 
        dashAttack,
        rangedAttack,
        attack1,
        attack2,
        attack3
    }
    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        currentState = new State();
    }
    private void Awake()
    {
        InputsByAttack.Add(KeyCode.Space, SetDashAttack);
        InputsByAttack.Add(KeyCode.RightArrow, SetAttack1);
        InputsByAttack.Add(KeyCode.DownArrow, SetRangedAttack);
        nextAttack = new AttackDelegate(SetIdle);

    }

    void Update()
    {
        switch (currentState)
        {
            case State.idle:
                if (Input.GetKeyDown(KeyCode.Space))
                {
                   nextAttack = InputsByAttack[KeyCode.Space];
                   nextAttack.Invoke();

                }
                if (Input.GetKeyDown(KeyCode.RightArrow))
                {
                   nextAttack = InputsByAttack[KeyCode.RightArrow];
                   nextAttack.Invoke();

                }
                if (playerMovement.moving)
                {
                    SetMoving();
                }
                if (Input.GetKeyDown(KeyCode.DownArrow))
                {
                    SetRangedAttack();
                }
                break;
            case State.moving:
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    SetDashAttack();
                }
                if (Input.GetKeyDown(KeyCode.DownArrow))
                {
                    SetRangedAttack();
                }
                if (!playerMovement.moving)
                {
                    SetIdle();
                }
                if (Input.GetKeyDown(KeyCode.RightArrow))
                {
                    SetAttack1();
                }
                break;
            case State.dashAttack:
                if (isAcceptingInput)
                {

                }
                if (!currentlyAttacking)
                {
                    if (confirmNextAttack)
                    {
                        inputbuffer = 0;
                        confirmNextAttack= false;
                        SetAttack1();
                    }
                    else
                    {
                        SetIdle();
                    }
                }
                break;
            case State.attack1:
                
                if (isAcceptingInput && currentState == State.attack1)
                {
                    if (Input.GetKeyDown(KeyCode.RightArrow))
                    {
                        confirmNextAttack = true;
                        inputbuffer++;
                    }
                }
                if (confirmNextAttack)
                {
                    if (currentlyAttacking == false)
                    {
                        confirmNextAttack = false;
                        currentState = State.attack2;                    
                    }
                }
                else
                {
                    if (currentlyAttacking == false)
                    {
                        confirmNextAttack = false;
                        inputbuffer = 0;
                        currentState = State.idle;
                    }
                }
                break; 
            case State.attack2:
                if (isAcceptingInput)
                {
                    if (Input.GetKeyDown(KeyCode.RightArrow))
                    {
                        confirmNextAttack = true;
                        inputbuffer++;
                    }
                }
                if (confirmNextAttack)
                {
                    if (currentlyAttacking == false)
                    {

                        confirmNextAttack = false;

                       
                         currentState = State.attack3;
                        
                    }

                }
               
                    if (inputbuffer >= 2 && !currentlyAttacking)
                    {
                        currentState = State.attack3;
                    }
                
     
                else
                {
                    if (currentlyAttacking == false)
                    {
                        confirmNextAttack = false;
                        inputbuffer = 0;
                        currentState = State.idle;
                    }
                }
                break; 
            case State.attack3:
                if(currentlyAttacking== false)
                {
                    inputbuffer = 0;
                    currentState = State.idle;
                }
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
    public void SetRangedAttack()
    {
        currentState = State.rangedAttack;
    }
    public void SetAttack1()
    {
        currentState = State.attack1;
    }
    public void SetAttack2()
    {
        currentState = State.attack2;

    }
    public void SetAttack3()
    {
        currentState = State.attack3;
    }
    public void AcceptInput()
    {
        isAcceptingInput= true;
        currentlyAttacking = true;
    }
    public void DoNotAcceptInput()
    {
        isAcceptingInput= false;
    }
    public void AttackFinished()
    {
        currentlyAttacking= false;
        nextAttack.Invoke();

    }
    public void TakeInputForNextAttack(KeyCode input)
    {
            confirmNextAttack = true;
            nextAttack = InputsByAttack[input]; 
        //left off here, looking for Input.ReturnKeyCode
        //so that can just set nextAttack by user input.
    }
}
