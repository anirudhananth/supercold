using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Bullets : MonoBehaviour
{
    public float speed = 20f;
    private Rigidbody2D rb;
    private Animator animator;
    private SoundManager soundManager;
    
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        soundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>();
        rb = GetComponent<Rigidbody2D>();
        Vector2 moveDirection = (GameObject.FindGameObjectWithTag("Player").transform.position - transform.position).normalized;
        rb.velocity = moveDirection * speed;

        float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
        rb.rotation = angle;
    }

    // Update is called once per frame
    void Update()
    {
        if(!RendererIsVisible())
        {
            Destroy(gameObject);
        }
    }

    bool RendererIsVisible()
    {
        var cam = Camera.main;
        var viewportPosition = cam.WorldToViewportPoint(transform.position);
        bool isVisible = viewportPosition.x >= 0 && viewportPosition.x <= 1 && viewportPosition.y >= 0 && viewportPosition.y <= 1;
        return isVisible;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        animator.SetTrigger("Hit");
        soundManager.PlayFireballExplode();
        rb.velocity = Vector2.zero;
        StartCoroutine(DestroyBullet());
    }

    private IEnumerator DestroyBullet() {
        GetComponent<Collider2D>().enabled = false;

        yield return new WaitForSeconds(1f);

        Destroy(gameObject);
    }
}
