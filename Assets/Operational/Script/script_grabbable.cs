namespace VRTK
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class script_grabbable : VRTK_InteractableObject
    {

        private GameObject grabbingController;

        public override void Grabbed(GameObject grabbingObject)
        {
            base.Grabbed(grabbingObject);
            grabbingController = grabbingObject;
        }

        public override void Ungrabbed(GameObject previousGrabbingObject)
        {
            base.Ungrabbed(previousGrabbingObject);
            grabbingController = null;
        }


        private void OnCollisionEnter(Collision collision)
        {
            if (grabbingController != null && IsGrabbed())
            {

                var hapticStrength = 1;
                VRTK_SharedMethods.TriggerHapticPulse(VRTK_DeviceFinder.GetControllerIndex(grabbingController), hapticStrength, 0.5f, 0.01f);
            }
            
        }

        

    }
}
    
