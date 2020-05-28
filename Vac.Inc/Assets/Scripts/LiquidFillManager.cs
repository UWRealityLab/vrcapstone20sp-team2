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
    public float incubateTime = 0.0f;
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
        if (!this.containsVirus && this.heatTime > 0 && otherLiquid.containsVirus) {
            this.containsVirus = true;
        } else if (!this.containsVirus) {
            this.containsVirus = otherLiquid.containsVirus;
            this.virusProcessRate = otherLiquid.virusProcessRate;
            this.virusRep = otherLiquid.virusRep;
            this.virusSev = otherLiquid.virusSev;
            this.virusSim = otherLiquid.virusSim;
        }
        if (this.heatTime == 0 && this.spinTime == 0 && this.incubateTime == 0) {
            if (otherLiquid.heatTime > 0) {
                this.heatTime = otherLiquid.heatTime;
            } else if (otherLiquid.spinTime > 0) {
                this.spinTime = otherLiquid.spinTime;
            } else if (otherLiquid.incubateTime > 0) {
                this.incubateTime = otherLiquid.incubateTime;
            }
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
    //.4 g .6 b
    // 1 r .25 g .6 b
    // Heat the liquid with the bunsen heater.
    public void HeatLiquid()
    {
        if (liquidVolumeIsConstant) {
            return;
        }
        if (liquid.activeSelf) {
            Color c = liquidMat.GetColor("_Tint");
            if (c.r + 0.0005f < 1) {
                c.r += 0.0005f;
            }
            if (c.g - 0.0005f > 0 && c.r > 0.997f) {
                c.g -= 0.0005f;
            }
            liquidMat.SetColor("_Tint", c);
        }
        if (spinTime == 0 && incubateTime == 0) {
            bool overheat = false;
            if (heatTime < 60.0f) {
                heatTime += Time.deltaTime;
            } else {
                overheat = true;
            }
            Color c = liquidMat.GetColor("_Tint");
            float gPercentDiff = Mathf.Abs(c.g - 0.25f) / ((c.g + 0.25f) / 2);
            float bPercentDiff = Mathf.Abs(c.b - 0.6f) / ((c.b + 0.6f) / 2);
            virusSev = overheat ? 0 : 1.0f / 30.0f * Mathf.Abs(heatTime - 30.0f);
            virusRep = overheat ? 0 : 1.0f - bPercentDiff / 2;
            virusSim = overheat ? 0 : 1.0f - gPercentDiff / 2;
            RoundVirus();
        }
    }

    // Spin the liquid in the centrifuge.
    // tubeCount is the number of test tubes inserted in the centrifuge
    // equalVolumes is true if all test tubes have the same volume
    public void SpinLiquid(int tubeCount, bool equalVolumes)
    {
        if (virusProcessRate < 1.0f && containsVirus && heatTime == 0 && incubateTime == 0)
        {
            if (spinTime < 59.5f) {
                spinTime += Time.deltaTime;
            }
            if (tubeCount == 5 && equalVolumes) {
                virusSim = 0.2432f * Mathf.Log(spinTime + 1);
                virusSev = -1.0f / 3600.0f * spinTime * spinTime + 1;
                virusRep = spinTime / 60.0f;
            } else if (tubeCount == 5) {
                virusSim = (0.2432f * Mathf.Log(spinTime + 1)) / 2;
                virusSev = (-1.0f / 3600.0f * spinTime * spinTime + 1) / 2;
                virusRep = (spinTime / 60.0f) / 2;
            } else if (equalVolumes) {
                virusSim = (0.2432f * Mathf.Log(spinTime + 1)) / 4;
                virusSev = (-1.0f / 3600.0f * spinTime * spinTime + 1) / 4;
                virusRep = (spinTime / 60.0f) / 4;
            } else {
                virusSim = (0.2432f * Mathf.Log(spinTime + 1)) / 8;
                virusSev = (-1.0f / 3600.0f * spinTime * spinTime + 1) / 8;
                virusRep = (spinTime / 60.0f) / 8;
            }
            RoundVirus();
        }
    }

    public void IncubateLiquid()
    {
      if (virusProcessRate < 1.0f && containsVirus && heatTime == 0 && spinTime == 0)
      {
          if (incubateTime < 59.5f) {
              incubateTime += Time.deltaTime;
          }
          virusSim = (0.2432f * Mathf.Log(incubateTime + 1)) / 8;
          virusSev = (-1.0f / 3600.0f * incubateTime * incubateTime + 1) / 8;
          virusRep = (incubateTime / 60.0f) / 8;
          RoundVirus();
      }
    }

    private void RoundVirus()
    {
        virusSim = Mathf.Round(virusSim * 100.0f) / 100.0f;
        virusSev = Mathf.Round(virusSev * 100.0f) / 100.0f;
        virusRep = Mathf.Round(virusRep * 100.0f) / 100.0f;
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
                this.virusSim = 0;
                this.virusSev = 0;
                this.virusRep = 0;
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
