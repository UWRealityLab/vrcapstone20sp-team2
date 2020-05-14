using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Valve.VR.InteractionSystem.Sample
{
  public class tube_generator : MonoBehaviour
  {
      public GameObject tubePrefab = null;
      public GameObject parent;

      private int positionCounter = 0;
      // Start is called before the first frame update
      void Start()
      {
        positionCounter = 0;
      }

      // Update is called once per frame
      void Update()
      {

      }

      protected virtual void OnHandHoverBegin(Hand hand)
      {
        if(positionCounter == 20) {
          positionCounter = 0;
        }
        Instantiate(tubePrefab, parent.transform.GetChild(positionCounter).gameObject.transform.position, Quaternion.identity);
        positionCounter = positionCounter + 1;
      }

      protected virtual void OnHandHoverEnd(Hand hand)
      {
        Debug.Log("xxxxxxxx");
      }
  }
}
