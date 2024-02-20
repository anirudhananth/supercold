using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballExplosion : MonoBehaviour
{   
    private SoundManager soundManager;
    [SerializeField] ParticleSystem explosionParticles;
    [SerializeField] Game game;
    // Start is called before the first frame update
    void Start()
    {   
        soundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>();
        game = FindObjectOfType<Game>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter2D(Collision2D col) {
        if(col.gameObject.tag != "Player") {
            soundManager.PlaySkullExplode();
            ParticleSystem effect = Instantiate(explosionParticles, transform.position, Quaternion.identity);
            effect.Play();
            game.explosionParticlesList.Add(effect);
            Destroy(gameObject);
        }
    }
}
