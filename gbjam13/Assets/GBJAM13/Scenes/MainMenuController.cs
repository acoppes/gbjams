using UnityEngine;
using UnityEngine.Events;

namespace GBJAM13.Scenes
{
    public class MainMenuController : MonoBehaviour
    {
        public int startingTotalJumps;

        public UnityEvent onGameStarted;

        public void StartGame()
        {
            GameParameters.galaxyData = null;
            GameParameters.totalJumps = startingTotalJumps;
            onGameStarted.Invoke();
        }
    }
}
