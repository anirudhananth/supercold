using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    private Movement player;

    private SoundManager soundManager;
    private GameObject messageText; // Assign text in the Unity Editor

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Movement>();
        messageText = GameObject.Find("Canvas").transform.Find("DoorMessage").gameObject;
        soundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>();
        messageText.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if(player.GetNumOfKeys()<1)
            {
                messageText.SetActive(true);
                
            }
            else
            {
                soundManager.PlayNextLevel();
            }

        }

    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if(player.GetNumOfKeys()<1)
            {
                messageText.SetActive(false);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            messageText.SetActive(true);
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            messageText.SetActive(false); // Hide the UI text when player exits the trigger zone
        }
    }
}
