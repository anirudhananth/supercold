using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballExplosion : MonoBehaviour
{
    [SerializeField] ParticleSystem explosionParticles;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter2D(Collision2D col) {
        if(col.gameObject.tag != "Player") {
            ParticleSystem effect = Instantiate(explosionParticles, transform.position, Quaternion.identity);
            effect.Play();
            Destroy(gameObject);
        }
    }
}
