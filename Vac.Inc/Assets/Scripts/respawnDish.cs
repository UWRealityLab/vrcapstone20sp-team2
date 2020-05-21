using System.Collections;
using System.Collections.Generic;
using UnityEngine;

  public class respawnDish : MonoBehaviour
  {
    public GameObject dishPrefab = null;
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
        print(canRespawn);
        Instantiate(dishPrefab, origin, Quaternion.identity);
        canRespawn = false;
      }
    }

  }
