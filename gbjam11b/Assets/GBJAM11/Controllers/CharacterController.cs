using System.Numerics;
using Game.Components;
using Game.Controllers;
using GBJAM11.Components;
using Gemserk.Leopotam.Ecs;
using Gemserk.Leopotam.Ecs.Components;
using Gemserk.Leopotam.Ecs.Controllers;
using Gemserk.Leopotam.Ecs.Events;
using MyBox;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;

namespace GBJAM11.Controllers
{
    public class CharacterController : ControllerBase, IUpdate, IActiveController
    {
        public bool CanBeInterrupted(Entity entity, IActiveController activeController)
        {
            throw new System.NotImplementedException();
        }

        public void OnInterrupt(Entity entity, IActiveController activeController)
        {
            throw new System.NotImplementedException();
        }
        
        public void OnUpdate(World world, Entity entity, float dt)
        {
            ref var states = ref entity.Get<StatesComponent>();
            ref var input = ref entity.Get<InputComponent>();
            ref var bufferedInput = ref entity.Get<BufferedInputComponent>();
            ref var animations = ref entity.Get<AnimationComponent>();
            ref var weapons = ref entity.Get<WeaponsComponent>();
            ref var movement = ref entity.Get<MovementComponent>();
            // if attacking 
            // fire attack
            
            if (states.TryGetState("Rolling", out var rollingState))
            {
                if (animations.IsPlaying("Roll") && rollingState.time > entity.Get<RollComponent>().duration)
                {
                    movement.speedMultiplier = entity.Get<RollComponent>().speedMultiplierGround;
                    animations.Play("RollEnd", 0);
                    return;
                }
                
                if (animations.IsPlaying("RollEnd") && animations.isCompleted)
                {
                    ExitRoll(entity);
                }

                return;
            }

            if (states.TryGetState("ChargingAttack", out var chargingState))
            {
                // if (input.direction().vector2.SqrMagnitude() > 0)
                // {
                //     weapons.weaponEntity.Get<LookingDirection>().value = input.direction().vector2;
                //     entity.Get<LookingDirection>().value = input.direction().vector2;
                // }
                
                if (!input.button1().isPressed)
                {
                    // enter attack
                    animations.Play("Attack", 0);
                    
                    FireProjectile(world, entity);
                    
                    states.ExitState("ChargingAttack");
                    states.EnterState("Attacking");
                }

                return;
            }
            
            if (states.TryGetState("Attacking", out var attackState))
            {
                if (animations.IsPlaying("Attack") && animations.isCompleted)
                {
                    ExitAttack(entity);
                }

                return;
            }
            
            movement.movingDirection = input.direction().vector2;
            
            if (input.direction().vector2.SqrMagnitude() > 0)
            {
                entity.Get<LookingDirection>().value = input.direction().vector2;
            }
            
            if (Mathf.Abs(input.direction().vector2.x) > 0)
            {
                weapons.weaponEntity.Get<LookingDirection>().value = input.direction().vector2.SetY(0);
            }
            
            if (bufferedInput.HasBufferedAction(input.button1()))
            {
                EnterAttack(world, entity);
                return;
            }
            
            if (bufferedInput.HasBufferedAction(input.button2()))
            {
                EnterRoll(world, entity);
                return;
            }
        }

        private void FireProjectile(World world, Entity entity)
        {
            ref var weapons = ref entity.Get<WeaponsComponent>();
            
            var weaponEntity = weapons.weaponEntity;

            var projectileEntity = world.CreateEntity(weaponEntity.Get<WeaponComponent>().projectileDefinition);
            projectileEntity.Get<PositionComponent>().value = weaponEntity.Get<PositionComponent>().value;
                    
            ref var projectile = ref projectileEntity.Get<ProjectileComponent>();
            projectile.initialVelocity = weaponEntity.Get<LookingDirection>().value;

            projectileEntity.Get<PlayerComponent>().player = entity.Get<PlayerComponent>().player;

            // weapons.lastFiredProjectile = projectileEntity;
                   
            weapons.weaponEntity.Get<WeaponComponent>().charging = false;
        }
        
        private void EnterAttack(World world, Entity entity)
        {
            // start anim, start state, etc...
            // lock looking direction, movement, etc..
            
            ref var states = ref entity.Get<StatesComponent>();
            ref var animations = ref entity.Get<AnimationComponent>();
            ref var activeController = ref entity.Get<ActiveControllerComponent>();
            ref var movement = ref entity.Get<MovementComponent>();
            ref var input = ref entity.Get<InputComponent>();
          
            
            activeController.TakeControl(entity, this);
            movement.speed = 0;
            
            // animations.Play("Charge");
            // states.EnterState("ChargingAttack");

            animations.Play("Attack", 0);
            states.EnterState("Attacking");
            
            FireProjectile(world, entity);
            
            // weapons.weaponEntity.Get<WeaponComponent>().charging = true;
            //
            // if (input.direction().vector2.SqrMagnitude() > 0)
            // {
            //     weapons.weaponEntity.Get<LookingDirection>().value = input.direction().vector2;
            // }
            // else
            // {
            //     weapons.weaponEntity.Get<LookingDirection>().value = entity.Get<LookingDirection>().value;
            // }

            entity.Get<Physics2dComponent>().body.velocity = Vector2.zero;
        }

        private void ExitAttack(Entity entity)
        {
            // exit state, stop anim, etc...
            
            ref var states = ref entity.Get<StatesComponent>();
            ref var activeController = ref entity.Get<ActiveControllerComponent>();
            ref var movement = ref entity.Get<MovementComponent>();

            activeController.ReleaseControl(this);
            movement.speed = movement.baseSpeed;
            
            states.ExitState("Attacking");
        }
        
        private void EnterRoll(World world, Entity entity)
        {
            // start anim, start state, etc...
            // lock looking direction, movement, etc..
            
            ref var states = ref entity.Get<StatesComponent>();
            ref var animations = ref entity.Get<AnimationComponent>();
            ref var activeController = ref entity.Get<ActiveControllerComponent>();
            ref var movement = ref entity.Get<MovementComponent>();
            ref var input = ref entity.Get<InputComponent>();
            
            activeController.TakeControl(entity, this);
            movement.speedMultiplier = entity.Get<RollComponent>().speedMultiplierAir;

            entity.Get<AutoAnimationComponent>().disabled = true;

            animations.Play("Roll", 0);
            states.EnterState("Rolling");
            
            // disable dynamic collisions!!

            entity.Get<Physics2dComponent>().body.transform.Find("Collider").gameObject.SetActive(false);
            
            movement.movingDirection = entity.Get<LookingDirection>().value;
        }

        private void ExitRoll(Entity entity)
        {
            // exit state, stop anim, etc...
            
            ref var states = ref entity.Get<StatesComponent>();
            ref var activeController = ref entity.Get<ActiveControllerComponent>();
            ref var movement = ref entity.Get<MovementComponent>();

            activeController.ReleaseControl(this);
            movement.speedMultiplier = 1;
            
            entity.Get<AutoAnimationComponent>().disabled = false;
            
            entity.Get<Physics2dComponent>().body.transform.Find("Collider").gameObject.SetActive(true);
            
            states.ExitState("Rolling");
        }
    }
}