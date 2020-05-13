using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Valve.VR.InteractionSystem.Sample
{
  public class pumpController : MonoBehaviour
  {
    public Transform origin = null;
    public GameObject streamPrefab = null;
    public GameObject liquid;
    public float fullFill = 0.1f;
    public float emptyFill = 0.5f;
    public bool startEmpty = false;
    public bool liquidVolumeIsConstant = false;
    public Color liquidColor = new Color(0, 1.0f, 0, 1.0f);


    private Material liquidMat;
    private float fill;
    private Stream currentStream = null;

    private LiquidFillManager container = null;
    private Material containerLiquid = null;

    private void Start()
    {
      liquid.SetActive(true);
      liquidMat = liquid.GetComponent<Renderer>().material;
      liquidMat.SetColor("_Tint", liquidColor);
      fill = liquidMat.GetFloat("_FillAmount");
    }

    public void IncreaseFill(Color other)
    {
        fill = liquidMat.GetFloat("_FillAmount");
        Debug.Log("init: " + fill);
        if (liquidVolumeIsConstant) {
            return;
        }
        if (!liquid.activeSelf) {
            liquid.SetActive(true);
            liquidMat.SetColor("_Tint", other);
        }
        //Debug.Log("filling1");
        //Debug.Log(fill);
        //Debug.Log(fullFill);
        if (fill > fullFill) {
            Debug.Log("filling2");
            liquidMat.SetFloat("_FillAmount", fill - 0.001f);
            fill -= 0.001f;
            Debug.Log("filling3");
        }
        Color c = liquidMat.GetColor("_Tint");
        Debug.Log("gotColor");
        /*
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
        */
        liquidMat.SetColor("_Tint", c);
    }

    void OnTriggerEnter(Collider other) {
      //get the target container's liquid information to the pipette
      if(other.gameObject.tag == "Beaker") {
          container = other.gameObject.GetComponent<LiquidFillManager>();
          containerLiquid = container.getLiquid();
      }
    }

    void OnTriggerExit(Collider other)
    {
      container = null;
      containerLiquid = null;
    }

    public void OnButtonPress(Hand hand)
    {
      Debug.Log("ButtonPressed");
      if(container != null) {
        Debug.Log("beakerFound");
        container.decreaseFill();
        Debug.Log(containerLiquid.GetColor("_Tint"));
        Debug.Log("decreased");
        if(container.liquid.activeSelf) {
          IncreaseFill(containerLiquid.GetColor("_Tint"));
          Debug.Log("inceased");
        }
      }
    }
}
}
