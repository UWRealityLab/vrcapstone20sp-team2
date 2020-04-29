using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiquidVolume : MonoBehaviour
{
    public float startFill = 0.4f;
    public float minFill = 0.372f;
    public float maxFill = 0.5f;

    private PourDetector pd;
    private Material liquid;
    private float fill;

    // Start is called before the first frame update
    void Start()
    {
        pd = GetComponentInParent<PourDetector>();
        liquid = GetComponent<Renderer>().sharedMaterial;
        fill = startFill;
        liquid.SetFloat("FillAmount", startFill);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (pd.isPouring) {
            Debug.Log(fill);
            if (fill < maxFill) {
                //Debug.Log(fill);
                liquid.SetFloat("_FillAmount", fill + 0.001f);
                fill += 0.001f;
            }
        }
    }
}
