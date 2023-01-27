using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private Vector2 dir;
    public bool moving;
    public PlayerState playerState;
    
    SpriteRenderer sr; 

    void Awake()
    {
        sr = GetComponentInChildren<SpriteRenderer>();    
    }

    void Update()
    {


        if(playerState.currentState != PlayerState.State.dashAttack)
        {
            dir.x = Input.GetAxis("Horizontal");
            dir.y = Input.GetAxis("Vertical");
            transform.Translate(dir * 5 * Time.deltaTime, Space.Self);

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
            transform.Translate(transform.right * flipX * Time.deltaTime * 2.5f);
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
