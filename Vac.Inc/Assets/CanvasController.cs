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
      OpenHelperMode = actionOpen.state;
      CloseHelperMode = actionClose.state;
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
