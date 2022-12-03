using Beatemup.MainMenu;
using UnityEngine;

namespace Beatemup.Screens
{
    public class MainMenuController : MonoBehaviour
    {
        public string gameSceneName = "Game";
        
        public void StartGameWithPlayers(int number)
        {
            StartCoroutine(LoadingSceneController.LoadNextScene(gameSceneName));
        }
    }
}
