using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballShooter : MonoBehaviour
{
    [SerializeField] GameObject fireballPrefab;
    [SerializeField] Transform fireballTransform;
    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) {
            ShootFireball();
        }
    }

    void ShootFireball() {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;

        Vector3 direction = (mousePosition - transform.position).normalized;

        GameObject fireball = Instantiate(fireballPrefab, fireballTransform.position, Quaternion.identity);

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        fireball.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

        rb = fireball.GetComponent<Rigidbody2D>();
        rb.velocity = direction * 15f;
    }
}
