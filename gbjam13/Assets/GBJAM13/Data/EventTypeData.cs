using Gemserk.BitmaskTypes;
using UnityEngine;

namespace GBJAM13.Data
{
    [CreateAssetMenu(menuName = "GBJAM13/EventTypeData")]
    public class EventTypeData : IntTypeAsset
    {
        private bool Equals(EventTypeData other)
        {
            return other.value == value;
        }

        public override bool Equals(object obj)
        {
            if (obj is null) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((EventTypeData)obj);
        }

        public override int GetHashCode()
        {
            return value.GetHashCode();
        }

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