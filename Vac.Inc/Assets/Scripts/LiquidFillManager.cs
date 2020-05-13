using System.Collections;
using UnityEngine;

public class LiquidFillManager : MonoBehaviour
{
    public GameObject liquid;
    public float fullFill = 0.372f;
    public float emptyFill = 0.5f;
    public float startingFill = 0.4f;
    public bool startWithLiquid = false;
    public bool liquidVolumeIsConstant = false;
    public bool contatinsVirus = false;
    public Color liquidColor = new Color(0, 1.0f, 0, 1.0f);


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
        //liquid.SetActive(true);
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

    public void IncreaseFill(Color other, bool virus)
    {
        if (liquidVolumeIsConstant) {
            return;
        }
        contatinsVirus |= virus;
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
                contatinsVirus = false;
            }
        }
    }

    public bool ContainsVirus()
    {
        return contatinsVirus;
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
