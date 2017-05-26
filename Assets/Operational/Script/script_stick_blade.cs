using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class script_stick_blade : MonoBehaviour {

    //public vars
    public GameObject m_Blade_Reference;
    public GameObject m_Ground;

    
    //helper vars
    private Transform m_Blade_Helper;
    private Rigidbody m_Rigidbody;
    private float angularVelocityLimit = float.PositiveInfinity;
    private float velocityLimit = float.PositiveInfinity;

    //blade vars
    private Vector3 m_position_reference;
    private Quaternion m_rotation_reference;

    //VRTK cashed vars
    float maxDistanceDelta = 10f;
    float angle;
    Vector3 axis;
    Vector3 positionDelta;
    Quaternion rotationDelta;


    void Awake()
    {
        //Physics.IgnoreCollision(transform.GetComponent<BoxCollider>(), m_Ground.GetComponent<Collider>(), true);
        m_Blade_Helper = m_Blade_Reference.transform.FindChild("helper");
        m_Rigidbody = GetComponent<Rigidbody>();
        m_Ground = GameObject.Find("Ice_Plane");
    }

	

    void FixedUpdate()
    {
        
        //calculate new position
        m_position_reference = m_Blade_Reference.transform.position;
        m_rotation_reference = m_Blade_Reference.transform.rotation;

        if (m_Ground.transform.position.y > m_Blade_Helper.position.y)
        {
            //new posisition y = add difference between helper and the ground
            m_position_reference.y += m_Ground.transform.position.y - m_Blade_Helper.position.y;
        }

        

        rotationDelta = m_rotation_reference * Quaternion.Inverse(transform.rotation);
        positionDelta = m_position_reference - transform.position;

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

    public void OnCollisionEnter(Collision collision)
    {
        m_Blade_Reference.GetComponent<script_stick_helper>().OnCollisionEnter(collision);
    }

    public void GroundEnter()
    {
        m_Blade_Reference.GetComponent<script_stick_helper>().GroundEnter();
    }
}
