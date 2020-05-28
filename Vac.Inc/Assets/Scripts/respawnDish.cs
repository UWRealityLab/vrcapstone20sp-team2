using System.Collections;
using System.Collections.Generic;
using UnityEngine;

  public class respawnDish : MonoBehaviour
  {
    // The object to clone after it is being picked up.
    public GameObject dishPrefab = null;
    // The initial rotation of the new clone.
    // (0, 0, 0, 1) to be default.
    public Quaternion spawnQuaternion = Quaternion.identity;

    private bool canRespawn = false;
    private Vector3 origin;
    // Start is called before the first frame update
    void Start()
    {
      canRespawn = true;
      GetComponent<Rigidbody>().isKinematic = false;
      origin = transform.position;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void respawn() {
      StartCoroutine(respawnHelper());
    }

    public IEnumerator respawnHelper()
    {
      yield return new WaitForSeconds(1.0f);
      if(canRespawn) {
        Instantiate(dishPrefab, origin, spawnQuaternion);
        canRespawn = false;
      }
    }

  }
