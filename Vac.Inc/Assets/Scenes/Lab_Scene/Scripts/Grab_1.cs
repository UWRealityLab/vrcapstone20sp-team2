using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Valve.VR.InteractionSystem
{
  public class Grab_1 : MonoBehaviour
  {
      [EnumFlags]
      public Hand.AttachmentFlags attachmentFlags = Hand.AttachmentFlags.ParentToHand | Hand.AttachmentFlags.DetachFromOtherHand;

      private void HandHoverUpdate(Hand hand)
      {
        GrabTypes startingGrabType = hand.GetGrabStarting();
        if (startingGrabType != GrabTypes.None)
        {
          hand.AttachObject(gameObject, startingGrabType, attachmentFlags);
        }

      }

      private void HandAttachedUpdate(Hand hand)
      {
        if (hand.IsGrabEnding(gameObject))
        {
          hand.DetachObject(gameObject);
        }
      }
      // Start is called before the first frame update
      void Start()
      {

      }

      // Update is called once per frame
      void Update()
      {

      }
  }
}
