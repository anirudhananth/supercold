using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private float followSpeed = 2f;
    [SerializeField] Transform targetTransform;
    [SerializeField] Vector3 standardOffset;
    [SerializeField] float minYOffsetWhenJumping = -5f;
    [SerializeField] Transform groundReference;
    [SerializeField] float offsetXWhenMoving = 15f;


    private float lastHorizontalInput = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {   
        float dynamicYOffset = Mathf.Lerp(standardOffset.y, minYOffsetWhenJumping, GetJumpHeightFactor());
        
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        if (horizontalInput != lastHorizontalInput && horizontalInput != 0)
        {
            float dynamicXOffset = horizontalInput < 0 ? -Mathf.Abs(offsetXWhenMoving) : Mathf.Abs(offsetXWhenMoving);
            standardOffset.x = dynamicXOffset;
        }
        if (horizontalInput != 0)
        {
            lastHorizontalInput = horizontalInput;
        }
        
        Vector3 dynamicOffset = new Vector3(standardOffset.x, dynamicYOffset, standardOffset.z);

        Vector3 targetPosition = targetTransform.position + dynamicOffset;
        Vector3 newPos = new Vector3(targetPosition.x, targetPosition.y, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, newPos, followSpeed * Time.deltaTime);
    }

    float GetJumpHeightFactor()
    {
        float jumpHeight = Mathf.Clamp(targetTransform.position.y - groundReference.position.y, 0, 5);
        return jumpHeight / 5;
    }
}
