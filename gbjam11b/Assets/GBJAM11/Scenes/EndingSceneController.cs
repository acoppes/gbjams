using Game.Scenes;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GBJAM11.Scenes
{
    public class EndingSceneController : MonoBehaviour
    {
        public string nextScene;

        public void OnAnimationCompleted()
        {
            GameSceneLoader.LoadNextScene(nextScene);
        }
    }
}