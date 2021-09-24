using System;
using System.Reflection;
using GBJAM9.Controllers;
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
        public ProjectileComponent projectile;
        
        [NonSerialized]
        public PickupComponent pickup;
        
        [NonSerialized]
        public VisualEffectComponent vfx;

        [NonSerialized]
        public UnitModelComponent model;
        
        [NonSerialized]
        public HealthComponent health;

        [NonSerialized]
        public SoundEffectComponent sfx;

        [NonSerialized]
        public ColliderComponent colliderComponent;

        [NonSerialized]
        public InventoryComponent inventory;

        [NonSerialized] 
        public UnitInput input;

        [NonSerialized]
        public UnitMovement movement;

        [NonSerialized]
        public UnitInputGameBoyControllerComponent gameboyController;

        [NonSerialized]
        public AttackComponent attack;

        [NonSerialized]
        public ControllerComponent controller;

        [NonSerialized]
        public DecoComponent decoComponent;
        
        private void Awake()
        {
            world = FindObjectOfType<World>();

            var fields = GetType().GetFields(BindingFlags.Public | BindingFlags.Instance);
            foreach (var fieldInfo in fields)
            {
                var fieldType = fieldInfo.FieldType;
                if (typeof(IGameComponent).IsAssignableFrom(fieldType))
                {
                    fieldInfo.SetValue(this, GetComponent(fieldInfo.FieldType));
                }
            }
            // player = GetComponent<PlayerComponent>();
            // state = GetComponent<UnitStateComponent>();
            // projectile = GetComponent<ProjectileComponent>();
            // pickup = GetComponent<PickupComponent>();
            // vfx = GetComponent<VisualEffectComponent>();
            // model = GetComponent<UnitModelComponent>();
            // health = GetComponent<HealthComponent>();
            // sfx = GetComponent<SoundEffectComponent>();
            // colliderComponent = GetComponent<ColliderComponent>();
            // inventory = GetComponent<InventoryComponent>();
            // input = GetComponent<UnitInput>();
            // movement = GetComponent<UnitMovement>();
            // gameboyController = GetComponent<UnitInputGameBoyControllerComponent>();
            // attack = GetComponent<AttackComponent>();
            // controller = GetComponent<ControllerComponent>();
            // decoComponent = GetComponent<DecoComponent>();
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