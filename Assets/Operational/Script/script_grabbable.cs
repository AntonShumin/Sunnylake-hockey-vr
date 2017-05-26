namespace VRTK
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class script_grabbable : VRTK_InteractableObject
    {

        private GameObject grabbingController;

        //cashed 
        private float impact_velocity;

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


        private void OnCollisionEnter(Collision collision)
        {

            //set vars
            impact_velocity = 1f / 40f * collision.relativeVelocity.magnitude;
            impact_velocity = Mathf.Min(30f,impact_velocity);
            impact_velocity = Mathf.Max(0.05f,impact_velocity);
            Debug.Log("haptic strength " + impact_velocity);

            //haptic feedback
            if (grabbingController != null && IsGrabbed())
            {

                Vibrate(impact_velocity, 0.1f);
            }
            
        }

        private void Vibrate(float strength, float duration)
        {
            VRTK_SharedMethods.TriggerHapticPulse(VRTK_DeviceFinder.GetControllerIndex(grabbingController), strength, duration, 0.01f);
        }

        

    }
}
    
