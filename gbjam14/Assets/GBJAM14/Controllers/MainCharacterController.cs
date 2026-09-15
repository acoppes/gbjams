using System.Collections.Generic;
using Game.Components;
using Game.Utilities;
using GBJAM14.Components;
using Gemserk.Leopotam.Ecs;
using Gemserk.Leopotam.Ecs.Controllers;
using Gemserk.Leopotam.Ecs.Events;
using Unity.Mathematics;
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
            var inputDirection = input.direction3d();
            ref var animations = ref entity.Get<AnimationsComponent>();

            movement.movingDirection = new Vector3(inputDirection.x, inputDirection.z * 0.75f, 0);

            ref var lookingDirection = ref entity.Get<LookingDirection>();
            
            if (movement.movingDirection.sqrMagnitude > 0.1f)
            {
                lookingDirection.value = movement.movingDirection.normalized;
            }

            // search for interactions

            var results = new List<Target>();
            world.GetTargets(new RuntimeTargetingParameters()
            {
                alliedPlayersBitmask = entity.Get<PlayerComponent>().GetAlliedPlayers(),
                direction = lookingDirection.value,
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

            if (inputDirection.sqrMagnitude > 0.01f)
            {
                if (Mathf.Abs(inputDirection.x) > 0.01f)
                {
                    if (!animations.IsPlaying("walk-side"))
                    {
                        animations.Play("walk-side");
                    }
                }
                else if (inputDirection.z > 0.01f)
                {
                    if (!animations.IsPlaying("walk-up"))
                    {
                        animations.Play("walk-up");
                    }
                }
                else if (inputDirection.z < 0.01f)
                {
                    if (!animations.IsPlaying("walk-down"))
                    {
                        animations.Play("walk-down");
                    }
                }
            }
            else
            {
                if (Mathf.Abs(lookingDirection.value.x) > 0.01f)
                {
                    if (!animations.IsPlaying("idle-side"))
                    {
                        animations.Play("idle-side");
                    }
                }
                else if (lookingDirection.value.y > 0.01f)
                {
                    if (!animations.IsPlaying("idle-up"))
                    {
                        animations.Play("idle-up");
                    }
                }
                else if (lookingDirection.value.y < 0.01f)
                {
                    if (!animations.IsPlaying("idle-down"))
                    {
                        animations.Play("idle-down");
                    }
                }
            }

        }
    }
}