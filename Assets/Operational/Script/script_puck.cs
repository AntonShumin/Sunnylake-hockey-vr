using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class script_puck : MonoBehaviour {

    //public
    public bool m_cannon_fired = false;

    //static
    private static script_cannon_settings.hot m_hot;

    //private
    private float m_time_idle = 0f;
    private Rigidbody m_rigidbody;
    private script_manager_gameplay_cannon m_manager_gameplay_cannon;

    //cashed
    private float m_velocity;

    void Awake()
    {
        m_rigidbody = GetComponent<Rigidbody>();
        m_manager_gameplay_cannon = GameObject.Find("Manager_Gameplay").GetComponent<script_manager_gameplay_cannon>();
    }

    void Update()
    {
        if (m_cannon_fired) Activity_Tracker();
    }

    private void Activity_Tracker()
    {
        m_velocity = m_rigidbody.velocity.magnitude;
        if(m_velocity < 1 )
        {
            m_time_idle += Time.deltaTime;
            if( m_time_idle > 0.5 )
            {
                m_manager_gameplay_cannon.Collider_Event(this, "exit zone");
            }
        } else
        {
            m_time_idle = 0f;
        }
    }

    public static void set_hot(script_cannon_settings.hot hot)
    {
        m_hot = hot;
    }





}
