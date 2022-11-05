using System.Collections.Generic;
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

            world.AddComponent(entity, ControlComponent.Default());

            world.AddComponent(entity, new ModelStateComponent
            {
                states = new Dictionary<string, bool>()
            });
            
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