using Beatemup.Definitions;
using Beatemup.Ecs;
using Gemserk.Leopotam.Ecs.Gameplay;
using Gemserk.Leopotam.Gameplay.Controllers;
using Gemserk.Leopotam.Gameplay.Events;
using UnityEngine;
using LookingDirection = Beatemup.Ecs.LookingDirection;

namespace Beatemup.Controllers
{
    public class ProjectileController : ControllerBase, IInit, IUpdate, IStateChanged
    {
        public float maxTravelTime;
        
        private float travelTime;
        private float controlTravelY = 0;
        
        public float deathDuration = 1.0f;
        
        public CameraShakeAsset hitCameraShakeAsset;

        public void OnInit()
        {
            ref var states = ref Get<StatesComponent>();
            states.EnterState("Travel");
            
            ref var animationComponent = ref Get<AnimationComponent>();
            animationComponent.Play("Idle");
        }
        
        public void OnEnterState()
        {
            ref var states = ref Get<StatesComponent>();
            ref var position = ref Get<PositionComponent>();
            ref var movement = ref Get<HorizontalMovementComponent>();
            ref var gravity = ref Get<GravityComponent>();
            ref var physicsComponent = ref Get<PhysicsComponent>();
            ref var lookingDirection = ref Get<LookingDirection>();
            ref var modelComponent = ref Get<UnitModelComponent>();
            
            if (states.statesEntered.Contains("Death"))
            {
                gravity.disabled = true;
                physicsComponent.syncType = PhysicsComponent.SyncType.Both;
                physicsComponent.disableCollideWithObstacles = true;
                physicsComponent.body.constraints = RigidbodyConstraints.FreezeAll;
                
                modelComponent.color = new Color(0.75f, 0.75f, 0.75f, 1.0f);
            }

            if (states.statesEntered.Contains("Travel"))
            {
                travelTime = 0;
                
                gravity.disabled = true;

                physicsComponent.disableCollideWithObstacles = true;

                var direction = lookingDirection.value;
                var velocity = direction * movement.baseSpeed;
                
                physicsComponent.syncType = PhysicsComponent.SyncType.FromPhysics;
                physicsComponent.body.position = position.value;
                physicsComponent.body.velocity = velocity;

                physicsComponent.body.constraints = RigidbodyConstraints.None;
            }
            
            if (states.statesEntered.Contains("Falling"))
            {
                gravity.disabled = false;
                physicsComponent.syncType = PhysicsComponent.SyncType.FromPhysics;
                
            
                
                // movement.speed = 0;

                // physicsComponent.body.velocity = new Vector3(movement.currentVelocity.x, 0, movement.currentVelocity.y);
            }
        }

        public void OnExitState()
        {

        }
        
        public void OnUpdate(float dt)
        {
            ref var states = ref Get<StatesComponent>();
            ref var lookingDirection = ref Get<LookingDirection>();
            ref var physicsComponent = ref Get<PhysicsComponent>();
            ref var position = ref Get<PositionComponent>();
            ref var movement = ref Get<HorizontalMovementComponent>();
            
            ref var modelComponent= ref Get<UnitModelComponent>();
            ref var cameraShakeProvider= ref Get<CameraShakeProvider>();
            
            State state;
            
            if (states.TryGetState("Death", out state))
            {
                movement.movingDirection = Vector2.zero;
                
                if (deathDuration > 0)
                {
                    var color = modelComponent.color;
                    color.a = 1.0f - (state.time / deathDuration);
                    modelComponent.color = color;
                }
                
                if (state.time > deathDuration)
                {
                    ref var destroyable = ref Get<DestroyableComponent>();
                    destroyable.destroy = true;
                }
                
                return;
            }
            
            if (states.TryGetState("Travel", out state))
            {
                travelTime += dt;
                
                if (Has<PlayerInputComponent>())
                {
                    var control = Get<ControlComponent>();
                    var direction = control.direction3d;
                    
                    if (control.button1.isPressed)
                    {
                        controlTravelY += 0.1f * dt;
                    } else if (control.button2.isPressed)
                    {
                        controlTravelY -= 0.1f * dt;
                    }
                    
                    direction.y = controlTravelY;
                    
                    if (direction.sqrMagnitude > 0.1f)
                    {
                        physicsComponent.body.velocity = direction * movement.baseSpeed;
                    }

                    travelTime = 0;
                }
                
                var velocity = physicsComponent.velocity;
                
                if (velocity.sqrMagnitude > 0.1f)
                {
                    lookingDirection.value = velocity.normalized;
                }
                
                if (Has<HitBoxComponent>())
                {
                    var hitTargets = world.GetTargets(entity);

                    foreach (var hitTarget in hitTargets)
                    {
                        ref var hitComponent = ref world.GetComponent<HitPointsComponent>(hitTarget.entity);
                        hitComponent.hits.Add(new HitData
                        {
                            position = position.value,
                            knockback = false,
                            hitPoints = 1,
                            source = entity
                        });
                    }
                    
                    if (hitTargets.Count > 0)
                    {
                        cameraShakeProvider.AddShake(hitCameraShakeAsset);

                        physicsComponent.body.velocity *= -0.25f;
                    
                        states.ExitState("Travel");
                        states.EnterState("Falling");
                        return;
                    }
                }
                
                if (travelTime > maxTravelTime)
                {
                    states.ExitState("Travel");
                    states.EnterState("Falling");
                    return;
                }

                return;
            }
            
            if (states.TryGetState("Falling", out state))
            {
                // movement.movingDirection = lookingDirection.value;

                var velocity = physicsComponent.velocity;
                // var direction = new Vector2(velocity.x, velocity.z);
                
                if (velocity.sqrMagnitude > 0.1f)
                {
                    lookingDirection.value = velocity.normalized;
                }
                else
                {
                    states.ExitState("Falling");
                    states.EnterState("Death");
                }

                // if (state.time > ttlAfterFalling)
                // {
                //
                // }
            }
            
        }


    }
}