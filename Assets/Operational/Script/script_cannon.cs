using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class script_cannon : MonoBehaviour {

    //cannon stats
    public float m_shoot_strength = 100f;
    public float m_height;
    public float m_width;

    //spowner
    private Transform m_puck_spowner;
    private Vector3 m_puck_spowner_position;

    //other private
    private script_manager_gameplay_cannon m_manager_gameplay_cannon;
    private Animator m_Animator;
    private ParticleSystem m_particles;


    //cached vars
    private Quaternion m_puck_rotation = Quaternion.Euler(-90, 0, 0);
    private Vector3 m_vector_shoot = new Vector3(0, 0, -1f);

    void Awake()
    {
        m_puck_spowner = transform.FindChild("spowner");
        m_puck_spowner_position = m_puck_spowner.position;
        m_manager_gameplay_cannon = GameObject.Find("Manager_Gameplay").GetComponent<script_manager_gameplay_cannon>();
        m_Animator = GetComponent<Animator>();
        m_particles = transform.FindChild("FireFx").GetComponent<ParticleSystem>();
    }

    void Start()
    {
        
    }

	
	public void Game_Event(string event_name)
    {
        
    }

    public void Shoot_prepare()
    {
        //play animation
        m_Animator.SetTrigger("trigger_ready");
    }

    public void Shoot(Rigidbody puck)
    {
        //play animation
        m_Animator.SetTrigger("trigger_shoot");

        //play particles
        m_particles.Play();

        //move puck
        m_vector_shoot.x = Random.Range(-m_width, m_width);
        m_vector_shoot.y = Random.Range(0.05f, m_height);
        puck.transform.position = m_puck_spowner_position;
        puck.transform.rotation = m_puck_rotation;
        puck.GetComponent<script_puck>().m_cannon_fired = true;
        puck.velocity = m_vector_shoot * m_shoot_strength;
    }
}
