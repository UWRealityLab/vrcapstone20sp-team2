using System.Collections;
using UnityEngine;

public class PourDetector : MonoBehaviour
{
    public int pourThreshold = 45;
    public Transform origin = null;
    public GameObject streamPrefab = null;
    public GameObject liquid;
    public float minFill = 0.372f;
    public float maxFill = 0.5f;
    public bool liquidActive = false;

    private Material liquidMat;
    private float fill;
    private bool isPouring = false;
    private Stream currentStream = null;

    private void Awake()
    {
        liquid.SetActive(true);
        liquidMat = liquid.GetComponent<Renderer>().material;
        if (!liquidActive) {
            liquidMat.SetFloat("_FillAmount", maxFill);
        }
        liquid.SetActive(!liquidActive);
        liquid.SetActive(liquidActive);
    }

    private void Start()
    {
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

        if (isPouring && liquid.activeSelf) {
            if (fill < maxFill) {
                liquidMat.SetFloat("_FillAmount", fill + 0.001f);
                fill += 0.001f;
                if (fill >= maxFill) {
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

    public void IncreaseFill()
    {
        if (fill > minFill) {
            liquidMat.SetFloat("_FillAmount", fill - 0.001f);
            fill -= 0.001f;
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
        return streamObject.GetComponent<Stream>();
    }
}