using System;
using System.Collections.Generic;
using Beatemup.Ecs;
using Gemserk.Leopotam.Ecs;
using Gemserk.Leopotam.Ecs.Controllers;
using Gemserk.Leopotam.Ecs.Gameplay;
using UnityEngine;

namespace Beatemup.Definitions
{
    public class UnitDefinition : MonoBehaviour, IEntityDefinition
    {
        [Flags]
        public enum UnitType
        {
            Nothing = 0,
            Everything = -1,
            Unit = 1 << 0,
            Bullet = 1 << 1,
            Pickup = 1 << 2
        }
        
        public float movementSpeed;
        public float health;

        public float invulnerableTime;

        public bool showLookingDirection = true;

        public bool canBeControlled = true;
        public bool canBeTargeted = true;
    
        public UnitType unitType;

        public bool canJump = false;

        public bool autoDestroyOnDeath = true;

        public float colliderRadius = 0f;
        public bool collidesWithTerrain = true;
    
        public GameObject modelPrefab;
        public GameObject configuration;

        public bool autoDestroyOutsideCamera = false;
        public bool keepInsideCameraBounds = false;

        public bool providesSpecialWeapon;
        public int specialWeapon;

        public bool hasSpecialWeapon;

        public void Apply(World world, Entity entity)
        {
            world.AddComponent(entity, new PlayerComponent());
            world.AddComponent(entity, new PositionComponent());

            if (unitType != 0)
            {
                world.AddComponent(entity, new UnitTypeComponent()
                {
                    type = (int) unitType
                });
            }
        
            if (canJump)
            {
                world.AddComponent(entity, new JumpComponent());
            }
        
            world.AddComponent(entity, new LookingDirection
            {
                value = Vector2.right, 
                disableIndicator = !showLookingDirection
            });

            if (canBeControlled)
            {
                world.AddComponent(entity, new PlayerInputComponent());
                world.AddComponent(entity, new UnitControlComponent());
            }

            world.AddComponent(entity, new UnitStateComponent());
            world.AddComponent(entity, new StatesComponent());

            if (modelPrefab != null)
            {
                world.AddComponent(entity, new UnitModelComponent
                {
                    prefab = modelPrefab
                });
            
                if (modelPrefab.GetComponentInChildren<Animator>() != null)
                {
                    world.AddComponent(entity, new AnimatorComponent());
                }
            }

            world.AddComponent(entity, new AbilitiesComponent
            {
                abilities = new List<Ability>(),
                targetings = new List<Targeting>()
            });

            world.AddComponent(entity, new UnitMovementComponent()
            {
                speed = movementSpeed
            });
        
            if (canBeTargeted)
            {
                world.AddComponent(entity, new TargetComponent());
            }
        
            if (health > 0)
            {
                world.AddComponent(entity, new HealthComponent
                {
                    current = health,
                    total = health,
                    autoDestroyOnDeath = autoDestroyOnDeath,
                    invulnerableTime = invulnerableTime
                });
            }

            if (colliderRadius > 0)
            {
                world.AddComponent(entity, new ColliderComponent
                {
                    radius = colliderRadius,
                    collisions = new Collider2D[10]
                });
            }

            if (collidesWithTerrain)
            {
                world.AddComponent(entity, new TerrainCollisionComponent());
            }

            if (configuration != null)
            {
                world.AddComponent(entity, new ConfigurationComponent()
                {
                    configuration = configuration.GetComponentInChildren<IConfiguration>()
                });
            }

            if (autoDestroyOutsideCamera)
            {
                world.AddComponent(entity, new AutoDestroyOutsideCamera());
            }

            if (keepInsideCameraBounds)
            {
                world.AddComponent(entity, new KeepInsideCameraComponent());
            }

            if (hasSpecialWeapon)
            {
                world.AddComponent(entity, new SpecialWeaponComponent());
            }
            
            if (providesSpecialWeapon)
            {
                world.AddComponent(entity, new SpecialWeaponProviderComponent()
                {
                    special = specialWeapon
                });
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(transform.position, colliderRadius);
        }
    }
}