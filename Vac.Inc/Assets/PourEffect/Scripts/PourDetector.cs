using System.Collections;
using UnityEngine;

public class PourDetector : MonoBehaviour
{
    public int pourThreshold = 45;
    public Transform origin = null;
    public GameObject streamPrefab = null;

    private bool isPouring = false;
    private Stream currentStream = null;
    private LiquidFillManager liquid;

    private void Start()
    {
        liquid = GetComponent<LiquidFillManager>();
    }

    private void Update()
    {
        bool pourCheck = CalculatePourAngle() > pourThreshold;
        if (isPouring != pourCheck) {
            isPouring = pourCheck;
            if (isPouring && liquid.IsActive()) {
                StartPour();
            } else if (liquid.IsActive()) {
                EndPour();
            }
        }

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

    private Stream CreateStream()
    {
        GameObject streamObject = Instantiate(streamPrefab, origin.position, Quaternion.identity, transform);
        streamObject.GetComponent<Stream>().liquidColor = liquid.GetColor();
        streamObject.GetComponent<Stream>().virus = liquid.ContainsVirus();
        return streamObject.GetComponent<Stream>();
    }
}
