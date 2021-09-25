using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace GBJAM9.Components
{
    public class PickupComponent : MonoBehaviour, IGameComponent
    {
        public GameObject pickupVfxPrefab;
        
        public string pickupType;
        public Object pickupData;
        
        public int count;
        
        [NonSerialized]
        public bool picked;
    }
}