using UnityEngine;
using System.Collections;

public class ignoreCollsionWithPip : MonoBehaviour
{
    public GameObject other;

    void Start()
    {
        Physics.IgnoreCollision(other.GetComponent<Collider>(), GetComponent<Collider>());
        Debug.Log("started collision");
    }


   void OnCollisionEnter (Collision collision) {
      Debug.Log("have collision");
      if (collision.gameObject.tag == "pip")
      {
        Debug.Log("have collision tagged");
        Physics.IgnoreCollision(collision.collider, GetComponent<Collider>());
      }
    }
}
