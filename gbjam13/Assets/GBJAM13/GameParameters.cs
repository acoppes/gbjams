using UnityEngine;

namespace GBJAM13
{
    public class SaveGame
    {
        public const int DefaultTotalJumps = 5;
        public const int JumpIncrementPerRun = 2;
        
        public int totalJumps = DefaultTotalJumps;

        public int currentColumn = 0;
        public int currentNode = 0;
        
        public GalaxyData galaxyData = null;
        
        public int nextNode = -1;

        public int[] resources = new int[5];
    }
    
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