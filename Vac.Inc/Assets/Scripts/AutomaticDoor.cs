using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutomaticDoor : MonoBehaviour
{
    public GameObject leftDoor;
    public GameObject rightDoor;
    public Transform player;
    private Vector3 center;
    private bool open;

    // Start is called before the first frame update
    void Start()
    {
        center = transform.position;
        open = false;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 playerPosition = player.position;
        bool isClose = Vector3.Distance(center, playerPosition) < 2.0f;
        if (isClose) {
            leftDoor.SetActive(false);
            rightDoor.SetActive(false);
            open = true;
        } else {
            leftDoor.SetActive(true);
            rightDoor.SetActive(true);
            open = false;
        }
        GetComponent<BoxCollider>().enabled = !open;
    }
}
