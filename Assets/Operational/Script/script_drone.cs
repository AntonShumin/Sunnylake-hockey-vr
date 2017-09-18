using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class script_drone : MonoBehaviour {

    public GameObject m_ball_prefab;
    private Vector3 m_position;
    private GameObject[] m_balls = new GameObject[5];
    private int m_ball_index = 0;

    public Vector3 c_target;

	// Use this for initialization
	void Start () {

        for (int i = 0; i < 5; i++)
        {
            m_balls[i] = GameObject.Instantiate(m_ball_prefab);
            
        }

        m_position = transform.position;
        m_position.y = 0.1f;
        StartCoroutine("Shoot");

	}
	
	// Update is called once per frame
	void Update () {
		
	}

    IEnumerator Shoot()
    {
        while(true)
        {
            m_ball_index++;
            if (m_ball_index >= m_balls.Length) m_ball_index = 0;
            m_balls[m_ball_index].transform.position = m_position;
            yield return new WaitForSeconds(2);
        }

    }
}
