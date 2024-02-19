using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Movement : MonoBehaviour
{

    private float horizontal;
    private float vertical;
    private float speed = 8f;
    private float jumpingPower = 20f;
    private float flyingPower = 5f;
    private bool canFly = false;
    private bool isFlying = false;
    private float groundTimer = 0f;
    private float currentStamina = 150.0f;
    private float maxStamina = 150.0f;
    private float staminaUsageSpeed = 50f;
    private float staminaRegenSpeed = 75f;
    private Coroutine regenStaminaRoutine = null;
    
    private bool canDash = true;
    private bool isDashing;
    private float dashingPowerX = 35f;
    private float dashingPowerY = 20f;
    private float dashingTime = 0.2f;
    private float dashingCooldown = 1f;
    private int dashCount = 0;

    private int playerHealth = 3;

    private bool isFacingRight = true;

    private Collider2D isGrounded;
    Vector3 movementVector;
    private int health = 4;

    private Rigidbody2D rb => GetComponent<Rigidbody2D>();

    [SerializeField] private ParticleSystem jetpackParticles;
    [SerializeField] private TrailRenderer dashTrail;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private ConstantForce2D constantForce;
    [SerializeField] private Slider staminaBar;
    [SerializeField] private Canvas canvas;
    [SerializeField] private GameObject heartPrefab;

    private void Start()
    {
        UpdateHealthUI();   
        staminaBar.maxValue = maxStamina;
    }

    private void Update()
    {
        isGrounded = IsGrounded();
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
        Jump();
        Dash();
        staminaBar.value = currentStamina;

        if(isGrounded) isFlying = false;
    }

    private void FixedUpdate()
    {
        if(isDashing) return;
        // Vector3 movementVector = new Vector3(Input.GetAxisRaw("Horizontal"), 0, 0);
        // rb.MovePosition(rb.position + movementVector * speed * Time.fixedDeltaTime);
        rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
        if (horizontal > 0 && !isFacingRight)
        {
            Flip();
        }
        else if (horizontal < 0 && isFacingRight)
        {
            Flip();
        }

        if(isFlying && Input.GetKey(KeyCode.Space)) {
            currentStamina -= Time.fixedDeltaTime * staminaUsageSpeed;
            currentStamina = Mathf.Max(currentStamina, 0f);
        }

        if(!isFlying) groundTimer += Time.fixedDeltaTime;

        if(!isFlying && groundTimer > 1.0f) {
            currentStamina += Time.fixedDeltaTime * staminaRegenSpeed;
            currentStamina = Mathf.Min(currentStamina, maxStamina);
        }
    }

    private void Jump()
    {
        if(isDashing) return;

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded) {
            // var platform = isGrounded.gameObject.name;
            rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
            canFly = false;
        }

        if (canFly && Input.GetKey(KeyCode.Space) && !isGrounded) {
            isFlying = true;
            groundTimer = 0f;
            if (currentStamina > 0.0f) {
                rb.velocity = new Vector2(rb.velocity.x, flyingPower);
                if (!jetpackParticles.isPlaying) {
                    jetpackParticles.Play();
                }
            }
            else {
                if (jetpackParticles.isPlaying) {
                    jetpackParticles.Stop();
                }
            }
        }

        if (Input.GetKeyUp(KeyCode.Space)) {
            jetpackParticles.Stop();
            if (rb.velocity.y > 0f) {
                rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
            }
            if (rb.velocity.y != 0f) {
                canFly = true;
            }
        }
    }

    private void Dash() {
        if (canDash && isGrounded) {
            dashCount = 0;
        }
        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash) {
            StartCoroutine(PerformDash());
        }
    }

    private Collider2D IsGrounded() {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    private void Flip()
    {
        if(isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f) {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }

    private IEnumerator PerformDash() {
        if (dashCount == 1 && !isGrounded) yield break;

        Vector2 temp = new(constantForce.force.x, constantForce.force.y);
        constantForce.force = new Vector2(0,0);
        canDash = false;
        isDashing = true;

        bool isVertical = false;
        bool pureVertical = false;

        if(isGrounded) {
            rb.velocity = new Vector2(transform.localScale.x * dashingPowerX, 0f);
        } else {
            if(vertical == 0f) {
                rb.velocity = new Vector2(transform.localScale.x * dashingPowerX, 0f);
            } else if(horizontal == 0f) {
                rb.velocity = new Vector2(0f, vertical * dashingPowerY);
                pureVertical = true;
            } else {
                isVertical = true;
                rb.velocity = new Vector2(transform.localScale.x * dashingPowerX, vertical * dashingPowerY);
            }
            dashCount = 1;
        }

        dashTrail.emitting = true;
        yield return new WaitForSeconds(dashingTime);

        if(isVertical) {
            rb.velocity = new(horizontal * speed, rb.velocity.y * 0.35f);
        } else if (pureVertical) {
            rb.velocity = new(horizontal * speed, rb.velocity.y * 0.50f);
        } else {
            rb.velocity = new(horizontal * speed, 0f);
        }

        constantForce.force = temp;
        isDashing = false;
        dashTrail.emitting = false;

        yield return new WaitForSeconds(dashingCooldown);

        canDash = true;
    }

    // private IEnumerator RegenStamina()
    // {
    //     yield return new WaitForSeconds(1.0f);

    //     while (currentStamina < maxStamina)
    //     {
    //         currentStamina = Mathf.Min(currentStamina + 0.05f, maxStamina);
    //         yield return null;
    //     }
    // }

    public void UpdateHealthUI()
    {
        DeleteHealthUI();
        for (int i = 0; i < playerHealth; i++)
        {
            GameObject heart = Instantiate(heartPrefab, canvas.transform);
            heart.tag = "HeartUI";

            RectTransform rect = heart.GetComponent<RectTransform>();
            rect.anchoredPosition = new Vector2(330 - (rect.sizeDelta.x /2 * i), 200);
        }
    }

    void DeleteHealthUI()
    {
        foreach (Transform child in canvas.transform)
        {
            if (child.gameObject.CompareTag("HeartUI"))
            {
                Destroy(child.gameObject);
            }
        }
    }


    void OnCollisionEnter2D(Collision2D col) {
        if(col.gameObject.tag == "Bullet") {
            Debug.Log("Hit by bullet");
            playerHealth--;
            UpdateHealthUI();
        }
    }
}
