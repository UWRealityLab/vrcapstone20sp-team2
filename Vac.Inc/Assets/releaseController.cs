using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Valve.VR.InteractionSystem.Sample
{
  public class releaseController : MonoBehaviour
  {
    public int pourThreshold = 45;
    public Transform origin = null;
    public GameObject streamPrefab = null;
    public GameObject liquid;
    public float fullFill = 0.372f;
    public float emptyFill = 0.5f;
    public bool liquidActive = false;
    public bool fixedBeaker = false;
    public Color liquidColor = new Color(0, 1.0f, 0, 1.0f);


    private Material liquidMat;
    private float fill;
    private bool isPouring = false;
    private Stream currentStream = null;


    private void Start()
    {
        liquid.SetActive(true);
        liquidMat = liquid.GetComponent<Renderer>().material;
        liquidMat.SetColor("_Tint", liquidColor);
        if (!liquidActive) {
            liquidMat.SetFloat("_FillAmount", emptyFill);
        } else if (fixedBeaker) {
            liquidMat.SetFloat("_FillAmount", fullFill);
        }
        liquid.SetActive(!liquidActive);
        liquid.SetActive(liquidActive);
        fill = liquidMat.GetFloat("_FillAmount");
        Debug.Log("start completed");
    }


    public void OnPress(Hand hand)
    {
      if (liquid.activeSelf) {
          StartPour();
      }
      if (!fixedBeaker && liquid.activeSelf) {
          if (fill < emptyFill) {
              liquidMat.SetFloat("_FillAmount", fill + 0.0005f);
              fill += 0.0005f;
              if (fill >= emptyFill) {
                  EndPour();
                  liquid.SetActive(false);
              }
          }
      }
      EndPour();
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

    private Stream CreateStream()
    {
        GameObject streamObject = Instantiate(streamPrefab, origin.position, Quaternion.identity, transform);
        streamObject.GetComponent<Stream>().liquidColor = liquidMat.GetColor("_Tint");
        return streamObject.GetComponent<Stream>();
    }
  }
}
