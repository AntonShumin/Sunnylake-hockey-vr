using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class script_puck : MonoBehaviour {

    //public
    public bool m_cannon_fired = false;
    public bool m_glove_touched = true; //used to brake speed on glove touch in script_grabbalble
    public AudioClip[] m_sounds;
    public bool m_hot_touched;

    //static
    public static script_cannon_settings.hot m_hot;

    //private
    private float m_time_idle = 0f;
    private Rigidbody m_rigidbody;
    private script_manager_gameplay_cannon m_manager_gameplay_cannon;
    private AudioSource m_audio_source;

    //cashed
    private float m_velocity;

    void Awake()
    {
        m_rigidbody = GetComponent<Rigidbody>();
        m_manager_gameplay_cannon = GameObject.Find("Manager_Gameplay").GetComponent<script_manager_gameplay_cannon>();
        m_audio_source = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (m_cannon_fired) Activity_Tracker();
    }

    public void Game_Events(string event_name)
    {
        switch (event_name)
        {
            case ("score sound"):
                m_audio_source.pitch = Random.Range(0.9f, 1.1f);
                m_audio_source.PlayOneShot(m_sounds[Random.Range(0,3)]);
                break;
            
        }

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






}
