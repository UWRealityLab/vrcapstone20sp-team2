using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class destroyer : MonoBehaviour
{
    public void OnTriggerEnter(Collider other)
    {
      if(other.gameObject.tag == "Ctube"){
        Debug.Log("dedededede");
        Destroy(other.gameObject);
      }
    }
}
