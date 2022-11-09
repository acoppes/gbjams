using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Beatemup.Development
{
    public class DevTools : MonoBehaviour
    {
        public InputAction slowerTimeScale;
        public InputAction fasterTimeScale;

        public Text debugTimeScale;

        private Coroutine _showTextCoroutine;

        private void Start()
        {
            debugTimeScale.enabled = false;
        }

        private void OnEnable()
        {
            slowerTimeScale.Enable();
            fasterTimeScale.Enable();
        }

        // Update is called once per frame
        void Update()
        {
            var previousTimeScale = Time.timeScale;
            
            if (slowerTimeScale.WasReleasedThisFrame())
            {
                if (Time.timeScale <= 1.5f && Time.timeScale >= 0.1f)
                {
                    Time.timeScale -= 0.1f;
                } else if (Time.timeScale > 2.0f)
                {
                    Time.timeScale -= 1.0f;
                }
                else if (Time.timeScale > 1.5f)
                {
                    Time.timeScale -= 0.5f;
                }
            }

            if (fasterTimeScale.WasReleasedThisFrame())
            {
                if (Time.timeScale < 1)
                {
                    Time.timeScale += 0.1f;
                } else if (Time.timeScale >= 5.0f)
                {
                    Time.timeScale += 1.0f;
                }
                else if (Time.timeScale >= 1.0f)
                {
                    Time.timeScale += 0.5f;
                }
            }

            if (Time.timeScale < 0)
            {
                Time.timeScale = 0;
            }

            if (Math.Abs(previousTimeScale - Time.timeScale) > Mathf.Epsilon)
            {
                // update
                _showTextCoroutine = StartCoroutine(ShowNewTimeScale());
            }
        }

        private IEnumerator ShowNewTimeScale()
        {
            if (_showTextCoroutine != null)
            {
                StopCoroutine(_showTextCoroutine);
                _showTextCoroutine = null;
            }
            
            debugTimeScale.enabled = true;
            debugTimeScale.text = $"{Time.timeScale:0.0}x";
            yield return new WaitForSecondsRealtime(1.0f);

            debugTimeScale.enabled = false;
        }
    }
}
