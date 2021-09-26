using System;
using UnityEngine;

namespace GBJAM9.Components
{
    public class HealthComponent : MonoBehaviour, IEntityComponent
    {
        public int total;
        public int current;

        public bool immortal;
        
        [NonSerialized]
        public int damages;

        [NonSerialized]
        public bool alive = true;

        public GameObject vfxPrefab;

        public Transform vfxAttachPoint;
    }
}