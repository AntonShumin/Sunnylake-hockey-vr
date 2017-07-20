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

        //cashed 
        private float impact_velocity;
        private Vector3 m_position_impact;
        private Vector3 m_position_impact_target;

        void Start()
        {
            m_particles_block = GameObject.Find("particles_block").GetComponent<ParticleSystem>();
            m_particles_block_special = GameObject.Find("particles_block_special").GetComponent<PlaygroundParticlesC>();
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
            //Debug.Log("haptic strength " + impact_velocity);
            Vibrate(impact_velocity, 0.1f);

            //particles
            m_position_impact_target = collision.gameObject.transform.position; //target position
            m_position_impact = collision.contacts[0].point;
            if (m_position_impact != null && m_particles_block != null && impact_velocity > 0.5f)
            {
                m_particles_block.transform.position = m_position_impact;
                m_particles_block.transform.LookAt(m_position_impact_target);
                m_particles_block.Stop();
                m_particles_block.Play();

                //special
                m_particles_block_special.transform.position = m_position_impact;
                m_particles_block_special.transform.LookAt(m_position_impact_target);
                m_particles_block_special.Emit();
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
    
