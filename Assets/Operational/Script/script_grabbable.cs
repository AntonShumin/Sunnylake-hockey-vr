namespace VRTK
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using ParticlePlayground;

    public class script_grabbable : VRTK_InteractableObject
    {

        private GameObject grabbingController;
        private ParticleSystem m_particles_block;
        private PlaygroundParticlesC m_particles_block_special;
        public script_cannon_settings.hot m_hot_object;
        public AudioSource m_Audio_source;

        //cashed 
        private float impact_velocity;
        private Vector3 m_position_impact;
        private Vector3 m_position_impact_target;
        private script_puck c_puck;
        

        void Start()
        {
            m_particles_block = GameObject.Find("particles_block").GetComponent<ParticleSystem>();
            m_particles_block_special = GameObject.Find("particles_block_special").GetComponent<PlaygroundParticlesC>();
            m_Audio_source = GetComponent<AudioSource>();
        }

        public override void Grabbed(GameObject grabbingObject)
        {
            base.Grabbed(grabbingObject);
            grabbingController = grabbingObject;
            Vibrate(0.2f, 0.1f);
        }

        public override void Ungrabbed(GameObject previousGrabbingObject)
        {
            base.Ungrabbed(previousGrabbingObject);
            grabbingController = null;
        }


        public void OnCollisionEnter(Collision collision)
        {

            //set vars
            impact_velocity = 1f / 40f * collision.relativeVelocity.magnitude;
            impact_velocity = Mathf.Min(30f,impact_velocity);
            impact_velocity = Mathf.Max(0.05f,impact_velocity);

            //vibrate
            Vibrate(impact_velocity, 0.1f);

            //particles
            Puck_Contact(collision, m_hot_object);

            
        }

        public void Puck_Contact(Collision collision, script_cannon_settings.hot blocking_object)
        {
            c_puck = collision.gameObject.GetComponent<script_puck>();
            //colliding object is a puck
            if ( c_puck != null )
            {

                //set vars
                m_position_impact_target = collision.gameObject.transform.position; //target position
                m_position_impact = collision.contacts[0].point;

                //if colision points are valid
                if (m_position_impact != null && m_particles_block != null)
                {
                    //hot collision
                    if (script_puck.m_hot == blocking_object && blocking_object != script_cannon_settings.hot.none)
                    {

                        //first collider
                        if(c_puck.m_hot_touched == false)
                        {
                            //haptic
                            Vibrate(1, 0.5f);

                            //particles
                            m_particles_block_special.transform.position = m_position_impact;
                            m_particles_block_special.transform.LookAt(m_position_impact_target);
                            m_particles_block_special.Emit(true);
                            c_puck.m_hot_touched = true;
                        }

                    }
                    //not hot collision but strong enough
                    else if (collision.relativeVelocity.magnitude > 10f)
                    {

                        //standard particles
                        m_particles_block.transform.position = m_position_impact;
                        m_particles_block.transform.LookAt(m_position_impact_target);
                        m_particles_block.Stop();
                        m_particles_block.Play();

                    }
                }
                   
            }
        }



        public void Vibrate(float strength, float duration)
        {
            if (grabbingController != null && IsGrabbed())
            {
                VRTK_SharedMethods.TriggerHapticPulse(VRTK_DeviceFinder.GetControllerIndex(grabbingController), strength, duration, 0.01f);
            }
            
        }


        public void Vibrate_pulse(float strength)
        {
            if (grabbingController != null && IsGrabbed())
            {
                VRTK_SharedMethods.TriggerHapticPulse(VRTK_DeviceFinder.GetControllerIndex(grabbingController), strength);
            }
        }

        public void GroundEnter()
        {
            Vibrate(0.1f, 0.05f);
        }




    }
}
    
