using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class script_cannon : MonoBehaviour {

    

    //cannon stats
    public float m_shoot_strength = 100f;
    public float m_height_top;
    public float m_height_bottom;
    public float m_width_max;
    public float m_width_min;

    //spowner
    private Transform m_puck_spowner;
    private Vector3 m_puck_spowner_position;

    //sound
    private AudioSource m_Audio_Source;
    public AudioClip[] m_Sounds_Shoot = new AudioClip[5];
    public AudioClip[] m_Sounds_Other = new AudioClip[5];

    //other private
    private script_manager_gameplay_cannon m_manager_gameplay_cannon;
    private Animator m_Animator;
    private ParticleSystem m_particles;
    
    //cached vars
    private Quaternion m_puck_rotation = Quaternion.Euler(-90, 0, 0);
    private Vector3 m_vector_shoot = new Vector3(0, 0, -1f);

    public script_cannon_settings[] m_cannon_settings;

    void Awake()
    {
        m_puck_spowner = transform.FindChild("spowner");
        m_puck_spowner_position = m_puck_spowner.position;
        m_manager_gameplay_cannon = GameObject.Find("Manager_Gameplay").GetComponent<script_manager_gameplay_cannon>();
        m_Animator = GetComponent<Animator>();
        m_particles = transform.FindChild("FireFx").GetComponent<ParticleSystem>();
        m_Audio_Source = GetComponent<AudioSource>();
    }

    public void Set_Next_Wave_Stats(int index)
    {
        m_shoot_strength = m_cannon_settings[index].m_velocity;
        m_height_top = m_cannon_settings[index].m_height_top;
        m_height_bottom = m_cannon_settings[index].m_heigh_bottom;
        m_width_min = m_cannon_settings[index].m_width_min;
        m_width_max = m_cannon_settings[index].m_width_max;

    }


    public void Shoot_prepare()
    {
        //play animation
        m_Animator.SetTrigger("trigger_ready");

        //play sound
        m_Audio_Source.pitch = 0.6f + Random.Range(0, 80) / 100f;
        m_Audio_Source.PlayOneShot(m_Sounds_Other[0],0.3f );
    }

    public void Shoot_play_sound()
    {
        //play sound
        m_Audio_Source.PlayOneShot(m_Sounds_Shoot[Random.Range(0, 4)], 0.4f);
    }

    public void Shoot(Rigidbody puck)
    {
        

        //play animation
        m_Animator.SetTrigger("trigger_shoot");

        //play particles
        m_particles.Play();

        //move puck

        m_vector_shoot.x = Random.Range(m_width_min, m_width_max);
        //m_vector_shoot.x = Random.Range(m_width_max, -m_width_max);
        m_vector_shoot.y = Random.Range(m_height_bottom, m_height_top);

        /*
        m_vector_shoot.x = m_width_max;
        m_vector_shoot.y = m_height_top;
        */

        puck.transform.position = m_puck_spowner_position;
        puck.transform.rotation = m_puck_rotation;
        puck.GetComponent<script_puck>().m_cannon_fired = true;
        puck.velocity = m_vector_shoot * m_shoot_strength;
    }

}
