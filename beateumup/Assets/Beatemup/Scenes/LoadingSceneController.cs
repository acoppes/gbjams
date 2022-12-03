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
            StartCoroutine(LoadNextScene(nextSceneName));
        }

        public static IEnumerator LoadNextScene(string name)
        {
            yield return null;
            SceneManager.LoadScene(name);
        }
    }
}