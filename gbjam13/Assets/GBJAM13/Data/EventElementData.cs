using System;
using UnityEngine;

namespace GBJAM13.Data
{
    [CreateAssetMenu(menuName = "GBJAM13/EventElementData")]
    public class EventElementData : ScriptableObject
    {
        public enum ResourceIncomeType
        {
            Negative = 0,
            Positive = 1,
            None = 2
        }
        
        [Serializable]
        public struct Option
        {
            [TextArea(2, 5)]
            public string description;
            public ResourceIncomeType type;
            public ResourceTypeData resourceType;
            
            public Outcome[] outcomes;

            public string GenerateDescription()
            {
                var number = UnityEngine.Random.Range(1, 4);
                return $"{description} (-{number} {resourceType.name})";
            }
        }        
        
        [Serializable]
        public struct Outcome
        {
            [TextArea(2, 5)]
            public string description;
            public ResourceIncomeType type;
            public ResourceTypeData resourceType;
        }   
        
        public EventTypeData type;
        
        [TextArea(2, 5)]
        public string description;
        public Option[] options;
    }
}