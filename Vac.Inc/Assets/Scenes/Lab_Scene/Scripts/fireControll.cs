using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Valve.VR.InteractionSystem.Sample
{
  public class fireControll : MonoBehaviour
  {
    public ParticleSystem particleObject;
    private bool isPlaying = false;

    public void OnPress(Hand hand)
    {
        Debug.Log("Pressed");
        if (isPlaying) {
          particleObject.Stop();
          isPlaying = false;
          Debug.Log("Played");
        } else {
          particleObject.Play();
          isPlaying = true;
          Debug.Log("Stopped");
        }
    }
  }
}
