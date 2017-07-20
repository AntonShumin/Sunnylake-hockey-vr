using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ParticlePlayground;

public class script_test : MonoBehaviour {

    private PlaygroundParticlesC m_particles_block_special;

    // Use this for initialization
    void Start () {
        m_particles_block_special = GameObject.Find("particles_block_special").GetComponent<PlaygroundParticlesC>();
    }

    // Update is called once per frame
    void Update () {

        if(Input.GetKeyDown("space"))
        {
            Debug.Log("space");
            m_particles_block_special.Emit(transform.position);
        }
		
	}
}
