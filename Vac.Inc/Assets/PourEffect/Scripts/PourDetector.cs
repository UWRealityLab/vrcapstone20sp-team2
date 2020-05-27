using System;
using System.Collections;
using UnityEngine;

public class PourDetector : MonoBehaviour
{
    public Transform origin = null;
    public GameObject streamPrefab = null;
    // Set to false if this liquid cannot be poured out.
    public bool pourable = true;

    private bool isPouring = false;
    private Stream currentStream = null;
    private LiquidFillManager liquid;
    private float fullFill;
    private float emptyFill;
    

    private void Start()
    {
        liquid = GetComponent<LiquidFillManager>();
        fullFill = liquid.fullFill;
        emptyFill = liquid.emptyFill;
    }

    private void Update()
    {
        IsPouring();
        EmptiedOut();
    }

    private void IsPouring()
    {
        float pourAngle = CalculatePourAngle();
        float pourThreshold = CalculatePourThreshold();
        bool pourCheck = pourAngle > pourThreshold;
        if (isPouring != pourCheck) {
            isPouring = pourCheck;
            if (isPouring && liquid.IsActive()) {
                Debug.Log("Angle: " + pourAngle);
                Debug.Log("Threshold: " + pourThreshold);
                StartPour();
            } else if (liquid.IsActive()) {
                EndPour();
            }
        }
    }

    private void EmptiedOut()
    {
        if (isPouring) {
            liquid.DecreaseFill();
            if (liquid.IsEmpty()) {
                EndPour();
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
        if (currentStream != null) {
            currentStream.End();
            currentStream = null;
        }
    }

    private float CalculatePourAngle()
    {
        float f = Vector3.Angle(transform.up, Vector3.up);
        return f;
    }

    private float CalculatePourThreshold()
    {
        if (pourable)
        {
            float fill = liquid.GetVolume();
            float percentEmpty = Math.Max(0.0f, (fill - fullFill)) / (emptyFill - fullFill);
            return percentEmpty * 50 + 40;
        } else
        {
            return 1000;
        }

    }

    private Stream CreateStream()
    {
        GameObject streamObject = Instantiate(streamPrefab, origin.position, Quaternion.identity, transform);
        streamObject.GetComponent<Stream>().sourceLiquid = this.liquid;
        return streamObject.GetComponent<Stream>();
    }
}
