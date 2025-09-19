using Gemserk.Utilities;
using UnityEngine;
using UnityEngine.Events;

namespace GBJAM13
{
    public class MapGeneratorSceneController : MonoBehaviour
    {
        public GalaxyGenerator.GalaxyGeneratorData data;

        public UnityEvent onMapGenerated;

        [ObjectType(typeof(IObjectList), filterString = "Database")]
        public Object mapElementsDatabase;
        
        public void GenerateGalaxyMap()
        {
            var galaxyGenerator = new GalaxyGenerator
            {
                mapElementsDatabase = mapElementsDatabase.GetInterface<IObjectList>()
            };
            
            GameParameters.galaxyData = galaxyGenerator.GenerateGalaxy(data, GameParameters.totalJumps);
            
            GameParameters.currentColumn = 0;
            GameParameters.currentNode = GameParameters.galaxyData.startingRow;
            
            onMapGenerated.Invoke();
        }
        
        // TODO: not sure if generation should be slow or not, might be generated over time or something
    }
}