using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using GBJAM13.Data;
using Gemserk.BitmaskTypes;
using Gemserk.Leopotam.Ecs;
using MyBox;
using UnityEngine;
using yutokun;

namespace GBJAM13
{
    public class EventsDatabase : SingletonBehaviour<EventsDatabase>
    {
        // LOAD EVENTS AT START FROM CSV AND OTHER DATABASE

        public TextAsset eventsCsvDatabase;
        
        public TypeSetAsset eventTypes;
        public TypeSetAsset resourceTypes;
        
        public readonly Dictionary<string, EventElementData> eventsDictionary = new(StringComparer.OrdinalIgnoreCase);
        
        private void Awake()
        {
            // LOAD FROM CSV AND FROM OTHER AND BUILD DATABASE
            LoadEventsFromCsv(eventsCsvDatabase.text);
            
            // var eventsFromAssets = eventsDb.GetInterface<IObjectList>().Get<EventElementData>();
            //
            // foreach (var eventElementData in eventsFromAssets)
            // {
            //     eventsDictionary[eventElementData.eventName] = eventElementData;
            // }
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
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture; 
            Thread.CurrentThread.CurrentUICulture = CultureInfo.InvariantCulture; 
            
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
                        Debug.LogError($"Failed to get resource type {row[3]} from {row[1]}, {row[2]}");
                    }

                    try
                    {
                        currentOption = new EventElementData.Option()
                        {
                            description = row[2],
                            resourceType = resourceType,
                            numberRange = new RangedInt(int.Parse(row[4]), int.Parse(row[5])),
                            outcomes = new List<EventElementData.Outcome>()
                        };
                    } catch
                    {
                        Debug.LogError($"Failed while parsing {row[4]} and {row[5]} from {row[1]}, {row[2]}");
                    }
                    
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
                        Debug.LogError($"Failed to get resource type {row[3]} from {row[1]}, {row[2]}");
                    }

                    var outcomeStats = new List<string>();
                    
                    if (!string.IsNullOrEmpty(row[7]))
                    {
                        var stats = row[7].Split(",");
                        outcomeStats = stats.ToList();
                    }
                    
                    try
                    {
                        currentOutcome = new EventElementData.Outcome()
                        {
                            description = row[2],
                            resourceType = resourceType,
                            numberRange = new RangedInt(int.Parse(row[4]), int.Parse(row[5])),
                            stats = outcomeStats
                        };
                    } catch
                    {
                        Debug.LogError($"Failed while parsing {row[4]} and {row[5]} from {row[1]}, {row[2]}");
                    }
                        
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

            foreach (var eventData in eventsDictionary.Values)
            {
                Debug.Log(JsonUtility.ToJson(eventData));
            }
        }
    }
}