using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class script_blade_movement : MonoBehaviour {

    //game objects, transforms and rigidbodies
    private Transform m_pad_reference_ground;
    private Transform m_pad_reference_stick;
    private Transform m_stick;
    private Rigidbody m_Rigidbody;
    private Transform m_Ground;

    //positions
    public Vector3 m_stick_original_position;

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


    // Use this for initialization
    void Start () {
		
	}

    void FixedUpdate()
    {
        //Debug.Log(m_stick.position);
        move_blade();
    }


    private void move_blade()
    {

        //calculate new position

        //if below ground
        c_p1 = m_pad_reference_stick.position;
        c_p0 = m_pad_reference_ground.position;
        c_pg = m_Ground.position;
        if (c_p0.y < c_pg.y)
        {
            c_k = (c_pg.y - c_p1.y) / (c_p0.y - c_p1.y);
            c_pg.x = c_p1.x + c_k * (c_p0.x - c_p1.x);
            c_pg.z = c_p1.z + c_k * (c_p0.z - c_p1.z);


            //else set stick height to reference height        
        }
        else
        {
            c_pg = c_p0;

        }

        //define new coordinates
        m_position_reference = c_pg + c_stick_offset;
        m_rotation_reference = m_pad_reference_stick.rotation;



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
