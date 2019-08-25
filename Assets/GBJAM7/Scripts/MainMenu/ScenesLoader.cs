using UnityEngine;
using UnityEngine.SceneManagement;

namespace GBJAM7.Scripts.MainMenu
{
    public class ScenesLoader
    {
        private const string GameSceneName = "GameScene";

        private static LevelDefinitionAsset pendingLevelLoad;
        
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

            var startLocation = GameObject.Find("~StartLocation");

            if (startLocation != null)
            {
                var gameController = GameObject.FindObjectOfType<GameController>();
                gameController.StartShowChangeTurnUI(startLocation.transform.position);
            }
            
        }
    }
}