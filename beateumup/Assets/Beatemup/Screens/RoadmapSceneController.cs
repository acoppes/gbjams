using System;
using Beatemup.MainMenu;
using Beatemup.Scenes;
using UnityEngine;

namespace Beatemup.Screens
{
    public class RoadmapSceneController : MonoBehaviour
    {
        public string nextSceneName = "MainMenu";

        public NotesScreen notes;
        
        private void Start()
        {
            notes.onClose += delegate
            {
                StartCoroutine(LoadingSceneController.LoadNextScene(nextSceneName));
            };
        }
    }
}