using Gemserk.Utilities;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace GBJAM13
{
    public class MapGeneratorSceneController : MonoBehaviour
    {
        public GalaxyGenerator.GalaxyGeneratorData data;

        public UnityEvent onMapGenerated;

        [ObjectType(typeof(IObjectList), filterString = "Database")]
        public Object eventsDb;
        
        [ObjectType(typeof(IObjectList), filterString = "Database")]
        public Object eventVariantsDb;
        
        [FormerlySerializedAs("mapElementsDatabase")] 
        [ObjectType(typeof(IObjectList), filterString = "Database")]
        public Object eventNamesDb;
        
        public void GenerateGalaxyMap()
        {
            var galaxyGenerator = new GalaxyGenerator();

            data.eventsDb = eventsDb.GetInterface<IObjectList>();
            data.eventsVariantsDb = eventVariantsDb.GetInterface<IObjectList>();
            data.eventNamesDb = eventNamesDb.GetInterface<IObjectList>();

            var saveGame = GameParameters.saveGame;
            
            saveGame.galaxyData = galaxyGenerator.GenerateGalaxy(data, saveGame.totalJumps);
            
            saveGame.currentColumn = 0;
            saveGame.currentNode = saveGame.galaxyData.startingRow;
            
            onMapGenerated.Invoke();
        }
        
        // TODO: not sure if generation should be slow or not, might be generated over time or something
    }
}