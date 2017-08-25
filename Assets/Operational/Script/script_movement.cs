using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class script_movement : MonoBehaviour {

    //public
    public bool b_skating;

    //private
    private GameObject m_player;
    private GameObject m_controller;
    private Vector3 m_position_last;
    private Vector3 m_position_current;
    private Vector3 m_possition_difference;
    private Vector3 m_position_difference_plane;
    private Vector3 m_direction;
    private float m_position_difference_y;
    private float m_dt;
    private float m_transport_delta; //how far the controller moved over time
    private float m_transport_delta_plane;
    private float m_transport_delta_y;
    private float m_total_distance;
    private float m_total_distance_plane;
    private float m_total_distance_y;
    private float m_timer;

    //cached
    private float c_dotProduct;
    


    void Update()
    {
        if (b_skating) Skate();
    }

    void Start()
    {
        m_controller = gameObject;
        m_player = GameObject.Find("[CameraRig]");
    }

    private void Skate()
    {
        /*
         * Base position and velocity
         * Subtract the movement of the player from the controller movemet
         */
        m_position_current = m_controller.transform.position - m_player.transform.position;
        if (m_position_last == null) m_position_last = m_position_current;
        m_possition_difference = m_position_difference_plane = m_position_current - m_position_last;
        m_position_difference_y = m_possition_difference.y;
        m_position_difference_plane.y = 0f;
        m_position_last = m_position_current;


        /*
         *Set vars 
         */
        m_dt = Time.deltaTime;
        m_transport_delta = m_position_difference_plane.magnitude / m_dt;
        m_transport_delta_plane = m_position_difference_plane.magnitude / m_dt;
        m_transport_delta_y = m_position_difference_y / m_dt;


        /*
         * Swiper detections
         */ 
        if(m_transport_delta > 0.5f) {

            m_total_distance += m_transport_delta;
            m_total_distance_plane += m_transport_delta_plane;
            m_total_distance_y += m_transport_delta_y;

            //record swipe if its the first movement
            // -------- fout, herschrijf met xz_begin en xz_eind
            if (m_direction == null) m_direction = Vector3.Normalize(m_position_difference_plane);

        }


        /*
         * Record time
         */ 
    
        if(m_total_distance > 0)
        {
            m_timer += m_dt;
            
        }


        /*
         * Detect swiper break 
         */

        //direction change
        c_dotProduct = Vector3.Dot(m_direction, Vector3.Normalize(m_position_difference_plane));

         //time break




    }
    

}
