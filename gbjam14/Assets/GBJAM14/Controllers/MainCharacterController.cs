using System;
using System.Collections.Generic;
using Game.Components;
using Game.Utilities;
using GBJAM14.Components;
using Gemserk.Leopotam.Ecs;
using Gemserk.Leopotam.Ecs.Components;
using Gemserk.Leopotam.Ecs.Controllers;
using Gemserk.Leopotam.Ecs.Events;

namespace GBJAM14.Controllers
{
    public class MainCharacterController : ControllerBase, IUpdate
    {
        public Targeting targeting;
        
        public void OnUpdate(World world, Entity entity, float dt)
        {
            var input = entity.Get<InputComponent>();
            ref var movement = ref entity.Get<MovementComponent>();
            movement.movingDirection = input.direction3d();
            
            // search for interactions

            var bufferedInput = entity.Get<BufferedInputComponent>();
            if (bufferedInput.HasBufferedAction(input.GetButton("button1")))
            {
                var results = new List<Target>();
                world.GetTargets(new RuntimeTargetingParameters()
                {
                    alliedPlayersBitmask = entity.Get<PlayerComponent>().GetAlliedPlayers(),
                    direction = entity.Get<LookingDirection>().value,
                    filter = targeting.targetingFilter,
                    position = entity.Get<PositionComponent>().value,
                    rangeMultiplier = 1
                }, results);

                foreach (var target in results)
                {
                    if (target.entity && target.entity.Has<CanBeInteractedComponent>())
                    {
                        world.CreateEntity(null, null, (e) =>
                        {
                            e.Add(new InteractActionComponent()
                            {
                                source = entity,
                                target = target.entity
                            });
                        });
                        
                        break;
                    }
                }
                
                bufferedInput.ConsumeBuffer();
            }
        }
    }
}