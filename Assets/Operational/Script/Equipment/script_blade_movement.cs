using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class script_blade_movement : MonoBehaviour {

    //blade offset
    public Vector3 m_position_offset;
    public Vector3 m_rotation_offset;

    //game objects, transforms and rigidbodies
    private Transform m_helper_ground_front;
    private Transform m_helper_ground_back;
    private Transform m_stick_bladereference;
    private Transform m_stick;
    private Rigidbody m_Rigidbody;
    private Transform m_Ground;

    //VRTK move natives
    private Vector3 m_position_reference;
    private Quaternion m_rotation_reference;
    float maxDistanceDelta = 10f;
    float angle;
    Vector3 axis;
    Vector3 positionDelta;
    Quaternion rotationDelta;
    private float angularVelocityLimit = float.PositiveInfinity;
    private float velocityLimit = float.PositiveInfinity;

    //cached
    private Vector3 c_p1;
    private Vector3 c_p0;
    private Vector3 c_pg;
    private float c_k;
    private Vector3 c_stick_offset;
    private float c_ground_helper;


    // Use this for initialization
    void Start () {

        m_Ground = GameObject.Find("Ground").transform;
        m_stick = transform.parent.FindChild("base");
        m_stick_bladereference = m_stick.FindChild("blade_reference");
        m_helper_ground_front = transform.FindChild("helper_ground_front");
        m_helper_ground_back = transform.FindChild("helper_ground_back");
        m_Rigidbody = GetComponent<Rigidbody>();
		
	}

    void FixedUpdate()
    {
        move_blade();
    }


    private void move_blade()
    {

        //if below ground
        c_p1 = m_stick.position;
        c_p0 = m_stick_bladereference.position;
        c_pg = m_Ground.position;
        c_ground_helper = Mathf.Min(m_helper_ground_front.position.y, m_helper_ground_back.position.y);
        if (c_ground_helper < c_pg.y)
        {
            c_k = (c_pg.y - c_p1.y) / (c_p0.y - c_p1.y);
            c_pg.x = c_p1.x + c_k * (c_p0.x - c_p1.x);
            c_pg.z = c_p1.z + c_k * (c_p0.z - c_p1.z);
      
        } else
        {
            c_pg = c_p0;
        }
      
        m_position_reference = c_pg;
        m_rotation_reference = m_stick_bladereference.rotation * Quaternion.Euler(m_rotation_offset);


        //VRTK - move the stick
        rotationDelta = m_rotation_reference * Quaternion.Inverse(transform.rotation);
        positionDelta = m_position_reference - transform.position;

        //correct rotation
        rotationDelta.ToAngleAxis(out angle, out axis);
        angle = ((angle > 180) ? angle -= 360 : angle);

        if (angle != 0)
        {
            Vector3 angularTarget = angle * axis;
            Vector3 calculatedAngularVelocity = Vector3.MoveTowards(m_Rigidbody.angularVelocity, angularTarget, maxDistanceDelta);
            if (angularVelocityLimit == float.PositiveInfinity || calculatedAngularVelocity.sqrMagnitude < angularVelocityLimit)
            {
                m_Rigidbody.angularVelocity = calculatedAngularVelocity;
            }
        }

        Vector3 velocityTarget = positionDelta / Time.fixedDeltaTime;
        Vector3 calculatedVelocity = Vector3.MoveTowards(m_Rigidbody.velocity, velocityTarget, maxDistanceDelta);

        if (velocityLimit == float.PositiveInfinity || calculatedVelocity.sqrMagnitude < velocityLimit)
        {
            m_Rigidbody.velocity = calculatedVelocity;
        }

    }
}
