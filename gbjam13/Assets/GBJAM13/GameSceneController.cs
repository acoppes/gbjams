using Game.Scenes;
using GBJAM13.Components;
using Gemserk.Leopotam.Ecs;
using UnityEngine;
using UnityEngine.Events;

namespace GBJAM13
{
    public class GameSceneController : MonoBehaviour
    {
        public WorldReference worldReference;

        public EntityPrefabInstance elementInstance;

        public UnityEvent onEventCompleted;
        public UnityEvent onGalaxyCompleted;
        
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
            mapElementComponent.eventVariant = node.eventVariant;
            mapElementComponent.mainPath = node.mainPath;

            // elementInstance.InstantiateEntity();
            // on complete =>
        }

        public void OnCurrentEventCompleted()
        {
            GameParameters.currentColumn++;
            GameParameters.currentNode = GameParameters.nextNode;

            if (GameParameters.currentColumn == GameParameters.galaxyData.columns.Length - 1)
            {
                onGalaxyCompleted.Invoke();
            }
            else
            {
                GameParameters.totalJumps += GameParameters.JumpIncrementPerRun;
                onEventCompleted.Invoke();
            }
        }
        
        // IF KEY UP/DOWN => swap selection

        private void Update()
        {

        }
    }
}