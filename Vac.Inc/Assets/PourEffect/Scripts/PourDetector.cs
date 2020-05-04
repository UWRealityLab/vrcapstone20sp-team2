using System.Collections;
using UnityEngine;

public class PourDetector : MonoBehaviour
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
    }

    private void Update()
    {
        bool pourCheck = CalculatePourAngle() > pourThreshold;
        if (isPouring != pourCheck) {
            isPouring = pourCheck;
            if (isPouring && liquid.activeSelf) {
                StartPour();
            } else if (liquid.activeSelf) {
                EndPour();
            }
        }

        if (!fixedBeaker && isPouring && liquid.activeSelf) {
            if (fill < emptyFill) {
                liquidMat.SetFloat("_FillAmount", fill + 0.001f);
                fill += 0.001f;
                if (fill >= emptyFill) {
                    EndPour();
                    liquid.SetActive(false);
                }
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
        if (fixedBeaker) {
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

    public void HeatLiquid()
    {
        if (fixedBeaker) {
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

    private float CalculatePourAngle()
    {
        float f = Vector3.Angle(transform.up, Vector3.up);
        return f;
    }

    private Stream CreateStream()
    {
        GameObject streamObject = Instantiate(streamPrefab, origin.position, Quaternion.identity, transform);
        streamObject.GetComponent<Stream>().liquidColor = liquidMat.GetColor("_Tint");
        return streamObject.GetComponent<Stream>();
    }
}