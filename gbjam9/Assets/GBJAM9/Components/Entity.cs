using System;
using UnityEngine;

namespace GBJAM9.Components
{
    public class Entity : MonoBehaviour, IGameComponent
    {
        [NonSerialized]
        public bool destroyed;

        [NonSerialized]
        public World world;

        [NonSerialized]
        public PlayerComponent player;
        
        [NonSerialized]
        public UnitStateComponent state;

        [NonSerialized]
        public ProjectileComponent projectileComponent;
        
        [NonSerialized]
        public PickupComponent pickupComponent;
        
        [NonSerialized]
        public VisualEffectComponent visualEffectComponent;

        [NonSerialized]
        public UnitModelComponent model;
        
        [NonSerialized]
        public HealthComponent health;

        [NonSerialized]
        public SoundEffectComponent sfxComponent;

        [NonSerialized]
        public ColliderComponent colliderComponent;

        [NonSerialized]
        public InventoryComponent inventoryComponent;

        [NonSerialized] 
        public UnitInput input;

        [NonSerialized]
        public UnitMovement movement;

        [NonSerialized]
        public UnitInputGameBoyControllerComponent gameboyControllerComponent;

        [NonSerialized]
        public AttackComponent attackComponent;

        private void Awake()
        {
            world = FindObjectOfType<World>();
            player = GetComponent<PlayerComponent>();
            state = GetComponent<UnitStateComponent>();
            projectileComponent = GetComponent<ProjectileComponent>();
            pickupComponent = GetComponent<PickupComponent>();
            visualEffectComponent = GetComponent<VisualEffectComponent>();
            model = GetComponent<UnitModelComponent>();
            health = GetComponent<HealthComponent>();
            sfxComponent = GetComponent<SoundEffectComponent>();
            colliderComponent = GetComponent<ColliderComponent>();
            inventoryComponent = GetComponent<InventoryComponent>();
            input = GetComponent<UnitInput>();
            movement = GetComponent<UnitMovement>();
            gameboyControllerComponent = GetComponent<UnitInputGameBoyControllerComponent>();
            attackComponent = GetComponent<AttackComponent>();
        }

        private void OnEnable()
        {
            if (world != null)
            {
                world.entities.Add(this);
            }
        }

        private void OnDisable()
        {
            if (world != null)
            {
                world.entities.Remove(this);
            }
        }
    }
}