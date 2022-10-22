using UnityEngine;
using UnityEngine.SceneManagement;

namespace GBJAM10.Changelogs
{
    public class ChangelogSceneController : MonoBehaviour
    {
        public ChangelogUI changelogUI;

        public string nextSceneName = "MainMenu";

        private void Start()
        {
            changelogUI.onClose += OnWindowClosed;
        }

        private void OnWindowClosed()
        {
            SceneManager.LoadScene(nextSceneName);
        }
    }
}
