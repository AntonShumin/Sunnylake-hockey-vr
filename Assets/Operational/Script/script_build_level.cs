using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class script_build_level : MonoBehaviour {

    public GameObject m_prefab_cube;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void Awake()
    {
        //preset vars
        Vector3 pos = Vector3.zero;
        Quaternion rot = Quaternion.identity;
        float height_diff = 0.2f;
        int max_blocks = 15;

        //build
        for (int i = -max_blocks; i < max_blocks; i++ )
        {
            height_diff *= -1;
            for (int y= -max_blocks; y <max_blocks; y++)
            {
                pos.x = i;
                height_diff *= -1;
                pos.y = -0.9f + height_diff;
                pos.z = y;
                Instantiate(m_prefab_cube, pos, rot);
            }
        }


    }
}
