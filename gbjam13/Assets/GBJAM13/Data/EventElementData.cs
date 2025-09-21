using System;
using MyBox;
using UnityEngine;

namespace GBJAM13.Data
{
    [CreateAssetMenu(menuName = "GBJAM13/EventElementData")]
    public class EventElementData : ScriptableObject
    {
        [Serializable]
        public struct Option
        {
            [TextArea(2, 5)]
            public string description;
            public ResourceTypeData resourceType;

            public RangedInt numberRange;
            
            public Outcome[] outcomes;

            public string GenerateDescription(int number)
            {
                if (number > 0)
                {
                    return $"{description} (+{number} {resourceType.name})";
                } 
                
                if (number < 0)
                {
                    return $"{description} ({number} {resourceType.name})";
                }
                
                return $"{description}";
            }
        }        
        
        [Serializable]
        public struct Outcome
        {
            [TextArea(2, 5)]
            public string description;
            public ResourceTypeData resourceType;
            
            public RangedInt numberRange;
            
            public string GenerateDescription(int number)
            {
                if (number > 0)
                {
                    return $"{description} (+{number} {resourceType.name})";
                } 
                
                if (number < 0)
                {
                    return $"{description} ({number} {resourceType.name})";
                }
                
                return $"{description}";
            }
        }   
        
        public EventTypeData type;
        
        [TextArea(2, 5)]
        public string description;
        public Option[] options;
    }
}