using Game.Components;
using Game.Controllers;
using Game.Queries;
using Game.Utilities;
using GBJAM11.Components;
using Gemserk.Leopotam.Ecs;
using Gemserk.Leopotam.Ecs.Components;
using Gemserk.Leopotam.Ecs.Controllers;
using Gemserk.Leopotam.Ecs.Events;
using Gemserk.Triggers.Queries;
using Gemserk.Utilities;
using MyBox;
using UnityEngine;
using JumpComponent = GBJAM11.Components.JumpComponent;

namespace GBJAM11.Controllers
{
    public class MainCharacterController : ControllerBase, IUpdate, IActiveController
    {
        // public void OnEntityCollision(World world, Entity entity, IEntityCollisionDelegate.EntityCollision entityCollision)
        // {
        //     if (!entityCollision.isTrigger)
        //     {
        //         var contact = entityCollision.collision2D.contacts[0];
        //         if (contact.normal.y < -0.9f)
        //         {
        //             Debug.Log("ROOF");
        //             EnterOnRoof(entity, contact.point);
        //         }
        //     }
        //         
        // }
        
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
            ref var weapons = ref entity.Get<WeaponsComponent>();
            
            ref var lookingDirection = ref entity.Get<LookingDirection>();
            // if attacking 
            // fire attack
            ref var jumpComponent = ref entity.Get<JumpComponent>();

            ref var movement = ref entity.Get<MovementComponent>();
            movement.movingDirection = input.direction().vector2.SetY(0);
            
            ref var physics = ref entity.Get<Physics2dComponent>();
            ref var gravity = ref entity.Get<GravityComponent>();
            
            if (states.TryGetState("OnRoof", out var roofState))
            {
                if (physics.contacts.Count == 0)
                {
                    // Debug.Log("Should fall");
                    entity.Get<PositionComponent>().value -= new Vector3(0, 0.5f, 0);
                    ExitOnRoof(entity);
                    EnterFalling(entity);
                }
            }
            
            if (states.TryGetState("Teleporting", out var teleportState))
            {
                if (animations.IsPlaying("Teleport") && animations.isCompleted)
                {
                    ExitTeleport(entity);
                }

                return;
            }

            
            if (states.TryGetState("ChargingAttack", out var chargingState))
            {
                if (input.direction().vector2.SqrMagnitude() > 0)
                {
                    weapons.weaponEntity.Get<LookingDirection>().value = input.direction().vector2.normalized.FixToAngles(45);
                    lookingDirection.value = input.direction().vector2.normalized;
                }
                
                if (!input.button1().isPressed)
                {
                    // enter attack
                    animations.Play("Attack", 0);
                    states.ExitState("ChargingAttack");
                    states.EnterState("Attacking");
                }

                return;
            }
            
            if (states.TryGetState("Attacking", out var attackState))
            {
                if (animations.IsPlaying("Attack") && animations.isCompleted)
                {
                    var weaponEntity = weapons.weaponEntity;

                    var projectileEntity = world.CreateEntity(weaponEntity.Get<WeaponComponent>().projectileDefinition);
                    projectileEntity.Get<PositionComponent>().value = weaponEntity.Get<PositionComponent>().value;
                    
                    ref var projectile = ref projectileEntity.Get<ProjectileComponent>();
                    projectile.initialVelocity = weaponEntity.Get<LookingDirection>().value;

                    projectileEntity.Get<PlayerComponent>().player = entity.Get<PlayerComponent>().player;

                   // weapons.lastFiredProjectile = projectileEntity;
                   
                    weapons.weaponEntity.Get<WeaponComponent>().charging = false;
                    
                    ExitAttack(entity);
                }

                return;
            }
            
            if (input.direction().vector2.SqrMagnitude() > 0)
            {
                weapons.weaponEntity.Get<LookingDirection>().value = input.direction().vector2.normalized.FixToAngles(45);
                lookingDirection.value = input.direction().vector2.normalized;
            }
            
            if (states.HasState("Falling"))
            {
                if (gravity.inContactWithGround || states.HasState("OnRoof"))
                {
                    entity.Get<AutoAnimationComponent>().disabled = false;
                    states.ExitState("Falling");
                }
                else
                {
                    if (!animations.IsPlaying("Fall"))
                    {
                        animations.Play("Fall");
                    }
                }
            }
            
            var teleportKunaiList = world.GetEntities(new EntityQuery(new TypesParameter("teleport_kunai")));

            if (bufferedInput.HasBufferedAction(input.button1()))
            {
                if (teleportKunaiList.Count > 0)
                {
                    bufferedInput.ConsumeBuffer();
                    EnterTeleport(entity, teleportKunaiList[0]);
                    return;
                }
                else
                {
                    bufferedInput.ConsumeBuffer();
                    EnterAttack(entity);
                    return;
                }

            }
            
            if (states.TryGetState("Jumping", out var jumpingState))
            {
                if (jumpComponent.state == JumpComponent.State.Starting)
                {
                    if (jumpingState.updateCount >= jumpComponent.durationInFrames || !input.button2().isPressed)
                    {
                        gravity.disabled = false;
                        physics.body.drag = jumpComponent.tempDrag;

                        jumpComponent.state = JumpComponent.State.Ending;

                        jumpComponent.tempDrag = 0;
                        return;
                    }
                }

                if (jumpComponent.state == JumpComponent.State.Ending)
                {
                    if (physics.body.velocity.y < 0)
                    {
                        ExitJumping(entity);
                        EnterFalling(entity);
                    }
                }

                return;
            }
            
            if (!states.HasState("Falling") && !states.HasState("OnRoof")  && !states.HasState("WallStick"))
            {
                // and not on roof either!!
                if (!gravity.inContactWithGround)
                {
                    EnterFalling(entity);
                }
            }
            
            if (bufferedInput.HasBufferedAction(input.button2()))
            {
                if (states.HasState("OnRoof"))
                {
                    ExitOnRoof(entity);
                    entity.Get<PositionComponent>().value -= new Vector3(0, 0.5f, 0);
                    return;
                } else if (states.HasState("WallStick"))
                {
                    ExitWallStick(entity);
                    return;
                }
                else if (jumpComponent.jumps < jumpComponent.totalJumps)
                {
                    bufferedInput.ConsumeBuffer();
                    EnterJumping(entity);
                    return;
                    // jump
                }
            }

            if (teleportKunaiList.Count > 0)
            {
                var teleportKunai = teleportKunaiList[0];
                
                // it is the kunai in the wall
                if (!teleportKunai.Has<Physics2dComponent>())
                {
                    EnterTeleport(entity, teleportKunaiList[0]);
                    return;
                }
            }

            if (gravity.inContactWithGround)
            {
                jumpComponent.jumps = 0;
            }
        }

        private void EnterJumping(Entity entity)
        {
            ref var states = ref entity.Get<StatesComponent>();
            ref var animations = ref entity.Get<AnimationComponent>();
            ref var activeController = ref entity.Get<ActiveControllerComponent>();
            ref var jumpComponent = ref entity.Get<JumpComponent>();
            
            activeController.TakeControl(entity, this);
            
            animations.Play("Jumping");
            states.EnterState("Jumping");

            jumpComponent.state = JumpComponent.State.Starting;

            entity.Get<GravityComponent>().disabled = true;
            entity.Get<Physics2dComponent>().body.velocity = new Vector2(0, jumpComponent.initialSpeed);

            jumpComponent.tempDrag = entity.Get<Physics2dComponent>().body.drag;
            entity.Get<Physics2dComponent>().body.drag = 0;

            entity.Get<AutoAnimationComponent>().disabled = true;

            jumpComponent.jumps++;
        }

        private void ExitJumping(Entity entity)
        {
            ref var states = ref entity.Get<StatesComponent>();
            ref var activeController = ref entity.Get<ActiveControllerComponent>();
            ref var jumpComponent = ref entity.Get<JumpComponent>();
            
            activeController.ReleaseControl(this);
            states.ExitState("Jumping");

            jumpComponent.state = JumpComponent.State.None;
            
            entity.Get<AutoAnimationComponent>().disabled = false;
        }

        private void EnterFalling(Entity entity)
        {
            ref var states = ref entity.Get<StatesComponent>();
            ref var animations = ref entity.Get<AnimationComponent>();
            
            // enter falling
            animations.Play("Fall");
            entity.Get<AutoAnimationComponent>().disabled = true;
                    
            states.EnterState("Falling");
        }
        
        private void ExitFalling(Entity entity)
        {
            ref var states = ref entity.Get<StatesComponent>();
            entity.Get<AutoAnimationComponent>().disabled = false;
            states.ExitState("Falling");
        }

        private void EnterOnRoof(Entity entity, Vector2 position)
        {
            entity.Get<StatesComponent>().EnterState("OnRoof");
            entity.Get<GravityComponent>().disabled = true;
            entity.Get<Physics2dComponent>().body.velocity = Vector2.zero;
            // force position to touch
            entity.Get<MovementComponent>().speedMultiplier = 0.85f;

            entity.Get<Physics2dComponent>().body.transform.localScale = new Vector3(1, -1, 1);
            entity.Get<ModelComponent>().instance.spriteRenderer.flipY = true;

            entity.Get<Physics2dComponent>().body.position = position;

            entity.Get<WeaponsComponent>().inverted = true;
        }
        
        private void EnterWallStick(Entity entity, Vector2 position)
        {
            ref var states = ref entity.Get<StatesComponent>();
            
            if (states.HasState("Falling"))
            {
                ExitFalling(entity);
            }
            
            states.EnterState("WallStick");
            
            entity.Get<GravityComponent>().disabled = true;
            entity.Get<Physics2dComponent>().body.velocity = Vector2.zero;
            // force position to touch
            entity.Get<MovementComponent>().speedMultiplier = 0;
            entity.Get<PositionComponent>().value = position;

            entity.Get<JumpComponent>().jumps = 0;
        }

        private void ExitOnRoof(Entity entity)
        {
            entity.Get<StatesComponent>().ExitState("OnRoof");
            entity.Get<GravityComponent>().disabled = false;
            entity.Get<MovementComponent>().speedMultiplier = 1.0f;
            
            entity.Get<Physics2dComponent>().body.transform.localScale = new Vector3(1, 1, 1);
            entity.Get<ModelComponent>().instance.spriteRenderer.flipY = false;
            
            entity.Get<WeaponsComponent>().inverted = false;
        }
        
        private void ExitWallStick(Entity entity)
        {
            entity.Get<StatesComponent>().ExitState("WallStick");
            entity.Get<GravityComponent>().disabled = false;
            entity.Get<MovementComponent>().speedMultiplier = 1.0f;
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

            weapons.weaponEntity.Get<WeaponComponent>().charging = true;
            
            if (input.direction().vector2.SqrMagnitude() > 0)
            {
                weapons.weaponEntity.Get<LookingDirection>().value = input.direction().vector2;
            }
            else
            {
                weapons.weaponEntity.Get<LookingDirection>().value = entity.Get<LookingDirection>().value;
            }

            entity.Get<GravityComponent>().disabled = true;
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
            
            if (!states.HasState("OnRoof") && !states.HasState("WallStick"))
            {
                entity.Get<GravityComponent>().disabled = false;
            }
        }

        private void EnterTeleport(Entity entity, Entity kunaiEntity)
        {
            ref var states = ref entity.Get<StatesComponent>();
            ref var animations = ref entity.Get<AnimationComponent>();
            ref var activeController = ref entity.Get<ActiveControllerComponent>();
            ref var movement = ref entity.Get<MovementComponent>();
            ref var weapons = ref entity.Get<WeaponsComponent>();
            
            ExitOnRoof(entity);
            
            activeController.TakeControl(entity, this);
            movement.speed = 0;
            
            animations.Play("Teleport", 0);
            states.EnterState("Teleporting");
            
            // spawn teleport particle in position

            var kunaiComponent = kunaiEntity.Get<KunaiComponent>();
            if (kunaiComponent.stuckEntity.Exists())
            {
                // swap places!!
                kunaiComponent.stuckEntity.Get<PositionComponent>().value = entity.Get<PositionComponent>().value;
            }
            
            var teleportPosition = kunaiEntity.Get<PositionComponent>().value;
            // teleportPosition.y = 0;
            entity.Get<PositionComponent>().value = teleportPosition;

            kunaiEntity.Get<DestroyableComponent>().destroy = true;
            // weapons.lastFiredProjectile = Entity.NullEntity;
            
            entity.Get<Physics2dComponent>().body.velocity = Vector2.zero;

            if (kunaiComponent.ceilingCollision)
            {
                EnterOnRoof(entity, kunaiEntity.Get<PositionComponent>().value);
            } else if (kunaiComponent.wallCollision)
            {
                EnterWallStick(entity, kunaiEntity.Get<PositionComponent>().value);
            }
        }

        private void ExitTeleport(Entity entity)
        {
            ref var states = ref entity.Get<StatesComponent>();
            ref var activeController = ref entity.Get<ActiveControllerComponent>();
            ref var movement = ref entity.Get<MovementComponent>();
            
            activeController.ReleaseControl(this);
            movement.speed = movement.baseSpeed;
            states.ExitState("Teleporting");
        }



    }
}