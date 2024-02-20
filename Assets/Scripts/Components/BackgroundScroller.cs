using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScroller : MonoBehaviour
{
    private float scollingSpeed = 0.2f;
    private bool isMovingLeft = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isMovingLeft && transform.position.x < -10.0f)
        {
            isMovingLeft = false;
        }
        else if (!isMovingLeft && transform.position.x > 0.0f)
        {
            isMovingLeft = true;
        }
        if (isMovingLeft)
        {
            transform.position += Vector3.left * Time.deltaTime * scollingSpeed;
        }
        else
        {
            transform.position += Vector3.right * Time.deltaTime * scollingSpeed;
        }
    }
}
