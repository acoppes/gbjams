using System;
using UnityEngine;

namespace GBJAM9.Components
{
    public class UnitComponent : MonoBehaviour, IGameComponent
    {
        public int player;

        public bool destroyed;

        private World world;

        [NonSerialized]
        public UnitStateComponent unitState;

        [NonSerialized]
        public ProjectileComponent projectileComponent;
        
        [NonSerialized]
        public PickupComponent pickupComponent;
        
        [NonSerialized]
        public VisualEffectComponent visualEffectComponent;

        [NonSerialized]
        public UnitModelComponent unitModel;
        
        [NonSerialized]
        public HealthComponent health;

        [NonSerialized]
        public SoundEffectComponent sfxComponent;

        [NonSerialized]
        public ColliderComponent colliderComponent;
        
        private void Awake()
        {
            world = FindObjectOfType<World>();
            unitState = GetComponent<UnitStateComponent>();
            projectileComponent = GetComponent<ProjectileComponent>();
            pickupComponent = GetComponent<PickupComponent>();
            visualEffectComponent = GetComponent<VisualEffectComponent>();
            unitModel = GetComponent<UnitModelComponent>();
            health = GetComponent<HealthComponent>();
            sfxComponent = GetComponent<SoundEffectComponent>();
            colliderComponent = GetComponent<ColliderComponent>();
        }

        private void OnEnable()
        {
            if (world != null)
            {
                world.units.Add(this);
            }
        }

        private void OnDisable()
        {
            if (world != null)
            {
                world.units.Remove(this);
            }
        }
    }
}