using System;
using UnityEngine;

namespace GBJAM9.Components
{
    public class PickupComponent : MonoBehaviour, IGameComponent
    {
        public GameObject pickupVfxPrefab;
        public string pickupType;
        public int count;
    }
}