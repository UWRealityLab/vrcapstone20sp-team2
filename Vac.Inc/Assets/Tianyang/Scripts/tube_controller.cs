using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Valve.VR.InteractionSystem
{
    public class tube_controller : MonoBehaviour
    {
        void OnHandHoverBegin(Hand hand)
        {
            Rigidbody body = this.gameObject.GetComponent<Rigidbody>();
            body.isKinematic = false;
        }
    }
}