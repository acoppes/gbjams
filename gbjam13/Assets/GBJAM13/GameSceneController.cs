using Game.Scenes;
using GBJAM13.Components;
using Gemserk.Leopotam.Ecs;
using MyBox;
using UnityEngine;

namespace GBJAM13
{
    public class GameSceneController : MonoBehaviour
    {
        public WorldReference worldReference;

        public EntityPrefabInstance elementInstance;
        
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

            var world = worldReference.GetReference(gameObject);
            var elementEntity = world.CreateEntity(elementInstance.entityDefinition);

            elementEntity.Get<PositionComponent>().value = elementInstance.transform.position;
            
            ref var mapElementComponent = ref elementEntity.Get<MapElementComponent>();

            var node = GameParameters.galaxyData.columns[GameParameters.currentColumn + 1].nodes[GameParameters.nextNode];
            mapElementComponent.name = node.name;
            mapElementComponent.type = node.type;
            mapElementComponent.element = node.element;
            mapElementComponent.mainPath = node.mainPath;

            // elementInstance.InstantiateEntity();

            // on complete =>
            GameParameters.currentColumn++;
            GameParameters.currentNode = GameParameters.nextNode;
        }
        
        // IF KEY UP/DOWN => swap selection

        private void Update()
        {

        }
    }
}