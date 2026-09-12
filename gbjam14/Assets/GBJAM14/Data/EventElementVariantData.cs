using UnityEngine;

namespace GBJAM14.Data
{
    [CreateAssetMenu(menuName = "GBJAM13/EventElementVariantData")]
    public class EventElementVariantData : ScriptableObject
    {
        public EventTypeData eventType;
        public string[] variants;
    }
}