using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tube_Score : MonoBehaviour
{
    public float similarity = 0.0f;
    public float reproducibility = 0.0f;
    public float severity = 0.0f;

    private LiquidFillManager liquid;

    private void Start()
    {
        liquid = GetComponent<LiquidFillManager>();
    }

    private void Update()
    {
        if (liquid.IsEmpty()) {
            similarity = 0;
            reproducibility = 0;
            severity = 0;
        } else {
            Color liquidColor = liquid.GetColor();
            similarity = 1.0f - Mathf.Abs(liquidColor.g - 0.25f);
            reproducibility = 1.0f - Mathf.Abs(liquidColor.b - 0.5f);
            severity = Mathf.Abs(liquidColor.r - 0.75f);
        }

    }
}
