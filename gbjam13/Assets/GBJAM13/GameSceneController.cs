using System.Linq;
using Game.Scenes;
using GBJAM13.Components;
using GBJAM13.Data;
using GBJAM13.UI;
using Gemserk.Leopotam.Ecs;
using Gemserk.Utilities;
using MyBox;
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
        public UIOptions uiOptions;
        
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
            var saveGame = GameParameters.saveGame;
            
            if (GameParameters.saveGame == null)
            {
                GameParameters.saveGame = new SaveGame();
                GameSceneLoader.LoadNextScene("MapGenerator");
                return;
            }
            
            if (saveGame.nextNode == -1)
            {
                GameSceneLoader.LoadNextScene("Map");
                return;
            }

            var world = worldReference.GetReference(gameObject);
            currentEventEntity = world.CreateEntity(elementInstance.entityDefinition);

            currentEventEntity.Get<PositionComponent>().value = elementInstance.transform.position;
            
            ref var mapElementComponent = ref currentEventEntity.Get<MapElementComponent>();

            var node = saveGame.galaxyData.columns[saveGame.currentColumn + 1].nodes[saveGame.nextNode];
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
         
            uiOptions.ShowOptions(eventData.options.Select(o => o.GenerateDescription()).ToList());
            
            // dialog.ShowText(eventData.options[0].description);
        }
        
        // ON OPTION ACCEPTED FROM DIALOG UI (OR CREATE ANOTHER UI)

        public void DisplayCurrentOutcome()
        {
            uiOptions.window.Close();
            
            // ref var mapElementComponent = ref currentEventEntity.Get<MapElementComponent>();

            // var eventData = eventsDb.GetInterface<IObjectList>()
            //     .FindByName<EventElementData>(mapElementComponent.eventName);
         
            // uiOptions.ShowOptions(eventData.options);
            
            ref var mapElementComponent = ref currentEventEntity.Get<MapElementComponent>();

            var eventData = eventsDb.GetInterface<IObjectList>()
                .FindByName<EventElementData>(mapElementComponent.eventName);

            var selectedOption = eventData.options[uiOptions.selectedOption];
            
            var outcome = selectedOption.outcomes.GetRandom();
            var randomNumber = UnityEngine.Random.Range(1, 5);
            uiDialog.ShowText($"{outcome.description} (+{randomNumber} {outcome.resourceType.name})");
        }
        
        public void OnCurrentEventCompleted()
        {
            uiDialog.window.Close();
            uiOptions.window.Close();

            var saveGame = GameParameters.saveGame;
            
            saveGame.currentColumn++;
            saveGame.currentNode = saveGame.nextNode;

            if (saveGame.currentColumn == saveGame.galaxyData.columns.Length - 1)
            {
                saveGame.totalJumps += SaveGame.JumpIncrementPerRun;
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