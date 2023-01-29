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

    void Awake()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
        controls = new PlayerControls();
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


        if(playerState.currentState != PlayerState.State.dashAttack && playerState.currentState != PlayerState.State.rangedAttack)
        {
            dir = move.ReadValue<Vector2>();
            transform.Translate(dir * 4.5f * Time.deltaTime, Space.Self);
        }
        if (dir != Vector2.zero)
        {
            moving = true;
            FlipSpriteBasedOnInput();
        }
        else
        {
            moving = false;
        }
        if(playerState.currentState == PlayerState.State.dashAttack)
        {
            int flipX = sr.flipX == true ? -1 : 1;
            transform.Translate(transform.right * flipX * Time.deltaTime * 6.5f);
        }
    }

    private void FlipSpriteBasedOnInput()
    {
        if (dir.x < 0)
        {
            var sr = GetComponentInChildren<SpriteRenderer>(); //TODO: put the getcomponent calls in awake
            sr.flipX = true;
        }
        if (dir.x > 0)
        {
            var sr = GetComponentInChildren<SpriteRenderer>();
            sr.flipX = false;
        }
    }
}
