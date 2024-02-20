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

    private bool isAlive = true;
    private bool isAttacking = false;

    private int health = 2;
    public float speed = 2f;
    public float shootingRange = 10f;
    public float shootingCooldown = 1.0f;
    private float angleToPlayer;
    
    private Rigidbody2D rb;
    private float shootingTimer;
    private bool movingToA = true;
    GameObject player;

    [SerializeField] Animator animator;
    

    // Start is called before the first frame update
    void Start()
    {
        shootingTimer = shootingCooldown;
        currentState = State.Patrolling;
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(!isAlive) return;

        if(player.GetComponent<Movement>().gameOver) return;
        switch(currentState)
        {
            case State.Patrolling:
                animator.SetBool("Attacking", false);
                animator.SetTrigger("Patrolling");
                shootingTimer = shootingCooldown;
                Patrol();
                if(CheckForPlayer())
                {
                    changeState(State.Attacking);
                }
                break;
            case State.Chasing:
                shootingTimer = shootingCooldown;
                break;
            case State.Attacking:
                isAttacking = true;
                StartCoroutine(AttackAnimationDelay());
                Attack();
                if(!CheckForPlayer() && !isAttacking && (shootingTimer == shootingCooldown) || angleToPlayer > 50f)
                {
                    changeState(State.Patrolling);
                }
                break;
        }
    }

    void Patrol()
    {
        Transform target = movingToA ? pointA : pointB;
        float x = movingToA ? -1f : 1f;
        transform.localScale = new(x, transform.localScale.y);

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
        angleToPlayer = Vector2.Angle(forward, directionToPlayer);
        
        float x = movingToA ? -1f : 1f;
        transform.localScale = new(x, transform.localScale.y);
        if(Mathf.Abs(angleToPlayer) > 50f) return false;
        
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
            // Coroutine coroutine = StartCoroutine(ShootBullet());
            Instantiate(bulletPrefab, shootingPoint.position, Quaternion.identity);
            shootingTimer = shootingCooldown;
        }
        else
        {
            shootingTimer -= Time.fixedDeltaTime * 1.35f;
        }
        isAttacking = false;
    }

    private IEnumerator AttackAnimationDelay() {
        yield return new WaitForSeconds(0.4f);
        animator.SetBool("Attacking", true);
    }

    public void changeState(State newState)
    {
        currentState = newState;
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
            health--;
            if(health == 0) {
                animator.SetTrigger("Death");
                isAlive = false;
            } else {
                animator.SetTrigger("Hit");
            }
        }
    }
}
