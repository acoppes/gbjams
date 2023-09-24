using System;
using Game.Scenes;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GBJAM11.Scenes
{
    public class IntroSceneController : MonoBehaviour
    {
        public string nextScene;

        public InputAction anyKeyPressed;

        private void Awake()
        {
            anyKeyPressed.performed += OnAnyKeyPressed;
        }

        private void OnEnable()
        {
            anyKeyPressed.Enable();
        }

        private void OnDisable()
        {
            anyKeyPressed.Disable();
        }

        private void OnAnyKeyPressed(InputAction.CallbackContext obj)
        {
            GameSceneLoader.LoadNextScene(nextScene);
        }


        public void OnAnimationCompleted()
        {
            GameSceneLoader.LoadNextScene(nextScene);
        }
    }
}
