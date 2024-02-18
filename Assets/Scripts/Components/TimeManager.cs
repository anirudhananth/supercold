using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public Rigidbody2D playerRb;

    private float scaleFactor = 0.05f;
    private bool isInSlowMotion = false;
    private float startFixedDeltaTime;

    private void Start()
    {

        startFixedDeltaTime = Time.fixedDeltaTime;
    }

    private void Update()
    {
        if (!isInSlowMotion && playerRb.velocity == Vector2.zero)
        {
            isInSlowMotion = true;
            Time.timeScale = scaleFactor;
            Time.fixedDeltaTime = startFixedDeltaTime * scaleFactor;
            Debug.Log("Slow motion on");
        }
        else if (isInSlowMotion && playerRb.velocity != Vector2.zero)
        {
            isInSlowMotion = false;
            Time.timeScale = 1.0f;
            Time.fixedDeltaTime = startFixedDeltaTime;
            Debug.Log("Slow motion off");
        }
    }
}
