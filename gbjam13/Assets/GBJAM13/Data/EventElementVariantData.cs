using UnityEngine;

namespace GBJAM13.Data
{
    [CreateAssetMenu(menuName = "GBJAM13/EventElementVariantData")]
    public class EventElementVariantData : ScriptableObject
    {
        public EventTypeData eventType;
        public string[] variants;
    }
}