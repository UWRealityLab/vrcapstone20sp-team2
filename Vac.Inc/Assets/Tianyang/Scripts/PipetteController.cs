using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class PipetteController : MonoBehaviour
{
    public Transform origin = null;

    private LiquidFillManager liquid = null;

    private LiquidFillManager otherLiquid = null;

    private Interactable interactable;

    public SteamVR_Action_Boolean actionPump = SteamVR_Input.GetAction<SteamVR_Action_Boolean>("default", "LiquidPump");

    public SteamVR_Action_Boolean actionRelease = SteamVR_Input.GetAction<SteamVR_Action_Boolean>("default", "LiquidRelease");


    // Start is called before the first frame update
    void Start()
    {
        liquid = GetComponent<LiquidFillManager>();

        interactable = transform.parent.gameObject.GetComponent<Interactable>();
    }

    // Update is called once per frame
    void Update()
    {
        // Raycasting to detect if a container is below the pipette
        RaycastHit hit;
        Vector3 dir = -transform.up;
        dir = dir.normalized;
        Ray ray = new Ray(transform.position - dir * 0.5f, dir);
        Physics.Raycast(ray, out hit, 0.6f, 1 << 8);
        if (hit.collider && hit.collider.CompareTag("Beaker"))
        {
            otherLiquid = hit.collider.gameObject.GetComponent<LiquidFillManager>();
        }
        else
        {
            otherLiquid = null;
        }

        // Detect if release/pump is pressed.
        bool pump = false;
        bool release = false;
        if (interactable.attachedToHand)
        {
            SteamVR_Input_Sources hand = interactable.attachedToHand.handType;
            pump = actionPump.GetState(hand);
            release = actionRelease.GetState(hand);
        }

        if (pump)
        {
            PumpLiquid();
        } else if (release)
        {
            ReleaseLiquid();
        }
    }

    void ReleaseLiquid()
    {
        if (!liquid.liquidVolumeIsConstant)
        {
            liquid.DecreaseFill();
        }
        if (otherLiquid != null && !liquid.IsEmpty())
        {
            otherLiquid.IncreaseFill(liquid);
        }

    }

    void PumpLiquid()
    {
        if (otherLiquid != null && !otherLiquid.IsEmpty())
        {
            liquid.IncreaseFill(otherLiquid);
            otherLiquid.DecreaseFill();
        }
    }
}
