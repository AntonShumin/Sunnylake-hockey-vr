using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class script_cannon : MonoBehaviour {

    public GameObject m_puck_prefab;
    public float m_shoot_frequency = 2f;
    public float m_shoot_strength = 100f;
    public float m_height;
    public float m_width;

    private Transform m_spowner;
    private Collider m_collider_exit;
    private script_manager_gameplay_cannon m_manager_gameplay_cannon;
    private Rigidbody[] m_pucks = new Rigidbody[5];
    private int m_current_puck = 0;
    private Vector3 m_spowner_position;
    private Quaternion m_puck_rotation = Quaternion.Euler(-90, 0, 0);
    
    //cached vars
    public Vector3 m_vector_shoot = new Vector3(0,0,1f);

    void Awake()
    {
        m_spowner = transform.FindChild("spowner");
        m_spowner_position = m_spowner.position;
        m_manager_gameplay_cannon = GameObject.Find("Manager_Gameplay").GetComponent<script_manager_gameplay_cannon>();
        
        for ( int i = 0; i < 5; i++ )
        {
            m_pucks[i] = GameObject.Instantiate(m_puck_prefab, m_spowner_position, Quaternion.identity).GetComponent<Rigidbody>();
            //m_pucks[i].gameObject.SetActive(false);
        }
    }

    void Start()
    {
        m_collider_exit = GameObject.Find("Collider Exit").GetComponent<Collider>();
    }

	
	public void Game_Event(string event_name)
    {
        switch (event_name)
        {

            case "cannon fire":
                StartCoroutine(Shoot_Puck());
                break;
            case "stop":
                StopAllCoroutines();
                break;
        }
        
    }

    IEnumerator Shoot_Puck()
    {
        while(true)
        {


            //fire
            m_collider_exit.enabled = false;
            m_vector_shoot.x = Random.Range(-m_width, m_width);
            m_vector_shoot.y = Random.Range(0.05f, m_height);
            m_pucks[m_current_puck].transform.position = m_spowner_position;
            m_pucks[m_current_puck].transform.rotation = m_puck_rotation;
            m_pucks[m_current_puck].GetComponent<script_puck>().m_cannon_fired = true;
            m_pucks[m_current_puck].velocity = m_vector_shoot * m_shoot_strength;
            m_collider_exit.enabled = true;
            m_manager_gameplay_cannon.Game_Event("count shots");

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
}
