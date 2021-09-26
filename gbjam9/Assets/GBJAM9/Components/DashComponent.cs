using System;
using UnityEngine;

namespace GBJAM9.Components
{
    public class DashComponent : MonoBehaviour, IEntityComponent
    {
        // how much it dashes once activated
        public float duration = 0.15f;
        
        public float cooldown = 1.0f;
        
        [NonSerialized]
        public float durationCurrent;
        
        [NonSerialized]
        public float cooldownCurrent;
        
        [NonSerialized]
        public Vector2 direction;
        
    }
}