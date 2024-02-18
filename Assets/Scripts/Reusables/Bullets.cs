using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Bullets : MonoBehaviour
{
    public float speed = 20f;
    private Rigidbody2D rb;
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Vector2 moveDirection = (GameObject.FindGameObjectWithTag("Player").transform.position - transform.position).normalized;
        rb.velocity = moveDirection * speed;
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
        Destroy(gameObject);
    }
}
