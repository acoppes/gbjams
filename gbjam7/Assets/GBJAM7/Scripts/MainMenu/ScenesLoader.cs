using UnityEngine;
using UnityEngine.SceneManagement;

namespace GBJAM7.Scripts.MainMenu
{
    public class ScenesLoader
    {
        private const string GameSceneName = "GameScene";
        private const string MainMenuSceneName = "MainMenuScene";

        private static LevelDefinitionAsset pendingLevelLoad;

        public static void ReturnToMainMenu()
        {
            SceneManager.LoadScene(MainMenuSceneName, LoadSceneMode.Single);
        }
        
        public static void LoadLevel(LevelDefinitionAsset level)
        {
            pendingLevelLoad = level;
            SceneManager.sceneLoaded += OnGameSceneLoaded;
            SceneManager.LoadScene(GameSceneName, LoadSceneMode.Single);
        }

        private static void OnGameSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            // do stuff
            GameObject.Instantiate(pendingLevelLoad.levelPrefab);
            GameObject.Instantiate(pendingLevelLoad.balancePrefab);

            SceneManager.sceneLoaded -= OnGameSceneLoaded;
        }
    }
}