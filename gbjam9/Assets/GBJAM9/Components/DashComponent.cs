using System;
using GBJAM.Commons.Prefabs.Sfx;
using UnityEngine;

namespace GBJAM9.Components
{
    public class DashComponent : MonoBehaviour, IEntityComponent
    {
        // how much it dashes once activated
        public float duration = 0.15f;
        
        public float cooldown = 1.0f;

        public float speed;

        public SfxVariant sfx;
        
        [NonSerialized]
        public float durationCurrent;
        
        [NonSerialized]
        public float cooldownCurrent;
        
        [NonSerialized]
        public Vector2 direction;
    }
    
}