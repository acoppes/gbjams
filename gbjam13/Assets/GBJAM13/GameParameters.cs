using UnityEngine;

namespace GBJAM13
{
    public static class GameParameters
    {
        public const int DefaultTotalJumps = 5;
        
        public static int totalJumps;
        public static GalaxyData galaxyData;
        
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static void OnBeforeSceneLoad()
        {
            GameParameters.totalJumps = DefaultTotalJumps;
            GameParameters.galaxyData = null;
        }
    }
}