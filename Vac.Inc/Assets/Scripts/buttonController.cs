using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Valve.VR.InteractionSystem.Sample
{
    public class buttonController : MonoBehaviour
    {
        public Transform origin = null;
        public GameObject streamPrefab = null;
        public GameObject liquid;
        public float fullFill = 0.372f;
        public float emptyFill = 0.5f;
        public float startingFill = 0.4f;
        public bool startEmpty = false;
        public bool liquidVolumeIsConstant = false;
        public Color liquidColor = new Color(0, 1.0f, 0, 1.0f);


        private Material liquidMat;
        private float fill;
        private Stream currentStream = null;

        private LiquidFillManager container = null;
        private Material containerLiquid = null;

        private void Start()
        {
            liquid.SetActive(true);
            liquidMat = liquid.GetComponent<Renderer>().material;
            liquidMat.SetColor("_Tint", liquidColor);
            if (!startEmpty) {
                liquidMat.SetFloat("_FillAmount", emptyFill);
            } else if (liquidVolumeIsConstant) {
                liquidMat.SetFloat("_FillAmount", fullFill);
            } else {
                liquidMat.SetFloat("_FillAmount", startingFill);
            }
            liquid.SetActive(!startEmpty);
            liquid.SetActive(startEmpty);
            fill = liquidMat.GetFloat("_FillAmount");
        }

        private void Update()
        {
            RaycastHit hit;
            Vector3 dir = -transform.up;
            dir = dir.normalized;
            Ray ray = new Ray(transform.position - dir * 0.5f, dir);
            Physics.Raycast(ray, out hit, 0.6f, 1 << 8);
            if (hit.collider && hit.collider.CompareTag("Beaker")) {
                container = hit.collider.gameObject.GetComponent<LiquidFillManager>();
                containerLiquid = container.getLiquid();
            } else {
                container = null;
            }
        }

        public void OnReleasePress(Hand hand)
        {
            if (!liquidVolumeIsConstant && liquid.activeSelf) {
                if (fill < emptyFill) {
                    liquidMat.SetFloat("_FillAmount", fill + 0.001f);
                    fill += 0.001f;
                    if (container != null) {
                        container.IncreaseFill(liquidMat.GetColor("_Tint"));
                    }
                    if (fill >= emptyFill) {
                        liquid.SetActive(false);
                    }
                }
            }
        }

        public void OnPumpPress(Hand hand)
        {
            if(container != null && !container.IsEmpty()) {
                container.decreaseFill();
                IncreaseFill(containerLiquid.GetColor("_Tint"));
            }
        }

        public void IncreaseFill(Color other)
        {
            if (liquidVolumeIsConstant) {
                return;
            }
            if (!liquid.activeSelf) {
                liquid.SetActive(true);
                liquidMat.SetColor("_Tint", other);
            }
            if (fill > fullFill) {
                liquidMat.SetFloat("_FillAmount", fill - 0.001f);
                fill -= 0.001f;
            }
            Color c = liquidMat.GetColor("_Tint");
            if (c.r < other.r) {
                c.r += 0.005f;
            } else if (c.r > other.r) {
                c.r -= 0.005f;
            }
            if (c.g < other.g) {
                c.g += 0.005f;
            } else if (c.g > other.g) {
                c.g -= 0.005f;
            }
            if (c.b < other.b) {
                c.b += 0.005f;
            } else if (c.g > other.b) {
                c.b -= 0.005f;
            }
            liquidMat.SetColor("_Tint", c);
        }

        private Stream CreateStream()
        {
            GameObject streamObject = Instantiate(streamPrefab, origin.position, Quaternion.identity, transform);
            streamObject.GetComponent<Stream>().liquidColor = liquidMat.GetColor("_Tint");
            return streamObject.GetComponent<Stream>();
        }
    }
}
