using Game.Components;
using Game.Controllers;
using GBJAM11.Components;
using Gemserk.Leopotam.Ecs;
using Gemserk.Leopotam.Ecs.Components;
using Gemserk.Leopotam.Ecs.Controllers;
using Gemserk.Leopotam.Ecs.Events;
using UnityEngine;

namespace GBJAM11.Controllers
{
    public class NekosamaController : ControllerBase, IUpdate, IActiveController
    {
        public bool CanBeInterrupted(Entity entity, IActiveController activeController)
        {
            return true;
        }

        public void OnInterrupt(Entity entity, IActiveController activeController)
        {
            // if attacking, exit attack
            ExitAttack(entity);
            
            // if teleporting, exit teleport
        }
        
        public void OnUpdate(World world, Entity entity, float dt)
        {
            ref var states = ref entity.Get<StatesComponent>();
            ref var input = ref entity.Get<InputComponent>();
            ref var bufferedInput = ref entity.Get<BufferedInputComponent>();
            ref var animations = ref entity.Get<AnimationComponent>();
            
            // if attacking 
            // fire attack
            
            if (states.TryGetState("Attacking", out var state))
            {
                if (animations.IsPlaying("Attack") && animations.isCompleted)
                {
                    // fire kunai

                    ref var weapons = ref entity.Get<WeaponsComponent>();
                    var weapon = weapons.weapon;

                    var projectileEntity = world.CreateEntity(weapon.projectileDefinition);
                    projectileEntity.Get<PositionComponent>().value = entity.Get<PositionComponent>().value + new Vector3(0, 0.35f, 0);
                    
                    ref var projectile = ref projectileEntity.Get<ProjectileComponent>();
                    projectile.initialVelocity = entity.Get<LookingDirection>().value;
                    
                    ExitAttack(entity);
                }

                return;
            }

            if (bufferedInput.HasBufferedAction(input.button1()))
            {
                // fire attack
                bufferedInput.ConsumeBuffer();
                EnterAttack(entity);
                return;
            }
            
            if (bufferedInput.HasBufferedAction(input.button2()))
            {
                // teleport to kunai
                EnterTeleport(entity);
                return;
            }
        }

        private void EnterAttack(Entity entity)
        {
            // start anim, start state, etc...
            // lock looking direction, movement, etc..
            
            ref var states = ref entity.Get<StatesComponent>();
            ref var animations = ref entity.Get<AnimationComponent>();
            ref var activeController = ref entity.Get<ActiveControllerComponent>();
            ref var movement = ref entity.Get<MovementComponent>();

            activeController.TakeControl(entity, this);
            movement.speed = 0;
            animations.Play("Attack", 0);
            states.EnterState("Attacking");
        }

        private void ExitAttack(Entity entity)
        {
            // exit state, stop anim, etc...
            
            ref var states = ref entity.Get<StatesComponent>();
            ref var animations = ref entity.Get<AnimationComponent>();
            ref var activeController = ref entity.Get<ActiveControllerComponent>();
            ref var movement = ref entity.Get<MovementComponent>();

            activeController.ReleaseControl(this);
            movement.speed = movement.baseSpeed;
            states.ExitState("Attacking");
        }

        private void EnterTeleport(Entity entity)
        {
            
        }

        private void ExitTeleport(Entity entity)
        {
            
        }


    }
}