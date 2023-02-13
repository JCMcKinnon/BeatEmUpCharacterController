using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFSM : MonoBehaviour
{
    //references--------------
    private GameObject player;
    private PlayerState playerState;
    private Camera cam;
    private SpriteRenderer sr;
    //------------------------
    public float speed;
    public EnemyState state;

    private Vector3 patrolPointOne;
    private Vector3 patrolPointTwo;

    [SerializeField] private float maxDistanceToPlayer;
    private void Awake()
    {
        player = GameObject.FindWithTag("Player");
        playerState = player.GetComponent<PlayerState>();
        cam = Camera.main;
        sr = GetComponentInChildren<SpriteRenderer>();
    }
    public enum EnemyState
    {
        persuing,
        attacking,
        selectPatrolRoute,
        walkingToPoint2,
        walkingToPoint1,
        idle,
        inPain
    } 
    void Start()
    {
        state = EnemyState.selectPatrolRoute;
    }
    void Update()
    {
        if (Vector3.Distance(player.transform.position, transform.position) < maxDistanceToPlayer)
        {
            state = EnemyState.persuing;
        }
        switch (state)
        {
            case EnemyState.idle:
                break;
            case EnemyState.attacking:
                break;
            case EnemyState.selectPatrolRoute:
                SelectPatrolPoints();              
                break;
            case EnemyState.walkingToPoint2:
                transform.position = Vector3.MoveTowards(transform.position, patrolPointTwo, speed * Time.deltaTime);
                if(transform.position.x > patrolPointTwo.x)
                {
                    sr.flipX= true;
                }
                else
                {
                    sr.flipX= false;
                }
                if (transform.position == patrolPointTwo)
                {
                    state = EnemyState.walkingToPoint1;
                }
                break;
            case EnemyState.walkingToPoint1:
                transform.position = Vector3.MoveTowards(transform.position, patrolPointOne, speed * Time.deltaTime);
                if (transform.position.x > patrolPointOne.x)
                {
                    sr.flipX = true;
                }
                else
                {
                    sr.flipX = false;
                }
                if (transform.position == patrolPointOne)
                    state = EnemyState.walkingToPoint2;
                break;
            case EnemyState.inPain: 
                break;
            case EnemyState.persuing:
                FollowPlayer();
                break;
               

        }
    }

    private void FollowPlayer()
    {
        var directionToPlayer = player.transform.position - transform.position;
        if(Vector2.Distance(player.transform.position, transform.position) > 0.5f)
        {
            transform.Translate(directionToPlayer * Time.deltaTime * speed, Space.Self);
            if(directionToPlayer.x < 0f)
            {
                sr.flipX = true;
            }
            else
            {
                sr.flipX= false;
            }
        }
    }
    private void SelectPatrolPoints()
    {       
        patrolPointOne = transform.position;
        patrolPointTwo = transform.position + new Vector3(Random.Range(-10,10),0,0);
        if(cam.WorldToViewportPoint(patrolPointTwo).x > 0 && cam.WorldToViewportPoint(patrolPointTwo).x < 1)
        {
            print("success " + patrolPointTwo);
            state = EnemyState.walkingToPoint2;
            return;
        }
        else
        {
            print("failed " + patrolPointTwo);
            return;
        }
    }   
    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.tag == "PlayerSword")
        {
            if (playerState.currentlyAttacking)
            {
                print("hit");
            }
        }
    }
}
