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
                // cameraShakeAnimator.SetTrigger("shake1");
                shakeCoroutine = StartCoroutine(Shake(cameraShake1, cameraShakeAnimator.transform));
            }

            if (shake2.WasPressedThisFrame())
            {
                // cameraShakeAnimator.SetTrigger("shake2");
                shakeCoroutine = StartCoroutine(Shake(cameraShake2, cameraShakeAnimator.transform));
            }
        }
        
        public IEnumerator Shake(CameraShake cameraShake, Transform t)
        {
            // var orignalPosition = Vector3.zero;

            if (shakeCoroutine != null)
            {
                t.position = Vector3.zero;
                StopCoroutine(shakeCoroutine);
            }

            var elapsed = 0f;

            var x = cameraShake.magnitude.x * (Random.Range(-1f, 1f) > 0 ? 1f : -1f);
            var y = cameraShake.magnitude.y * (Random.Range(-1f, 1f) > 0 ? 1f : -1f);
            
            while (elapsed < cameraShake.duration)
            {
                // float x = Random.Range(-1f, 1f) * magnitude.x;
                // float y = Random.Range(-1f, 1f) * magnitude.y;

                t.position = new Vector3(x, y, 0);
                // elapsed += Time.deltaTime;
                yield return new WaitForSeconds(1f/15f);
                elapsed += 1f / 15f;

                var time = elapsed / cameraShake.duration;
                var decayValue = cameraShake.decay.Evaluate(time);

                x *= -decayValue;
                y *= -decayValue;
            }
            
            t.position = Vector3.zero;
            //t.position = orignalPosition;
        }
    }
}
