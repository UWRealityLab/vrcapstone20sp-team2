using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Valve.VR.InteractionSystem
{
    public class Centrifuge_control : MonoBehaviour
    {

        public HoverButton hoverButton;

        private Animator _animator;

        void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        // Start is called before the first frame update
        void Start()
        {
            hoverButton.onButtonDown.AddListener(OnButtonDown);
        }

        private void OnButtonDown(Hand hand)
        {
            _animator.SetBool("capOpen", !_animator.GetBool("capOpen"));
        }
    }
}
