using UnityEngine;

namespace GBJAM9.Components
{
    public class PickupComponent : MonoBehaviour, IGameComponent
    {
        public GameObject pickupVfxPrefab;
        
        public string pickupType;
        public Object pickupData;
        
        public int count;
    }
}