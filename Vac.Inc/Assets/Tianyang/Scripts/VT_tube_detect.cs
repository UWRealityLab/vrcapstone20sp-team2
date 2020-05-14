using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VT_tube_detect : MonoBehaviour
{
    public float similarity;
    public float reproducibility;
    public float severity;
    public Tube_Score lastTube;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Ctube")
        {
            lastTube = other.gameObject.GetComponent<Tube_Score>();
            similarity = Mathf.Round(lastTube.similarity * 100.0f) / 100.0f;
            reproducibility = Mathf.Round(lastTube.reproducibility * 100.0f) / 100.0f;
            severity = Mathf.Round(lastTube.severity * 100.0f) / 100.0f;
        }
    }
}
