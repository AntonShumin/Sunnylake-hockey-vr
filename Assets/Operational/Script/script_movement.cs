using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class script_movement : MonoBehaviour {

    //public
    public bool b_skating;

    //GameObject
    private GameObject m_player;
    private GameObject m_controller;

    //Vector3
    private Vector3 m_position_last;
    private Vector3 m_position_current;
    private Vector3 m_position_difference;
    private Vector3 m_position_difference_plane;
    private Vector3 m_swipe_start = Vector3.zero;
    private Vector3 m_swipe_end;
    

    //Float
    private float m_position_difference_y;
    private float m_dt;
    private float m_transport_delta; //how far the controller moved over time
    private float m_transport_delta_plane;
    private float m_transport_delta_y;
    private float m_total_distance;
    private float m_total_distance_plane;
    private float m_total_distance_y;
    private float m_timer;
    private float m_timer_idle;

    //misc
    private Rigidbody m_rigidbody;


    //cached
    private float c_dotProduct;
    private float c_force_strength;
    private Vector3 c_direction;
    private float c_distance;


    void Update()
    {
        if (b_skating) Skate();
    }

    void Start()
    {
        m_controller = gameObject;
        m_player = GameObject.Find("[CameraRig]");
        m_rigidbody = m_player.GetComponent<Rigidbody>();
    }

    private void Skate()
    {
        /*******************************************************************
         * Base position and velocity
         * Subtract the movement of the player from the controller movemet
         ******************************************************************/
        m_position_current = m_controller.transform.position - m_player.transform.position;
        if (m_position_last == null) m_position_last = m_position_current;
        m_position_difference = m_position_difference_plane = m_position_current - m_position_last;
        m_position_difference_y = m_position_difference.y;
        m_position_difference_plane.y = 0f;
        m_position_last = m_position_current;


        /******************************************
         *****************Set vars****************** 
         *******************************************/
        m_dt = Time.deltaTime;
        m_transport_delta = m_position_difference_plane.magnitude / m_dt;
        m_transport_delta_plane = m_position_difference_plane.magnitude / m_dt;
        m_transport_delta_y = m_position_difference_y / m_dt;


        /***********************************************
         * ***************Swiper detections*************
         **********************************************/ 
        if(m_transport_delta > 0.5f) {

            m_total_distance += m_transport_delta;
            m_total_distance_plane += m_transport_delta_plane;
            m_total_distance_y += m_transport_delta_y;
            m_timer_idle = 0;

            //record swipe if its the first movement
           
            if (m_swipe_start == Vector3.zero)
            {
                m_swipe_start = m_position_last;
                m_swipe_end = m_position_last;
            }

            //calculate swipe end position
            m_swipe_end += m_position_difference_plane;
            

        } else if (m_total_distance > 0 )
        {
            m_timer_idle += m_dt;
        }


        /**************************************************
         * *****************Record time*********************
         **************************************************/ 
    
        if(m_total_distance > 0)
        {
            m_timer += m_dt;
            
        }


        /*****************************************************
         ************** Detect swiper break******************* 
         ****************************************************/

        //direction change
        c_direction = Vector3.Normalize(m_swipe_end - m_swipe_start);
        c_dotProduct = Vector3.Dot(c_direction, Vector3.Normalize(m_position_difference));
        c_distance = Vector3.Distance(m_swipe_end, m_swipe_start);

        //time break or direction break
        if ( m_swipe_start != Vector3.zero )
        {
            if (m_timer_idle >= 0.1f || c_dotProduct < 0)
            {

                

                if (c_distance > 0.5f)
                {
                    //set velocity
                    if( Vector3.Dot(m_rigidbody.velocity, m_swipe_end - m_swipe_start) < 0 )
                    {
                        //add force if the same direction
                        m_rigidbody.velocity += c_direction * c_distance * -1;
                    } else
                    {
                        //full break on reverse direction
                        m_rigidbody.velocity = c_direction * c_distance * -1;
                    }
                    
                    //Debug.Log("end with " + c_direction * c_force_strength + " distance " + c_distance);

                    
                } else
                {
                    //Debug.Log("distance " + c_distance);
                }

                //reset
                m_timer = 0;
                m_timer_idle = 0;
                m_swipe_start = Vector3.zero;
                m_swipe_end = Vector3.zero;

            }
        }
        
        

        

        
        

         




    }
    

}
