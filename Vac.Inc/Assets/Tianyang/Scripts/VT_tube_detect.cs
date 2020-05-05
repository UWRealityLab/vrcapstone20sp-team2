using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VT_tube_detect : MonoBehaviour
{
    public tube_score lastTube = null;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Ctube")
        {
            lastTube = other.gameObject.GetComponent<tube_score>();
        }
    }
}
