using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Beatemup.MainMenu
{
    public class LoadingSceneController : MonoBehaviour
    {
        public string nextSceneName = "Game";

        private void Start()
        {
            StartCoroutine(SequenceToNextScene());
        }

        private IEnumerator SequenceToNextScene()
        {
            yield return null;
            SceneManager.LoadScene(nextSceneName);
        }
    }
}