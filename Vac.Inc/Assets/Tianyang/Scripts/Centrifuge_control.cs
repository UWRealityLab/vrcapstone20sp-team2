using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Valve.VR.InteractionSystem
{
    public class Centrifuge_control : MonoBehaviour
    {

        public HoverButton hoverButton;
        public HoverButton startButton;
        public GameObject startButtonBox;
        public HoverButton stopButton;
        public Material startButtonActivateMat;

        private Animator _animator;
        private bool capOpen;
        private bool isRunning;

        private Material startButtonOrigMat;

        void Awake()
        {
            capOpen = false;
            isRunning = false;
            _animator = GetComponent<Animator>();
            startButtonOrigMat = startButtonBox.GetComponent<Renderer>().material;
        }

        // Start is called before the first frame update
        void Start()
        {
            hoverButton.onButtonDown.AddListener(OnButtonDown);
            startButton.onButtonDown.AddListener(StartButtonDown);
            stopButton.onButtonDown.AddListener(StopButtonDown);
        }

        void Update()
        {
            if (isRunning)
            {
                Transform slots = transform.Find("slots");
                foreach (Transform slot in slots)
                {
                    if (slot.childCount > 0)
                    {
                        Transform tube = slot.GetChild(0);
                        LiquidFillManager liquid = tube.Find("Liquid Holder").gameObject.GetComponent<LiquidFillManager>();
                        liquid.SpinLiquid();
                    }
                }
            }
        }

        private void OnButtonDown(Hand hand)
        {
            if (!isRunning)
            {
                capOpen = !capOpen;
                _animator.SetBool("capOpen", capOpen);
            }
        }

        private void StartButtonDown(Hand hand)
        {
            if (!capOpen)
            {
                isRunning = true;
                startButtonBox.GetComponent<Renderer>().material = startButtonActivateMat;
            }
        }

        private void StopButtonDown(Hand hand)
        {
            if (isRunning)
            {
                isRunning = false;
                startButtonBox.GetComponent<Renderer>().material = startButtonOrigMat;
            }
        }
    }
}
