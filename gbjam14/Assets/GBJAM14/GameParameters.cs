using UnityEngine;

namespace GBJAM14
{
    public static class GameParameters
    {
        public static SaveGame saveGame;
        
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static void OnBeforeSceneLoad()
        {
            saveGame = null;
        }
    }
}