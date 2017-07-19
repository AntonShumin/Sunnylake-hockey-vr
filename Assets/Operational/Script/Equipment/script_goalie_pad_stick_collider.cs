namespace VRTK {

    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class script_goalie_pad_stick_collider : MonoBehaviour
    {

        //set vars
        private script_grabbable m_script_controller;

        //cached
        private float c_pulse_strength;
        
        void Awake()
        {
            
        } 

        public void OnCollisionEnter(Collision collision)
        {
            Pulse(collision);
        }

        public void OnCollisionStay(Collision collision)
        {
            Pulse(collision);
        }

        private void Pulse(Collision collision)
        {
            c_pulse_strength = collision.relativeVelocity.magnitude / 25;
            c_pulse_strength = Mathf.Clamp(c_pulse_strength, 0.1f, 1f);
            Debug.Log("colliding force is " + c_pulse_strength);
            m_script_controller.Vibrate_pulse(0.1f);
        }
    }
}



