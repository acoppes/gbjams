using System;
using Beatemup.Ecs;
using Gemserk.Leopotam.Ecs;
using Gemserk.Leopotam.Ecs.Controllers;
using Gemserk.Leopotam.Ecs.Gameplay;
using UnityEngine;
using LookingDirection = Beatemup.Ecs.LookingDirection;

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
        }

        public UnitType unitType;
        public float movementSpeed;

        public GameObject modelPrefab;

        public void Apply(World world, Entity entity)
        {
            world.AddComponent(entity, new PlayerComponent());
            world.AddComponent(entity, new PositionComponent());
            world.AddComponent(entity, new LookingDirection
            {
                value = Vector2.right
            });

            if (unitType != 0)
            {
                world.AddComponent(entity, new UnitTypeComponent()
                {
                    type = (int) unitType
                });
            }
            
            world.AddComponent(entity, new ControlComponent());

            world.AddComponent(entity, new ModelStateComponent());
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
            world.AddComponent(entity, new UnitMovementComponent()
            {
                speed = movementSpeed
            });
        }
    }
}