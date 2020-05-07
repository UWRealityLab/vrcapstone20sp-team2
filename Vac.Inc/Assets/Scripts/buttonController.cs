using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Valve.VR.InteractionSystem.Sample
{
  public class buttonController : MonoBehaviour
  {
    public Transform origin = null;
    public GameObject streamPrefab = null;
    public GameObject liquid;
    public float fullFill = 0.372f;
    public float emptyFill = 0.5f;
    public float startingFill = 0.4f;
    public bool startEmpty = false;
    public bool liquidVolumeIsConstant = false;
    public Color liquidColor = new Color(0, 1.0f, 0, 1.0f);


    private Material liquidMat;
    private float fill;
    private Stream currentStream = null;

    private PourDetector container = null;
    private Material containerLiquid = null;

    private void Start()
    {
        liquid.SetActive(true);
        liquidMat = liquid.GetComponent<Renderer>().material;
        liquidMat.SetColor("_Tint", liquidColor);
        if (!startEmpty) {
            liquidMat.SetFloat("_FillAmount", emptyFill);
        } else if (liquidVolumeIsConstant) {
            liquidMat.SetFloat("_FillAmount", fullFill);
        } else {
            liquidMat.SetFloat("_FillAmount", startingFill);
        }
        liquid.SetActive(!startEmpty);
        liquid.SetActive(startEmpty);
        fill = liquidMat.GetFloat("_FillAmount");
    }

    public void OnReleasePress(Hand hand)
    {
      if (liquid.activeSelf) {
          StartPour();
      }
      if (!liquidVolumeIsConstant && liquid.activeSelf) {
          if (fill < emptyFill) {
              liquidMat.SetFloat("_FillAmount", fill + 0.001f);
              fill += 0.001f;
              if (fill >= emptyFill) {
                  EndPour();
                  liquid.SetActive(false);
              }
          }
      }
      EndPour();
    }

    void OnTriggerEnter(Collider other) {
      //get the target container's liquid information to the pipette
      if(other.gameObject.tag == "Beaker") {
          container = other.gameObject.GetComponent<PourDetector>();
          containerLiquid = container.getLiquid();
      }
    }

    void OnTriggerExit(Collider other)
    {
      container = null;
      containerLiquid = null;
    }

    public void OnPumpPress(Hand hand)
    {
      //Debug.Log("ButtonPressed");
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

    private void StartPour()
    {
        currentStream = CreateStream();
        currentStream.Begin();
    }

    private void EndPour()
    {
        currentStream.End();
        //currentStream = null;
    }

    public void IncreaseFill(Color other)
    {
        if (liquidVolumeIsConstant) {
            return;
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

    private Stream CreateStream()
    {
        GameObject streamObject = Instantiate(streamPrefab, origin.position, Quaternion.identity, transform);
        streamObject.GetComponent<Stream>().liquidColor = liquidMat.GetColor("_Tint");
        return streamObject.GetComponent<Stream>();
    }
  }
}
