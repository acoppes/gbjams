using System;
using System.Collections.Generic;
using System.Linq;
using GBJAM13.Data;
using Gemserk.BitmaskTypes;
using Gemserk.Leopotam.Ecs;
using Gemserk.Utilities;
using MyBox;
using UnityEngine;
using UnityEngine.Assertions;
using yutokun;
using Object = UnityEngine.Object;

namespace GBJAM13
{
    public class EventsDatabase : SingletonBehaviour<EventsDatabase>
    {
        // LOAD EVENTS AT START FROM CSV AND OTHER DATABASE

        public TextAsset eventsCsvDatabase;
        
        [ObjectType(typeof(IObjectList), filterString = "Database")]
        public Object eventsDb;
        
        [ObjectType(typeof(IObjectList), filterString = "Database")]
        public Object eventVariantsDb;
        
        [ObjectType(typeof(IObjectList), filterString = "Database")]
        public Object eventNamesDb;

        public TypeSetAsset eventTypes;
        public TypeSetAsset resourceTypes;
        
        public readonly Dictionary<string, EventElementData> eventsDictionary = new(StringComparer.OrdinalIgnoreCase);
        
        private void Awake()
        {
            // LOAD FROM CSV AND FROM OTHER AND BUILD DATABASE
            LoadEventsFromCsv(eventsCsvDatabase.text);
            var eventsFromAssets = eventsDb.GetInterface<IObjectList>().Get<EventElementData>();

            foreach (var eventElementData in eventsFromAssets)
            {
                eventsDictionary[eventElementData.eventName] = eventElementData;
            }
        }

        public List<EventElementData> GetEventsOfType(int eventType)
        {
            return eventsDictionary.Values.Where(e => e.type == eventType).ToList();
        }
        
        public List<EventElementData> GetEventsNotOfType(int eventType)
        {
            return eventsDictionary.Values.Where(e => e.type != eventType).ToList();
        }

        private void LoadEventsFromCsv(string csvText)
        {
            var time = Time.realtimeSinceStartupAsDouble;
            
            var results = CSVParser.LoadFromString(csvText);

            EventElementData currentEvent = null;
            // var buildingOption = false;
            
            EventElementData.Option currentOption = default;
            EventElementData.Outcome currentOutcome = default;
            
            // ignore first row which is mainly the type...
            for (var i = 1; i < results.Count; i++)
            {
                var row = results[i];

                if (row[0].Equals("event", StringComparison.OrdinalIgnoreCase))
                {
                    // if (currentEvent)
                    // {
                    //     eventsDictionary[currentEvent.eventName] = currentEvent;
                    //     currentEvent = null;
                    // }
                    
                    currentEvent = ScriptableObject.CreateInstance<EventElementData>();
                    currentEvent.type = eventTypes.types
                            .First(t => t.name.Equals(row[1], StringComparison.OrdinalIgnoreCase))
                            .As<EventTypeData>();
                    
                    if (!currentEvent.type)
                    {
                        Debug.LogError($"Failed to get event type {row[1]}");
                    }
                    
                    currentEvent.eventName = row[2];
                    currentEvent.description = row[3];
                    
                    eventsDictionary[currentEvent.eventName] = currentEvent;
                }
                
                if (row[0].Equals("event_option", StringComparison.OrdinalIgnoreCase))
                {
                    if (!currentEvent)
                    {
                        throw new Exception("Detected option without event");
                    }
                    
                    // if (buildingOption)
                    // {
                    //     // store it in the event
                    //     currentEvent.options.Add(currentOption);
                    // }

                    // buildingOption = true;

                    ResourceTypeData resourceType = null;

                    if (!string.IsNullOrEmpty(row[3]))
                    {
                        resourceType = resourceTypes.types
                            .First(t => t.name.Equals(row[3], StringComparison.OrdinalIgnoreCase))
                            .As<ResourceTypeData>();
                    }
                    
                    if (!resourceType)
                    {
                        Debug.LogError($"Failed to get resource type {row[3]}");
                    }
                    
                    currentOption = new EventElementData.Option()
                    {
                        description = row[2],
                        resourceType = resourceType,
                        numberRange = new RangedInt(int.Parse(row[4]), int.Parse(row[5])),
                        outcomes = new List<EventElementData.Outcome>()
                    };
                    
                    currentEvent.options.Add(currentOption);
                }
                
                if (row[0].Equals("event_option_outcome", StringComparison.OrdinalIgnoreCase))
                {
                    if (!currentEvent)
                    {
                        throw new Exception("Detected option without event");
                    }
                    
                    // if (buildingOutcome)
                    // {
                    //     // store it in the event
                    //     currentOption.outcomes.Add(currentOutcome);
                    // }

                    // buildingOutcome = true;
                    
                    ResourceTypeData resourceType = null;

                    if (!string.IsNullOrEmpty(row[3]))
                    {
                        resourceType = resourceTypes.types
                            .First(t => t.name.Equals(row[3], StringComparison.OrdinalIgnoreCase))
                            .As<ResourceTypeData>();
                    }
                    
                    if (!resourceType)
                    {
                        Debug.LogError($"Failed to get resource type {row[3]}");
                    }
                    
                    currentOutcome = new EventElementData.Outcome()
                    {
                        description = row[2],
                        resourceType = resourceType,
                        numberRange = new RangedInt(int.Parse(row[4]), int.Parse(row[5]))
                    };
                    
                    currentOption.outcomes.Add(currentOutcome);
                }
            }
            
            // if (currentEvent)
            // {
            //     eventsDictionary[currentEvent.eventName] = currentEvent;
            //     currentEvent = null;
            // }
            
            var completeTime = Time.realtimeSinceStartupAsDouble;
            
            Debug.Log($"CSV PARSE AND BUILD EVENTS: {completeTime} seconds");
        }
    }
}