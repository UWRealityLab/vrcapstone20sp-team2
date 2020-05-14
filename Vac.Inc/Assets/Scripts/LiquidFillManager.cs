using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class LiquidFillManager : MonoBehaviour
{
    public GameObject liquid;
    public float fullFill = 0.372f;
    public float emptyFill = 0.5f;
    public float startingFill = 0.4f;
    public bool startWithLiquid = false;
    public bool liquidVolumeIsConstant = false;

    public Color liquidColor = new Color(0, 1.0f, 0, 1.0f);

    // The information of the virus contained in the liquid.
    public bool containsVirus = false;
    public float virusSim = 0.0f;
    public float virusRep = 0.0f;
    public float virusSev = 0.0f;
    // If the virus was put into any of the following:
    // centrifuge, bunsen heater, petri dish,
    // then it is considered as processed, which means
    // it will not be further improved by any upcoming processes.
    // 1f stands for fully processed; 0f stands for not processed.
    private float virusProcessRate = 0.0f;

    // Only liquid with embyros can be sent into the incubator.
    // WIP
    public bool containsEmbyro = false;

    // Only liquid with chemical can be heated.
    // WIP
    public bool containsChemical = false;

    private Material liquidMat;
    private float fill;

    private void Awake()
    {
        if (liquid != null) {
            liquid.SetActive(true);
        }
    }

    private void Start()
    {
        liquidMat = liquid.GetComponent<Renderer>().material;
        liquidMat.renderQueue = 3001;
        liquidMat.SetColor("_Tint", liquidColor);
        if (!startWithLiquid) {
            liquidMat.SetFloat("_FillAmount", emptyFill);
        } else if (liquidVolumeIsConstant) {
            liquidMat.SetFloat("_FillAmount", fullFill);
        } else {
            liquidMat.SetFloat("_FillAmount", startingFill);
        }
        liquid.SetActive(!startWithLiquid);
        liquid.SetActive(startWithLiquid);
        fill = liquidMat.GetFloat("_FillAmount");
    }

    // Increase the fill of this liquid, sourcing from the otherLiquid.
    // Note that the virus will only be passed over if and only if one
    // liquid contains virus. Otherwise, no virus is passed.
    public void IncreaseFill(LiquidFillManager otherLiquid)
    {
        Color other = otherLiquid.GetColor();
        if (liquidVolumeIsConstant) {
            return;
        }
        if (!this.containsVirus)
        {
            this.containsVirus = otherLiquid.containsVirus;
            this.virusProcessRate = otherLiquid.virusProcessRate;
            this.virusRep = otherLiquid.virusRep;
            this.virusSev = otherLiquid.virusSev;
            this.virusSim = otherLiquid.virusSim;
        }
        if (!liquid.activeSelf) {
            liquid.SetActive(true);
            liquidMat.SetColor("_Tint", other);
        }
        if (fill > fullFill) {
            liquidMat.SetFloat("_FillAmount", fill - 0.001f);
            fill -= 0.001f;
        }
        Color c = liquidMat.GetColor("_Tint");
        if (c.r < other.r) {
            c.r += 0.005f;
        } else if (c.r > other.r) {
            c.r -= 0.005f;
        }
        if (c.g < other.g) {
            c.g += 0.005f;
        } else if (c.g > other.g) {
            c.g -= 0.005f;
        }
        if (c.b < other.b) {
            c.b += 0.005f;
        } else if (c.g > other.b) {
            c.b -= 0.005f;
        }
        liquidMat.SetColor("_Tint", c);
    }

    // Heat the liquid with the bunsen heater.
    public void HeatLiquid()
    {
        if (liquidVolumeIsConstant) {
            return;
        }
        if (liquid.activeSelf) {
            Color c = liquidMat.GetColor("_Tint");
            if (c.r < 1) {
                c.r += 0.001f;
            }
            if (c.g > 0 && c.r > 0.997f) {
                c.g -= 0.001f;
            }
            liquidMat.SetColor("_Tint", c);
        }
        if (virusProcessRate < 1.0f && containsVirus)
        {
            virusProcessRate += 0.005f;
            // These params are subject to change.
            virusSim -= 0.003f * Random.value;
            virusSev -= 0.003f * Random.value;
            virusRep -= 0.003f * Random.value;
        }
    }

    // Spin the liquid in the centrifuge.
    public void SpinLiquid()
    {
        if (virusProcessRate < 1.0f && containsVirus)
        {
            virusProcessRate += 0.005f;
            // These params are subject to change.
            virusSim -= 0.002f * Random.value;
            virusSev -= 0.003f * Random.value;
            virusRep -= 0.004f * Random.value;
        }
    }

    public void IncubateLiquid()
    {
        if (virusProcessRate < 1.0f && containsEmbyro && containsVirus)
        {
            virusProcessRate += 0.005f;
            // These params are subject to change.
            virusSim -= 0.004f * Random.value;
            virusSev -= 0.004f * Random.value;
            virusRep -= 0.004f * Random.value;
        }
    }

    public Material GetLiquid()
    {
        return liquidMat;
    }

    public void DecreaseFill()
    {
        if (!liquidVolumeIsConstant && liquid.activeSelf && fill < emptyFill) {
            liquidMat.SetFloat("_FillAmount", fill + 0.001f);
            fill += 0.001f;
            if (IsEmpty()) {
                liquid.SetActive(false);
                this.containsVirus = false;
                this.virusProcessRate = 0.0f;
            }
        }
    }

    public bool ContainsVirus()
    {
        return containsVirus;
    }

    public bool IsActive()
    {
        return liquid.activeSelf;
    }

    public bool IsEmpty()
    {
        return fill >= emptyFill;
    }

    public Color GetColor()
    {
        return liquidMat.GetColor("_Tint");
    }
}
