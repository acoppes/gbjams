using Gemserk.BitmaskTypes;
using UnityEngine;

namespace GBJAM13.Data
{
    [CreateAssetMenu(menuName = "GBJAM13/EventTypeData")]
    public class EventTypeData : IntTypeAsset
    {
        public static bool operator ==(EventTypeData eventData, int value)
        {
            return eventData && eventData.value == value;
        }

        public static bool operator !=(EventTypeData eventData, int value)
        {
            return eventData && eventData.value != value;
        }
    }
}