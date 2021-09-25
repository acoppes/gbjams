using System;
using System.Reflection;
using GBJAM9.Controllers;
using UnityEngine;

namespace GBJAM9.Components
{
    public class Entity : MonoBehaviour, IEntityComponent
    {
        [NonSerialized]
        public bool destroyed;

        [NonSerialized]
        public World world;

        [NonSerialized]
        public MainUnitComponent mainUnit;

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

        [NonSerialized]
        public RoomComponent room;

        [NonSerialized]
        public RoomExitComponent roomExit;
        
        private void Awake()
        {
            world = FindObjectOfType<World>();

            var fields = GetType().GetFields(BindingFlags.Public | BindingFlags.Instance);
            foreach (var fieldInfo in fields)
            {
                var fieldType = fieldInfo.FieldType;
                if (typeof(IEntityComponent).IsAssignableFrom(fieldType))
                {
                    fieldInfo.SetValue(this, GetComponent(fieldInfo.FieldType));
                }
            }
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