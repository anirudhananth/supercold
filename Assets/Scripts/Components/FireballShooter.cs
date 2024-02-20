using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballShooter : MonoBehaviour
{
    [SerializeField] GameObject fireballPrefab;
    [SerializeField] Transform fireballTransform;
    private Rigidbody2D rb;
    private float fireballSpeed = 17.5f;
    private bool canShoot = true;
    [SerializeField] Animator playerAnimator;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update() {
        if (canShoot && Input.GetMouseButtonDown(0)) {
            CheckPosition();
        }
    }

    void CheckPosition() {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;

        Vector3 direction = (mousePosition - fireballTransform.position).normalized;

        if(transform.localScale.x > 0) {
            if(direction.x < 0) {
                return;
            }
        } else {
            if(direction.x > 0) {
                return;
            }
        }
        StartCoroutine(ShootFireball(direction));
    }

    private IEnumerator ShootFireball(Vector3 direction) {
        canShoot = false;
        playerAnimator.SetTrigger("Attack");

        yield return new WaitForSeconds(0.4f);
        
        GameObject fireball = Instantiate(fireballPrefab, fireballTransform.position, Quaternion.identity);

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        fireball.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

        rb = fireball.GetComponent<Rigidbody2D>();
        rb.velocity = direction * fireballSpeed;

        canShoot = true;
    }
}
