using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class script_goal_exit : MonoBehaviour {

    private script_manager_gameplay_cannon m_manager_gameplay_cannon;

    //cashed
    private script_puck m_script_puck;

	void Awake()
    {
        m_manager_gameplay_cannon = GameObject.Find("Manager_Gameplay").GetComponent<script_manager_gameplay_cannon>();
    }

    public void OnTriggerExit(Collider collider)
    {
        if (collider.GetComponent<script_puck>())
        {
            m_script_puck = collider.GetComponent<script_puck>();
            if(m_script_puck.m_cannon_fired == true)
            {
                m_manager_gameplay_cannon.Collider_Event(m_script_puck, "exit zone");
            }
        }
        
    }






}
