using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class PlayerMovement : MonoBehaviour
{
    private Vector2 dir;
    public bool moving;
    public PlayerState playerState;
    private PlayerControls controls;
    private InputAction move;
    SpriteRenderer sr;
    public BoxCollider2D col;
    public bool hitEnemy;
    void Awake()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
        controls = new PlayerControls();
        hitEnemy = false;
    }
    void OnEnable()
    {
        move = controls.Player.Move;
        move.Enable();
    }
    void OnDisable()
    {
        move.Disable();
    }
    void Update()
    {
        //if not dash attack or ranged attack
        //take in input and move character
        if (playerState.currentState != PlayerState.State.dashAttack && playerState.currentState != PlayerState.State.rangedAttack && playerState.currentState != PlayerState.State.attack1 && playerState.currentState != PlayerState.State.attack2 && playerState.currentState != PlayerState.State.attack3)
        {
            dir = move.ReadValue<Vector2>();
            transform.Translate(dir * 4.5f * Time.deltaTime, Space.Self);
        }
        //if dash attacking or ranged attacking
        //slow down movement
        if (playerState.currentState == PlayerState.State.dashAttack 
            || playerState.currentState == PlayerState.State.rangedAttack 
            || playerState.currentState == PlayerState.State.attack1
            || playerState.currentState == PlayerState.State.attack2 
            || playerState.currentState == PlayerState.State.attack3)
        {
            dir = move.ReadValue<Vector2>();
            transform.Translate(dir * 0.3f * Time.deltaTime, Space.Self);
        }
        //if input is bigger than zero, move and flip sprite based on direction
        if (dir != Vector2.zero)
        {
            moving = true;

            if(playerState.currentState == PlayerState.State.moving || playerState.currentState == PlayerState.State.idle)
            {
                FlipSpriteBasedOnInput();
            }
        }
        else
        {
            moving = false;
        }
        //if dash attacking, make player dash in direction sprite is facing
        if(playerState.currentState == PlayerState.State.dashAttack)
        {
            int flipX = sr.flipX == true ? -1 : 1;
            if(!hitEnemy)
            {
                transform.Translate(transform.right * flipX * Time.deltaTime * 6.5f);
            }
            else
            {
                return;
            }
        }

        if (sr.flipX)
        {
            col.offset = new Vector2(-0.8f, 0.1f);

        }
        else
        {
            col.offset = new Vector2(0, 0.1f);
        }
    }

    private void FlipSpriteBasedOnInput()
    {
        if (dir.x < 0)
        {
            
            sr.flipX = true;
        }
        if (dir.x > 0)
        {
            
            sr.flipX = false;
        }
    }
    void OnTriggerStay2D(Collider2D col)
    {
        if(col.tag == "Enemy")
        {
            if (playerState.currentlyAttacking)
            {
                hitEnemy = true;
                print(hitEnemy);
                StartCoroutine(SetHitEnemyFlag());
            }
            
        }
    }
    public IEnumerator SetHitEnemyFlag()
    {

        yield return new WaitForSeconds(0.1f);
        hitEnemy = false;
        
        //playerState.canDealDamageToEnemy = false;
        //playerState.currentlyAttacking = false;
    }
}
