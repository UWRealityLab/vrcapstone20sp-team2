﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tube_slot_controller : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
        // The centrifuge only accepts tubes with tag "Ctube"
        if (collision.gameObject.tag == "Ctube" && this.transform.childCount == 0)
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
