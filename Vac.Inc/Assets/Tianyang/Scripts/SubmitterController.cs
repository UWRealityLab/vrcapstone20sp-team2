using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

namespace Valve.VR.InteractionSystem
{
    public class SubmitterController : MonoBehaviour
    {
        public HoverButton hoverButton;
        public float loadTime = 5f;

        public GameObject display;
        public GameObject canvas;
        private float timeRemain;
        private Animator _animator;
        private bool capOpen;

        // Start is called before the first frame update
        void Start()
        {
            hoverButton.onButtonDown.AddListener(OnButtonDown);
            capOpen = true;
            _animator = GetComponent<Animator>();
            _animator.SetBool("capOpen", capOpen);
            timeRemain = loadTime;
        }

        // Update is called once per frame
        void Update()
        {
            timeRemain -= Time.deltaTime;
            if (capOpen)
            {
                timeRemain = loadTime;
            } else if (timeRemain > 0f) {
                canvas.SetActive(false);
            } else
            {
                Transform slot = transform.Find("slot");
                if (slot.childCount > 0)
                {
                    Transform tube = slot.GetChild(0);
                    LiquidFillManager liquid = tube.Find("Liquid Holder").gameObject.GetComponent<LiquidFillManager>();
                    showResult(liquid);
                }
            }
        }

        private void OnButtonDown(Hand hand)
        {
            capOpen = !capOpen;
            _animator.SetBool("capOpen", capOpen);
        }

        private void showResult(LiquidFillManager liquid)
        {
            canvas.SetActive(true);
            float similarity = liquid.virusSim;
            float reproducibility = liquid.virusRep;
            float severity = liquid.virusSev;
            float score = similarity + reproducibility + (1 - severity * 2);



            Text component = display.GetComponent<Text>();
            if (score >= 2.0f)
            {
                component.color = Color.green;
                component.text = "Your vaccine sample proved to be very effective against the virus. \n You saved the entire world!";
            }
            else if (score >= 1.5f)
            {
                component.color = Color.yellow;
                component.text = "Your vaccine is effective. \n You saved the most of the world!";
            }
            else
            {
                component.color = Color.red;
                component.text = "Your vaccine didn't have effects on the patients.\n Keep up the work!";
            }
        }
    }
}
