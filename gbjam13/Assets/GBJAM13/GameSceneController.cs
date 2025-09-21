using System.Linq;
using Game;
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
        
        // [ObjectType(typeof(IObjectList), filterString = "Database")]
        // public Object eventsDb;
        
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

            var eventsDatabase = EventsDatabase.Instance;
            var eventData = eventsDatabase.eventsDictionary[mapElementComponent.eventName];
            
            // var eventData = eventsDb.GetInterface<IObjectList>()
            //     .FindByName<EventElementData>(mapElementComponent.eventName);
            
            uiDialog.ShowText(eventData.description);
        }

        private int temporaryResourceNumber;
        private EventElementData.Outcome randomOutcome;
        
        public void DisplayCurrentEventOptions()
        {
            var saveGame = GameParameters.saveGame;
            
            uiDialog.window.Close();
            
            ref var mapElementComponent = ref currentEventEntity.Get<MapElementComponent>();

            var eventsDatabase = EventsDatabase.Instance;
            var eventData = eventsDatabase.eventsDictionary[mapElementComponent.eventName];
            
            // var eventData = eventsDb.GetInterface<IObjectList>()
            //     .FindByName<EventElementData>(mapElementComponent.eventName);
         
            // temporaryResourceNumber = UnityEngine.Random.Range(1, 4);
            
            uiOptions.ShowOptions(eventData.options.Select(o =>
            {
                var number = o.numberRange.RandomInRangeInclusive();
                var current = saveGame.resources[o.resourceType.value];
                
                return new Option()
                {
                    name = o.GenerateDescription(number),
                    userData = number,
                    disabled = (current + number) < 0
                };
            }).ToList());
            
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

            var eventsDatabase = EventsDatabase.Instance;
            var eventData = eventsDatabase.eventsDictionary[mapElementComponent.eventName];
            
            // var eventData = eventsDb.GetInterface<IObjectList>()
            //     .FindByName<EventElementData>(mapElementComponent.eventName);

            var selectedOption = eventData.options[uiOptions.selectedOptionIndex];
            var selectedOptionResourceNumber = (int) uiOptions.selectedOption.userData;

            var saveGame = GameParameters.saveGame;
            if (selectedOptionResourceNumber != 0)
            {
                saveGame.resources[selectedOption.resourceType.value] += selectedOptionResourceNumber;
                if (saveGame.resources[selectedOption.resourceType.value] < 0)
                {
                    saveGame.resources[selectedOption.resourceType.value] = 0;
                }
            }
            
            randomOutcome = selectedOption.outcomes.GetRandom();
            temporaryResourceNumber = randomOutcome.numberRange.RandomInRangeInclusive();
            uiDialog.ShowText(randomOutcome.GenerateDescription(temporaryResourceNumber));
        }
        
        public void ProcessOutcomeResources()
        {
            var saveGame = GameParameters.saveGame;
            
            // could have a temporary delay until the event is completed
            if (temporaryResourceNumber != 0)
            {
                saveGame.resources[randomOutcome.resourceType.value] += temporaryResourceNumber;
                if (saveGame.resources[randomOutcome.resourceType.value] < 0)
                {
                    saveGame.resources[randomOutcome.resourceType.value] = 0;
                }
            }

            temporaryResourceNumber = 0;
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
    }
}