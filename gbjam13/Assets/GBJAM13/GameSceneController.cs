using Game.Scenes;
using Gemserk.Leopotam.Ecs;
using UnityEngine;
using Object = UnityEngine.Object;

namespace GBJAM13
{
    public class GameSceneController : MonoBehaviour
    {
        public WorldReference worldReference;
        
        [EntityDefinition] 
        public Object shipDefinition;
        
        public void StartGame()
        {
            if (GameParameters.galaxyData == null)
            {
                GameParameters.totalJumps = GameParameters.DefaultTotalJumps;
                GameSceneLoader.LoadNextScene("MapGenerator");
                return;
            }
            
            if (GameParameters.nextNode == -1)
            {
                GameSceneLoader.LoadNextScene("Map");
                return;
            }


            GameParameters.currentColumn++;
            GameParameters.currentNode = GameParameters.nextNode;
        }
        
        // IF KEY UP/DOWN => swap selection

        private void Update()
        {

        }
    }
}