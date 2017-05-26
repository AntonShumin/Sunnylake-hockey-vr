using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class script_stick_helper : MonoBehaviour {

    private script_grabbable m_grabbable_parent; 

    void Awake()
    {
        m_grabbable_parent = transform.parent.gameObject.GetComponent<script_grabbable>();
    }

    public void OnCollisionEnter(Collision collision)
    {
        m_grabbable_parent.OnCollisionEnter(collision);
    }

    public void GroundEnter()
    {
        m_grabbable_parent.GroundEnter();
    }
}
