using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class script_stick_grounddetect : MonoBehaviour {

    private script_stick_blade m_parent;

    void Awake()
    {
        m_parent = transform.parent.GetComponent<script_stick_blade>();
    }

    public void OnCollisionEnter(Collision collision)
    {

        m_parent.OnCollisionEnter(collision);

    }

    public void OnTriggerEnter(Collider collider)
    {
        //m_parent.GroundEnter();
    }
}
