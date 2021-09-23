using System;
using UnityEngine;

namespace GBJAM9.Components
{
    public class ProjectileComponent : MonoBehaviour, IGameComponent
    {
        public int totalTargets = 1;
        
        public int damage;

        [NonSerialized]
        public bool damagePerformed;
        
        public GameObject hitSfxPrefab;
    }
}