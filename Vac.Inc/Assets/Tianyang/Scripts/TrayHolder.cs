using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Valve.VR.InteractionSystem
{
    public class TrayHolder : MonoBehaviour
    {
        private float threshold = 20f;
        private Vector3 holdDirection = Vector3.down;

        void OnCollisionEnter(Collision other)
        {
            if (!other.gameObject.name.Contains("HandCollider") &&
                other.gameObject.GetComponent<Interactable>() != null)
            {
                Debug.Log("first pass" + other.gameObject);
                Vector3 contactNormal = other.GetContact(0).normal;
                if (Vector3.Angle(contactNormal, holdDirection) <= threshold)
                {
                    Debug.Log("second pass" + other.gameObject);
                    other.gameObject.transform.SetParent(this.transform);
                }
            }

        }

    }
}
