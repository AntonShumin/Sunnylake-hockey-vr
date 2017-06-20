using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class script_glove_brake : MonoBehaviour {

	void OnTriggerEnter(Collider collider)
    {
        if (collider.GetComponent<script_puck>())
        {
            collider.GetComponent<Rigidbody>().velocity = Vector3.zero;
        }
    }
}
