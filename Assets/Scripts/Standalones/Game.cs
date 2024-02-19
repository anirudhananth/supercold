using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    [NonSerialized] public List<ParticleSystem> explosionParticlesList;
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
    }
}
