using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class script_movement : MonoBehaviour {

    public bool m_move = false;
    public GameObject m_right_controller;
    public Transform m_transform;
    public GameObject m_reference_center;
    public GameObject m_reference_headset;


    //caching
    private Vector3 m_reference_center_position;
    private Vector3 m_controller_feet_position;

    void Awake()
    {
        m_right_controller = gameObject;
        m_reference_headset = Camera.main.gameObject;
    }

	// Use this for initialization
	void Start () {
       
    }
	
	// Update is called once per frame
	void Update () {
        if(m_right_controller) Feet_swipe_skating();
	}

    void FixedUpdate()
    {
        if (m_move)
        {
            //Vector3 forward = m_transform.forward;
            //transform.GetComponent<Rigidbody>().velocity = forward * 2;
        } else
        {
            //transform.GetComponent<Rigidbody>().velocity = Vector3.zero;
            //Physics.ignore
        }
    }

    private void Feet_scating()
    {
        //get data
        m_reference_center_position = m_reference_center.transform.position;
        m_controller_feet_position = m_right_controller.transform.position;
        float y_controller = m_controller_feet_position.y;
        float y_reference = m_reference_center_position.y;
        m_controller_feet_position.y = 0;
        m_reference_center_position.y = 0;

        //calc
        float diff_y = y_controller - y_reference;
        Vector3 dir_xz = m_controller_feet_position - m_reference_center_position;
        float dist_xz = Vector3.Distance(m_reference_center_position, m_controller_feet_position);

        if(dist_xz > 0.3)
        {
            transform.GetComponent<Rigidbody>().velocity = dir_xz * 5;
        } else
        {
            transform.GetComponent<Rigidbody>().velocity = Vector3.zero;
        }

        Debug.Log(dist_xz);
    }

    private Vector3 m_hip_original_position;
    private Vector3 m_hip_original_rotation;

    private void Hip_abstract_scating()
    {
        //get data
        Vector3 hip_position = m_right_controller.transform.position;
        float hip_rotation_y = m_right_controller.transform.rotation.y;
        
        if(Mathf.Abs(hip_rotation_y)  > 0.3)
        {
            Debug.Log(-1000 * Time.deltaTime * hip_rotation_y);
            transform.GetComponent<Rigidbody>().angularVelocity = new Vector3(0,-1000 * Time.deltaTime * hip_rotation_y, 0);
        } else
        {
            transform.transform.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        }
    }


    private Vector3 m_skate_position_last = Vector3.zero;
    private Vector3 m_skate_position_current;
    private Vector3 m_skate_diff;
    private float m_skate_velocity;
    private float m_total_distance;


    private void Feet_swipe_skating()
    {
        m_skate_position_current = m_right_controller.transform.position;
        m_skate_diff = m_skate_position_current - m_skate_position_last;
        m_skate_velocity = Vector3.Magnitude(m_skate_diff)  / Time.deltaTime;
        if(m_skate_velocity > 0.5)
        {
            m_total_distance += Vector3.Magnitude(m_skate_diff);
            Debug.Log(m_total_distance);
        } else
        {
            Debug.Log("--------------reset");
            m_total_distance = 0;
        }
        

        //store values
        m_skate_position_last = m_skate_position_current;
    }

    public void Spown_Ball()
    {

    }
}
