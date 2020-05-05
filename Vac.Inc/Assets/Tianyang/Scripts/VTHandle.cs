using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Valve.VR.InteractionSystem
{
    public class VTHandle : MonoBehaviour
    {
        [EnumFlags] public Hand.AttachmentFlags attachmentFlags = Hand.AttachmentFlags.ParentToHand | Hand.AttachmentFlags.DetachFromOtherHand;
        GameObject handle;
        HingeJoint hj;
        private Hand curHand;

        void Start()
        {
            handle = transform.gameObject;
            hj = handle.transform.parent.GetComponent<HingeJoint>();
            curHand = null;
        }

        void Update()
        {
        //    if (curHand != null && Vector3.Distance(transform.position, handle.transform.position) > 0.2f)
        //    {
        //        detach(curHand);
        //    }
        }

        private void HandHoverUpdate(Hand hand)
        {
            GrabTypes startingGrabType = hand.GetGrabStarting();
            if (startingGrabType != GrabTypes.None)
            {
                hand.AttachObject(gameObject, startingGrabType, attachmentFlags);
                hj.useSpring = false;
                curHand = hand;
            }
        }

        private void HandAttachedUpdate(Hand hand)
        {
            if (hand.IsGrabEnding(gameObject))
            {
                detach(hand);
            }
        }

        private void detach(Hand hand)
        {
            hand.DetachObject(gameObject);
            transform.position = handle.transform.position;
            transform.rotation = handle.transform.rotation;
            transform.localScale = Vector3.one;
            Rigidbody handleRb = handle.GetComponent<Rigidbody>();
            handleRb.velocity = Vector3.zero;
            handleRb.angularVelocity = Vector3.zero;
            hj.useSpring = true;
            curHand = null;
        }
    }
}
