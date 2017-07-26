using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ParticlePlayground;

public class script_test : MonoBehaviour {

    private PlaygroundParticlesC m_particles;
    private PlaygroundParticlesC m_particle_drip;

    // Use this for initialization
    void Start () {
        m_particles = GameObject.Find("particles_goal").GetComponent<PlaygroundParticlesC>();
    }

    // Update is called once per frame
    void Update () {

        if(Input.GetKeyDown("space"))
        {
            Debug.Log("space");
            m_particles.Emit(true);
        }
		
	}
}
