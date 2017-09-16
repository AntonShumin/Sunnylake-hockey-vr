namespace VRTK
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class script_grabbable_basic : VRTK_InteractableObject
    {

        private GameObject grabbingController;

        // Use this for initialization
        void Start()
        {

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

    }

}

