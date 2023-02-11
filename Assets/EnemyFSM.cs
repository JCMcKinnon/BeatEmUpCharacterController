using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFSM : MonoBehaviour
{
    //references--------------
    private GameObject player;
    //------------------------
    public float speed;
    
    private void Awake()
    {
        player = GameObject.FindWithTag("Player");
    }
    public enum EnemyState
    {
        persuing,
        attacking,
        patrolling,
        idle,
        inPain
    } 
    void Update()
    {
        FollowPlayer();
    }

    private void FollowPlayer()
    {
        var directionToPlayer = player.transform.position - transform.position;

        print(Vector2.Distance(player.transform.position, transform.position));
        if(Vector2.Distance(player.transform.position, transform.position) > 0.5f)
        {
            transform.Translate(directionToPlayer * Time.deltaTime * speed, Space.Self);
        }
    }
}
