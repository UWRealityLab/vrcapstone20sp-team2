using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Valve.VR.InteractionSystem.Sample
{
  public class starter : MonoBehaviour
  {
      // Start is called before the first frame update
      public ParticleSystem particleObject;

      void Start()
      {
          particleObject.Stop();
      }
  }
}
