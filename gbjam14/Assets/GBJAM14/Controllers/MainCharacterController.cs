using System;
using System.Collections.Generic;
using Game.Components;
using Game.Utilities;
using GBJAM14.Components;
using Gemserk.Leopotam.Ecs;
using Gemserk.Leopotam.Ecs.Components;
using Gemserk.Leopotam.Ecs.Controllers;
using Gemserk.Leopotam.Ecs.Events;
using UnityEngine;

namespace GBJAM14.Controllers
{
    public class MainCharacterController : ControllerBase, IUpdate
    {
        public Targeting targeting;
        
        public void OnUpdate(World world, Entity entity, float dt)
        {
            var input = entity.Get<InputComponent>();
            ref var movement = ref entity.Get<MovementComponent>();
            var dir = input.direction3d();
            movement.movingDirection = new Vector3(dir.x, dir.z, 0);
            
            // search for interactions
            
            var results = new List<Target>();
            world.GetTargets(new RuntimeTargetingParameters()
            {
                alliedPlayersBitmask = entity.Get<PlayerComponent>().GetAlliedPlayers(),
                direction = entity.Get<LookingDirection>().value,
                filter = targeting.targetingFilter,
                position = entity.Get<PositionComponent>().value,
                rangeMultiplier = 1
            }, results);

            var interactEntity = Entity.NullEntity;
            
            foreach (var target in results)
            {
                if (target.entity && target.entity.Has<InteractableComponent>())
                {
                    interactEntity = target.entity;
                    target.entity.Get<InteractableComponent>().focusedByPlayer = true;
                    break;
                }
            }

            var bufferedInput = entity.Get<BufferedInputComponent>();
            if (bufferedInput.HasBufferedAction(input.GetButton("button1")))
            {
                if (interactEntity)
                {
                    world.CreateEntity(null, null, (e) =>
                    {
                        e.Add(new InteractActionComponent()
                        {
                            source = entity,
                            target = interactEntity
                        });
                    });
                }
                
                bufferedInput.ConsumeBuffer();
            }
        }
    }
}