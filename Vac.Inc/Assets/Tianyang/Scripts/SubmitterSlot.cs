using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubmitterSlot : MonoBehaviour
{

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.gameObject);
        // The centrifuge only accepts tubes with tag "Ctube"
        if (collision.gameObject.tag == "Ctube" && this.transform.childCount <= 1)
        {
            Transform tube = collision.transform;
            Rigidbody tube_body = collision.gameObject.GetComponent<Rigidbody>();
            tube_body.isKinematic = true;
            tube.SetParent(gameObject.transform);
            tube.localPosition = Vector3.zero;
            tube.localRotation = Quaternion.identity;
        }
    }
}
