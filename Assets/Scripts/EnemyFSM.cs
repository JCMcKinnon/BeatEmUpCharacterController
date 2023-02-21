using System.Collections;
using System.Collections.Generic;
using System.Resources;
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

    private Animator anim;
    private bool canDamage;
    public bool finishedAnim;
    [SerializeField] private float maxDistanceToPlayer;
    public int pain;
    private void Awake()
    {
        player = GameObject.FindWithTag("Player");
        playerState = player.GetComponent<PlayerState>();
        anim = GetComponentInChildren<Animator>();
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
        finishedAnim = false;
    }
    void Update()
    {
        if (Vector3.Distance(player.transform.position, transform.position) < maxDistanceToPlayer && state != EnemyState.inPain)
        {
            state = EnemyState.persuing;
        }
        switch (state)
        {
            case EnemyState.idle:
                finishedAnim= false;
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
                canDamage = false;
                anim.Play("enemyDamaged");
                if (finishedAnim)
                {
                    pain++;
                    state = EnemyState.persuing;
                }

                break;
            case EnemyState.persuing:
                finishedAnim = false;
                canDamage = true;
                anim.Play("enemy1Run");
                FollowPlayer();
                break;
               

        }
    }

    private void FollowPlayer()
    {
        var directionToPlayer = player.transform.position - transform.position;
        if(Vector2.Distance(player.transform.position, transform.position) > 0.7f)
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
            state = EnemyState.walkingToPoint2;
            return;
        }
        else
        {
            return;
        }
    }   

    void OnTriggerStay2D(Collider2D col)
    {

            if (col.tag == "PlayerSword")
            {
                if (playerState.canDealDamageToEnemy && !finishedAnim)
                {
                    if(canDamage)
                    state = EnemyState.inPain;
                }             
            }       
    }

}
