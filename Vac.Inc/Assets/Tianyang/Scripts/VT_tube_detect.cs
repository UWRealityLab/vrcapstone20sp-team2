using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VT_tube_detect : MonoBehaviour
{
    public float similarity;
    public float reproducibility;
    public float severity;
    public LiquidFillManager liquid;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Ctube")
        {
            liquid = other.transform.Find("Liquid Holder").gameObject.GetComponent<LiquidFillManager>();
            if (liquid.containsVirus) {
                similarity = Mathf.Round(liquid.virusSim * 100.0f) / 100.0f;
                reproducibility = Mathf.Round(liquid.virusRep * 100.0f) / 100.0f;
                severity = Mathf.Round(liquid.virusSev * 100.0f) / 100.0f;
            }
        }
    }
}
