using Gemserk.Utilities;
using UnityEngine;

namespace GBJAM13.Data
{
    [CreateAssetMenu(menuName = "GBJAM13/MapElementData")]
    public class MapElementData : ScriptableObject
    {
        public string type;
        public string elementName;
        
        public string[] prefixes;
        public string[] suffixes;

        public string GenerateName()
        {
            var randomPrefix = prefixes.Random();
            var randomSuffix = suffixes.Random();

            var generatedName = elementName;
            
            if (!string.IsNullOrEmpty(randomPrefix))
            {
                generatedName = $"{randomPrefix} {generatedName}";
            }
            
            if (!string.IsNullOrEmpty(randomSuffix))
            {
                generatedName = $"{generatedName} {randomSuffix}";
            }

            return generatedName;
        }
    }
}