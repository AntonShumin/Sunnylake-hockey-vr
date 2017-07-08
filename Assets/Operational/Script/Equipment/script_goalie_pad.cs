using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class script_goalie_pad : MonoBehaviour {

    //game objects, transforms and rigidbodies
    public Transform m_ground_detector;
    public Transform m_stick;
    private Rigidbody m_Rigidbody;

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


    void Awake()
    {
        m_ground_detector = transform.FindChild("Goalie_Pad_Hand/stick_ground_detector");
        m_stick = transform.FindChild("model_goalie_stick_v1");
        m_stick_original_position = m_stick.position;
        m_Rigidbody = m_stick.GetComponent<Rigidbody>();
    }

	void FixedUpdate()
    {
        Debug.Log(m_stick.position);
        //Move_Stick();
    }

    private void Move_Stick()
    {

        //calculate new position
        m_position_reference = Vector3.zero;
        m_rotation_reference = Quaternion.identity;

        


        //VRTK movement
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
