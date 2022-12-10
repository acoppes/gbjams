using Beatemup.MainMenu;
using Beatemup.Scenes;
using UnityEngine;

namespace Beatemup.Screens
{
    public class MainMenuController : MonoBehaviour
    {
        public string gameSceneName = "Game";

        public string notesSceneName = "Notes";

        public TextAsset changelog;
        public TextAsset roadmap;
        
        public void StartGameWithPlayers(int players)
        {
            GameSceneController.players = players;
            StartCoroutine(LoadingSceneController.LoadNextScene(gameSceneName));
        }

        public void ShowChangelog()
        {
            NotesSceneController.notesText = changelog;
            StartCoroutine(LoadingSceneController.LoadNextScene(notesSceneName));
        }
        
        public void ShowRoadmap()
        {
            NotesSceneController.notesText = roadmap;
            StartCoroutine(LoadingSceneController.LoadNextScene(notesSceneName));
        }
    }
}
