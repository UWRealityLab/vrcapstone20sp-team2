using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class CanvasController : MonoBehaviour
{
  public GameObject canvas;

  public SteamVR_Action_Boolean actionOpen = SteamVR_Input.GetAction<SteamVR_Action_Boolean>("default", "OpenHelperMode");

  public SteamVR_Action_Boolean actionClose = SteamVR_Input.GetAction<SteamVR_Action_Boolean>("default", "CloseHelperMode");

  private SteamVR_Input_Sources LeftInputSource = SteamVR_Input_Sources.LeftHand;
  private SteamVR_Input_Sources RightInputSource = SteamVR_Input_Sources.RightHand;

    // Start is called before the first frame update
    void Start()
  {

  }

  // Update is called once per frame
  void Update()
  {
      // Detect if release/pump is pressed.
      bool OpenHelperMode = false;
      bool CloseHelperMode = false;
      OpenHelperMode = actionOpen.state && !SteamVR_Input.GetState("buggy", "liquidPump", LeftInputSource) && !SteamVR_Input.GetState("buggy", "liquidPump", RightInputSource);
      CloseHelperMode = actionClose.state && !SteamVR_Input.GetState("buggy", "liquidRelease", LeftInputSource) && !SteamVR_Input.GetState("buggy", "liquidRelease", RightInputSource);
      print("CloseHelperMode: " + CloseHelperMode);
      print("OpenHelperMode: " + OpenHelperMode);
      if (OpenHelperMode)
      {
          canvas.SetActive(true);
      } else if (CloseHelperMode)
      {
          canvas.SetActive(false);
      }
  }
}
