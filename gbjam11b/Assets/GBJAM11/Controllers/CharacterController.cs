using System.Numerics;
using Game.Components;
using Game.Controllers;
using GBJAM11.Components;
using Gemserk.Leopotam.Ecs;
using Gemserk.Leopotam.Ecs.Components;
using Gemserk.Leopotam.Ecs.Controllers;
using Gemserk.Leopotam.Ecs.Events;
using MyBox;
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
            
            // if attacking 
            // fire attack

            ref var movement = ref entity.Get<MovementComponent>();
            movement.movingDirection = input.direction().vector2;

            if (states.TryGetState("ChargingAttack", out var chargingState))
            {
                if (input.direction().vector2.SqrMagnitude() > 0)
                {
                    weapons.weaponEntity.Get<LookingDirection>().value = input.direction().vector2;
                    entity.Get<LookingDirection>().value = input.direction().vector2;
                }
                
                if (!input.button1().isPressed)
                {
                    // enter attack
                    animations.Play("Attack", 0);
                    
                    var weaponEntity = weapons.weaponEntity;

                    var projectileEntity = world.CreateEntity(weaponEntity.Get<WeaponComponent>().projectileDefinition);
                    projectileEntity.Get<PositionComponent>().value = weaponEntity.Get<PositionComponent>().value;
                    
                    ref var projectile = ref projectileEntity.Get<ProjectileComponent>();
                    projectile.initialVelocity = weaponEntity.Get<LookingDirection>().value;

                    projectileEntity.Get<PlayerComponent>().player = entity.Get<PlayerComponent>().player;

                    // weapons.lastFiredProjectile = projectileEntity;
                   
                    weapons.weaponEntity.Get<WeaponComponent>().charging = false;
                    
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
            
            if (bufferedInput.HasBufferedAction(input.button1()))
            {
                EnterAttack(entity);
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
            ref var input = ref entity.Get<InputComponent>();
            ref var weapons = ref entity.Get<WeaponsComponent>();
            
            activeController.TakeControl(entity, this);
            movement.speed = 0;
            
            animations.Play("Charge");
            states.EnterState("ChargingAttack");

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
    }
}