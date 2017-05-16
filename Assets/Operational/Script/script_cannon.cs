using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class script_cannon : MonoBehaviour {

    public float m_shoot_frequency = 2f;
    public float m_shoot_strength = 100f;
    public float m_height;
    public float m_width;



    private script_manager_gameplay_cannon m_manager_gameplay_cannon;
    
    
    private Quaternion m_puck_rotation = Quaternion.Euler(-90, 0, 0);

    //cached vars
    private Vector3 m_vector_shoot = new Vector3(0, 0, 1f);

    void Awake()
    {
        
        m_manager_gameplay_cannon = GameObject.Find("Manager_Gameplay").GetComponent<script_manager_gameplay_cannon>();
    }

    void Start()
    {
        
    }

	
	public void Game_Event(string event_name)
    {
        
    }

    IEnumerator Shoot_Puck()
    {
        while(true)
        {


            //cleanup
            m_current_puck++;
            if (m_current_puck > m_pucks.Length - 1)
            {
                m_current_puck = 0;
            }

            //delay
            yield return new WaitForSeconds(m_shoot_frequency);


        }
    }

    public void Shoot_prepare()
    {
        //play animation
    }

    public void Shoot(GameObject puck)
    {
        //play animation

        //play particles
    }
}
