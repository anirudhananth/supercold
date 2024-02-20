using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.XR;

public class Enemy : MonoBehaviour
{
    public enum State
    {
        Patrolling,
        Chasing,
        Attacking,
    }
    public State currentState;
    public Transform pointA, pointB;
    public LayerMask playerLayer;
    public GameObject bulletPrefab;
    public Transform shootingPoint;

    public float speed = 2f;
    public float shootingRange = 10f;
    public float shootingCooldown = 2f;
    
    private Rigidbody2D rb;
    private float shootingTimer;
    private bool movingToA = true;
    GameObject player;
    

    // Start is called before the first frame update
    void Start()
    {
        currentState = State.Patrolling;
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if(player.GetComponent<Movement>().gameOver) return;
        switch(currentState)
        {
            case State.Patrolling:
                Patro();
                if(CheckForPlayer())
                {
                    changeState(State.Attacking);
                }
                break;
            case State.Chasing:
                break;
            case State.Attacking:
                Attack();
                if(!CheckForPlayer())
                {
                    changeState(State.Patrolling);
                }
                break;
        }
    }

    void Patro()
    {
        Transform target = movingToA ? pointA : pointB;
        Vector2 newPosition = Vector2.MoveTowards(rb.position, target.position, speed * Time.fixedDeltaTime);

        rb.MovePosition(newPosition);

        if (Vector2.Distance(rb.position, target.position) < 0.1f)
        {
            movingToA = !movingToA;
        }
    }

    bool CheckForPlayer()
    {
        
        if (player == null) return false;

        Vector2 directionToPlayer = player.transform.position - transform.position;
        Vector2 forward = movingToA ? (pointA.position - pointB.position).normalized : (pointB.position - pointA.position).normalized;
        float angleToPlayer = Vector2.Angle(forward, directionToPlayer);
        float distanceToPlayer = directionToPlayer.magnitude;

        //Check if facing the player and within range
        bool playerInShootingZone = angleToPlayer <= 90 && distanceToPlayer <= shootingRange;

        // use raycast to see if can hit the player
        RaycastHit2D hit = Physics2D.Raycast(transform.position, directionToPlayer.normalized, shootingRange, playerLayer);
        bool lineOfSightToPlayer = hit.collider != null && hit.collider.gameObject == player;

        if (playerInShootingZone && lineOfSightToPlayer)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    void Attack()
    {
        if (shootingTimer <= 0)
        {
            Instantiate(bulletPrefab, shootingPoint.position, Quaternion.identity);
            shootingTimer = shootingCooldown;
        }
        else
        {
            shootingTimer -= Time.deltaTime;
        }
    }

    public void changeState(State newState)
    {
        currentState = newState;
        Debug.Log(currentState);
        switch(newState)
        {
            case State.Patrolling:
                break;
            case State.Chasing:
                break;
            case State.Attacking:
                break;
        }
    }

    void OnCollisionEnter2D(Collision2D col) {
        if(col.gameObject.tag == "Fireball") {
            Debug.Log("Hit by fireball");
            Destroy(gameObject);
        }
    }
}
