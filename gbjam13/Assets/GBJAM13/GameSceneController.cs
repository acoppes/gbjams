using Game.Scenes;
using GBJAM13.Components;
using GBJAM13.Data;
using GBJAM13.UI;
using Gemserk.Leopotam.Ecs;
using Gemserk.Utilities;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace GBJAM13
{
    public class GameSceneController : MonoBehaviour
    {
        public WorldReference worldReference;

        public EntityPrefabInstance elementInstance;
        
        [FormerlySerializedAs("dialog")] 
        public UIDialog uiDialog;
        public UIEventOptions uiOptions;
        
        public UnityEvent onEventCompleted;
        public UnityEvent onGalaxyCompleted;
        
        [ObjectType(typeof(IObjectList), filterString = "Database")]
        public Object eventsDb;
        
        [ObjectType(typeof(IObjectList), filterString = "Database")]
        public Object eventVariantsDb;
        
        [FormerlySerializedAs("mapElementsDatabase")] 
        [ObjectType(typeof(IObjectList), filterString = "Database")]
        public Object eventNamesDb;

        private Entity currentEventEntity;

        
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
            currentEventEntity = world.CreateEntity(elementInstance.entityDefinition);

            currentEventEntity.Get<PositionComponent>().value = elementInstance.transform.position;
            
            ref var mapElementComponent = ref currentEventEntity.Get<MapElementComponent>();

            var node = GameParameters.galaxyData.columns[GameParameters.currentColumn + 1].nodes[GameParameters.nextNode];
            mapElementComponent.name = node.name;
            mapElementComponent.eventName = node.eventName;
            mapElementComponent.eventType = node.type;
            mapElementComponent.eventVariant = node.eventVariant;
            mapElementComponent.mainPath = node.mainPath;

            // elementInstance.InstantiateEntity();
            // on complete =>
        }

        public void DisplayCurrentEventDescription()
        {
            ref var mapElementComponent = ref currentEventEntity.Get<MapElementComponent>();

            var eventData = eventsDb.GetInterface<IObjectList>()
                .FindByName<EventElementData>(mapElementComponent.eventName);
            
            uiDialog.ShowText(eventData.description);
        }
        
        public void DisplayCurrentEventOptions()
        {
            uiDialog.window.Close();
            
            ref var mapElementComponent = ref currentEventEntity.Get<MapElementComponent>();

            var eventData = eventsDb.GetInterface<IObjectList>()
                .FindByName<EventElementData>(mapElementComponent.eventName);
         
            uiOptions.ShowOptions(eventData.options);
            
            // dialog.ShowText(eventData.options[0].description);
        }
        
        // ON OPTION ACCEPTED FROM DIALOG UI (OR CREATE ANOTHER UI)

        public void OnCurrentEventCompleted()
        {
            uiDialog.window.Close();
            uiOptions.window.Close();
            
            GameParameters.currentColumn++;
            GameParameters.currentNode = GameParameters.nextNode;

            if (GameParameters.currentColumn == GameParameters.galaxyData.columns.Length - 1)
            {
                GameParameters.totalJumps += GameParameters.JumpIncrementPerRun;
                onGalaxyCompleted.Invoke();
            }
            else
            {
                onEventCompleted.Invoke();
            }
        }
        
        // IF KEY UP/DOWN => swap selection

        private void Update()
        {

        }
    }
}