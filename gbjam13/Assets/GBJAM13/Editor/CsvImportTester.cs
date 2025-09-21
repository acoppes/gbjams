using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using GBJAM13.Data;
using MyBox;
using UnityEditor;
using UnityEngine;
using yutokun;

namespace GBJAM13.Editor
{
    public static class CsvImportTester
    {
        [MenuItem("GBJAM/GBJAM13/Load From CSV")]
        public static void LoadColorSetFromPaletteFile()
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture; 
            Thread.CurrentThread.CurrentUICulture = CultureInfo.InvariantCulture; 
            
            var csvTextAsset = AssetDatabase.LoadAssetAtPath<TextAsset>("Assets/Resources/events-database.csv");

            var results = CSVParser.LoadFromString(csvTextAsset.text);

            EventElementData currentEvent = null;
            var buildingOption = false;
            EventElementData.Option currentOption = default;
            
            var buildingOutcome = false;
            EventElementData.Outcome currentOutcome = default;
            
            // ignore first row which is mainly the type...
            for (var i = 1; i < results.Count; i++)
            {
                var row = results[i];

                if (row[0].Equals("event", StringComparison.OrdinalIgnoreCase))
                {
                    if (currentEvent)
                    {
                        // LOG TO CONSOLE/SAVE/SOMETHING
                        Debug.Log(JsonUtility.ToJson(currentEvent));
                        currentEvent = null;
                    }
                    
                    currentEvent = ScriptableObject.CreateInstance<EventElementData>();
                    // currentEvent.type = row[1];
                    // event type = TODO: GET FROM DATABASE
                    currentEvent.name = row[2];
                    currentEvent.description = row[3];
                }
                
                if (row[0].Equals("event_option", StringComparison.OrdinalIgnoreCase))
                {
                    if (!currentEvent)
                    {
                        throw new Exception("Detected option without event");
                    }
                    
                    if (buildingOption)
                    {
                        // store it in the event
                        currentEvent.options.Add(currentOption);
                    }

                    buildingOption = true;
                    
                    currentOption = new EventElementData.Option()
                    {
                        description = row[2],
                        numberRange = new RangedInt(int.Parse(row[4]), int.Parse(row[5])),
                        outcomes = new List<EventElementData.Outcome>(),
                        // resourceType = TODO: GET FROM DATABASE
                    };
                }
                
                if (row[0].Equals("event_option_outcome", StringComparison.OrdinalIgnoreCase))
                {
                    if (!currentEvent)
                    {
                        throw new Exception("Detected option without event");
                    }
                    
                    if (buildingOutcome)
                    {
                        // store it in the event
                        currentOption.outcomes.Add(currentOutcome);
                    }

                    buildingOutcome = true;
                    
                    currentOutcome = new EventElementData.Outcome()
                    {
                        description = row[2],
                        numberRange = new RangedInt(int.Parse(row[4]), int.Parse(row[5])),
                        // resourceType = TODO: GET FROM DATABASE
                    };
                }
            }
            
            if (currentEvent)
            {
                // LOG TO CONSOLE/SAVE/SOMETHING
                Debug.Log(JsonUtility.ToJson(currentEvent));
                currentEvent = null;
            }
        }
    }
}