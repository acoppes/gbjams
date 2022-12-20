using System;
using System.Collections;
using Beatemup.Definitions;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

namespace Development
{
    public class MockupCameraShakeController : MonoBehaviour
    {
        public Animator cameraShakeAnimator;
        
        public InputAction shake1;
        public InputAction shake2;

        public CameraShake cameraShake1;
        public CameraShake cameraShake2;

        private Coroutine shakeCoroutine;

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
                if (shakeCoroutine != null)
                {
                    StopCoroutine(shakeCoroutine);
                }
                
                // cameraShakeAnimator.SetTrigger("shake1");
                shakeCoroutine = StartCoroutine(CameraShake.Shake(cameraShake1, cameraShakeAnimator.transform));
            }

            if (shake2.WasPressedThisFrame())
            {
                if (shakeCoroutine != null)
                {
                    StopCoroutine(shakeCoroutine);
                }
                
                // cameraShakeAnimator.SetTrigger("shake2");
                shakeCoroutine =  StartCoroutine(CameraShake.Shake(cameraShake2, cameraShakeAnimator.transform));
            }
        }
    }
}
