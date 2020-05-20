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

    // Number of seconds this liquid has been heated
    public float heatTime = 0.0f;

    // Number of seconds this liquid has been spinning
    public float spinTime = 0.0f;

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
            this.heatTime = otherLiquid.heatTime;
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
        if (containsVirus) {
            virusRep = 1.0f - Mathf.Abs(c.b - 0.5f);
            virusSim = 1.0f - Mathf.Abs(c.g - 0.25f);
        }
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
        if (heatTime < 29.5f) {
            heatTime += Time.deltaTime;
        }
        if (containsVirus) {
            float heat = heatTime < 15.0f ? heatTime : 30.0f - heatTime;
            virusSev = heat / 30.0f;
        }
    }

    // Spin the liquid in the centrifuge.
    // tubeCount is the number of test tubes inserted in the centrifuge
    // equalVolumes is true if all test tubes have the same volume
    public void SpinLiquid(int tubeCount, bool equalVolumes)
    {
        if (virusProcessRate < 1.0f && containsVirus && heatTime == 0)
        {
            if (spinTime < 59.5f) {
                spinTime += Time.deltaTime;
            }
            if (tubeCount == 5 && equalVolumes) {
                virusSim = 24.32f * Mathf.Log(spinTime + 1);
                virusSev = 1.0f / 36.0f * spinTime * spinTime;
                virusRep = spinTime / 60.0f;
            } else if (tubeCount == 5) {
                virusSim = (24.32f * Mathf.Log(spinTime + 1)) / 2;
                virusSev = (1.0f / 36.0f * spinTime * spinTime) / 2;
                virusRep = (spinTime / 60.0f) / 2;
            } else if (equalVolumes) {
                virusSim = (24.32f * Mathf.Log(spinTime + 1)) / 4;
                virusSev = (1.0f / 36.0f * spinTime * spinTime) / 4;
                virusRep = (spinTime / 60.0f) / 4;
            } else {
                virusSim = (24.32f * Mathf.Log(spinTime + 1)) / 8;
                virusSev = (1.0f / 36.0f * spinTime * spinTime) / 8;
                virusRep = (spinTime / 60.0f) / 8;
            }
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
                this.heatTime = 0.0f;
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

    public float GetVolume()
    {
        return fill;
    }
}
