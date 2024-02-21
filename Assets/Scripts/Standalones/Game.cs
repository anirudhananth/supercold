using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour
{
    [NonSerialized] public List<ParticleSystem> explosionParticlesList;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject DeathYPosition;
    // Start is called before the first frame update
    void Start()
    {
        explosionParticlesList = new List<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        List<ParticleSystem> t = new List<ParticleSystem>();
        foreach (ParticleSystem p in explosionParticlesList) {
            if(p.gameObject && p.isStopped) {
                Destroy(p.gameObject);
                t.Add(p);
            }
        }

        explosionParticlesList.RemoveAll(p => t.Contains(p));

        if(player.transform.position.y < DeathYPosition.transform.position.y) {
            Movement playerMovement = player.GetComponent<Movement>();
            if(playerMovement.playerHealth > 0) {
                playerMovement.Restart();
            } else {
                SceneManager.LoadScene("GameOver");
            }
        }
    }
}
