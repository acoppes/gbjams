using Beatemup.MainMenu;
using Beatemup.Scenes;
using UnityEngine;

namespace Beatemup.Screens
{
    public class MainMenuController : MonoBehaviour
    {
        public string gameSceneName = "Game";
        
        public void StartGameWithPlayers(int players)
        {
            GameSceneController.players = players;
            StartCoroutine(LoadingSceneController.LoadNextScene(gameSceneName));
        }
    }
}
