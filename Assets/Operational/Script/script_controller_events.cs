namespace VRTK.Examples
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class script_controller_events : MonoBehaviour
    {

        // Use this for initialization
        void Start()
        {

            if (GetComponent<VRTK_ControllerEvents>() == null)
            {
                Debug.LogError("VRTK_ControllerEvents_ListenerExample is required to be attached to a Controller that has the VRTK_ControllerEvents script attached to it");
                return;
            }

            GetComponent<VRTK_ControllerEvents>().TouchpadTouchStart += new ControllerInteractionEventHandler(DoTouchpadTouchStart);
            GetComponent<VRTK_ControllerEvents>().TouchpadTouchEnd += new ControllerInteractionEventHandler(DoTouchpadTouchEnd);

        }

        private void DoTouchpadTouchStart(object sender, ControllerInteractionEventArgs e)
        {
            GameObject puck = GameObject.Find("prefab_ball");
            puck.GetComponent<Rigidbody>().velocity = Vector3.zero;
            puck.transform.position = transform.position;

        }

        private void DoTouchpadTouchEnd(object sender, ControllerInteractionEventArgs e)
        {
            
        }

    }
}


