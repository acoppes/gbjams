using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Development
{
    public class MockupCameraShakeController : MonoBehaviour
    {
        public Animator cameraShakeAnimator;
        
        public InputAction shake1;
        public InputAction shake2;

        private void OnEnable()
        {
            shake1.Enable();
            shake2.Enable();
        }

        // Update is called once per frame
        void Update()
        {

            if (shake1.WasPressedThisFrame())
            {
                cameraShakeAnimator.SetTrigger("shake1");
            }

            if (shake2.WasPressedThisFrame())
            {
                cameraShakeAnimator.SetTrigger("shake2");
            }
        }
    }
}
