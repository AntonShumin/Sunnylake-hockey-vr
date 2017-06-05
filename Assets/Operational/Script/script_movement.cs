using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class script_movement : MonoBehaviour {

    public bool m_move = false;
    public GameObject m_right_controller;
    public Transform m_transform;
    public GameObject m_reference_center;
    public GameObject m_reference_headset;
    public Rigidbody m_reference_cameraRig;

    //cameraRig movement
    private Vector3 m_cameraRig_position;
    private Vector3 m_cameraRig_position_last;


    //caching
    private Vector3 m_reference_center_position;
    private Vector3 m_controller_feet_position;

    void Awake()
    {
        //right controller
        m_right_controller = transform.parent.gameObject;
        m_skate_position_current_raw = m_right_controller.transform.position;
        //camera
        m_reference_headset = Camera.main.gameObject;
        //camera Rig
        m_reference_cameraRig = GameObject.Find("[CameraRig]").GetComponent<Rigidbody>();
        m_cameraRig_position = m_reference_cameraRig.transform.position;
    }

	// Use this for initialization
	void Start () {
        //m_reference_cameraRig.velocity = new Vector3(5, 0, 0);
    }
	
	// Update is called once per frame
	void Update () {

        CameraRig_position_tracking();
        Feet_swipe_skating();

    }

    private void CameraRig_position_tracking()
    {
        m_cameraRig_position_last = m_cameraRig_position;
        m_cameraRig_position = m_reference_cameraRig.transform.position;

        
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


    //skate swipe strength
    private Vector3 m_skate_position_last_raw;
    private Vector3 m_skate_position_current_raw;
    private Vector3 m_skate_position_last;
    private Vector3 m_skate_position_current;
    private Vector3 m_skate_diff;
    private float m_skate_velocity;
    private float m_total_distance;

    //skate swiper vector
    private Vector3 m_skate_headset_position;
    private Vector3 m_skate_headset_diffVector;
    private float m_skate_headset_dot;


    private void Feet_swipe_skating()
    {
        //set basic vars
        m_skate_position_last_raw = m_skate_position_current_raw;
        m_skate_position_current_raw = m_right_controller.transform.position;
        //substract CameraRig movement
        m_skate_position_current = m_skate_position_current_raw - m_cameraRig_position;
        m_skate_position_last = m_skate_position_last_raw - m_cameraRig_position_last;

        

        //swipe vector
        m_skate_diff = m_skate_position_current - m_skate_position_last;
        m_skate_velocity = Vector3.Magnitude(m_skate_diff) / Time.deltaTime;
        
        //headset to leg vector
        m_skate_headset_position = m_reference_headset.transform.position;
        m_skate_headset_diffVector = m_skate_position_current_raw - m_skate_headset_position;
        m_skate_headset_dot = Vector3.Dot(m_skate_headset_diffVector, m_skate_diff);
        Debug.Log(m_skate_headset_dot);

        //action
        if (m_skate_velocity > 0.3 && m_skate_headset_dot > 0)
        {
            m_total_distance += Vector3.Magnitude(m_skate_diff);
        }
        else
        {
            if (m_total_distance > 0)
            {
                //Debug.Log(m_total_distance);
                if(m_total_distance > 0.5)
                {
                    m_reference_cameraRig.velocity = m_skate_diff * -5;
                }
                m_total_distance = 0;
            }

        }
        
        

    }

    public void Spown_Ball()
    {

    }
}
