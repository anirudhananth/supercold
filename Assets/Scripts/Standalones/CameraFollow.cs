using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private float followSpeed = 1f;
    [SerializeField] Transform targetTransform;
    [SerializeField] Vector3 standardOffset;
    [SerializeField] float minYOffsetWhenJumping = -5f;
    [SerializeField] Transform groundReference;
    [SerializeField] float offsetXWhenMoving = 10f;
    public float offsetXChangeSpeed = 2.0f; // Control the speed of the offset change
    private float targetXOffset = 5f;

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
            targetXOffset = horizontalInput < 0 ? -Mathf.Abs(offsetXWhenMoving) : Mathf.Abs(offsetXWhenMoving);
            lastHorizontalInput = horizontalInput;
        }

        standardOffset.x = Mathf.Lerp(standardOffset.x, targetXOffset, offsetXChangeSpeed * Time.deltaTime);


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
