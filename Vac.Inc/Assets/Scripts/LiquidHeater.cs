using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiquidHeater : MonoBehaviour
{
    private ParticleSystem fire;

    // Start is called before the first frame update
    void Start()
    {
        fire = GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        if (fire.isPlaying) {
            EmitHeat();
        }
    }

    void EmitHeat()
    {
        RaycastHit hit;
        Ray ray = new Ray(transform.position, Vector3.up);
        Physics.Raycast(ray, out hit, 0.15f);
        if (hit.collider && hit.collider.CompareTag("Beaker")) {
            LiquidFillManager beaker = hit.collider.gameObject.GetComponent<LiquidFillManager>();
            beaker.HeatLiquid();
        }
    }
}
