using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class script_glove_brake : MonoBehaviour {

	void OnTriggerEnter(Collider collider)
    {
        if (collider.GetComponent<script_puck>())
        {
            Debug.Log(collider.GetComponent<Rigidbody>().velocity.magnitude);
            if (collider.GetComponent<Rigidbody>().velocity.magnitude > 10)
            {
                collider.GetComponent<Rigidbody>().velocity /= 10;
                Debug.Log(collider.GetComponent<Rigidbody>().velocity);
            }
            
        }
    }
}
