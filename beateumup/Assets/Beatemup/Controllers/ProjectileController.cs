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
        public float ttlAfterFalling = 2.0f;
        
        private Vector3 startingPosition;
        
        public void OnInit()
        {
            ref var states = ref GetComponent<StatesComponent>();
            states.EnterState("Travel");
            
            ref var animationComponent = ref GetComponent<AnimationComponent>();
            animationComponent.Play("Idle");
        }
        
        public void OnEnterState()
        {
            ref var states = ref GetComponent<StatesComponent>();
            ref var position = ref GetComponent<PositionComponent>();
            ref var movement = ref GetComponent<HorizontalMovementComponent>();
            ref var gravity = ref GetComponent<GravityComponent>();
            ref var physicsComponent = ref GetComponent<PhysicsComponent>();
            ref var lookingDirection = ref GetComponent<LookingDirection>();
            
            if (states.statesEntered.Contains("Travel"))
            {
                gravity.disabled = true;

                // movement.speed = movement.baseSpeed;

                startingPosition = position.value;

                physicsComponent.disableCollideWithObstacles = false;

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
            ref var states = ref GetComponent<StatesComponent>();
            ref var lookingDirection = ref GetComponent<LookingDirection>();
            ref var physicsComponent = ref GetComponent<PhysicsComponent>();
            ref var position = ref GetComponent<PositionComponent>();
            ref var modelShakeComponent = ref GetComponent<ModelShakeComponent>();
            ref var movement = ref GetComponent<HorizontalMovementComponent>();
            
            State state;
            
            if (states.TryGetState("Death", out state))
            {
                movement.movingDirection = Vector2.zero;

                ref var destroyable = ref world.GetComponent<DestroyableComponent>(entity);
                destroyable.destroy = true;
                
                return;
            }
            
            if (states.TryGetState("Travel", out state))
            {
                if (world.HasComponent<PlayerInputComponent>(entity))
                {
                    var control = GetComponent<ControlComponent>();
                    var direction = control.direction3d;

                    if (direction.sqrMagnitude > 0.1f)
                    {
                        physicsComponent.body.velocity = direction * movement.baseSpeed;
                    }
                }
                
                var hitTargets = world.GetTargets(entity);

                foreach (var hitTarget in hitTargets)
                {
                    ref var hitComponent = ref world.GetComponent<HitPointsComponent>(hitTarget.entity);
                    hitComponent.hits.Add(new HitData
                    {
                        position = position.value,
                        knockback = true,
                        hitPoints = 1,
                        source = entity
                    });
                }
                
                var velocity = physicsComponent.velocity;
                
                if (velocity.sqrMagnitude > 0.1f)
                {
                    lookingDirection.value = velocity.normalized;
                }

                if (hitTargets.Count > 0)
                {
                    states.ExitState("Travel");
                    states.EnterState("Death");
                    return;
                }

                if (state.time > maxTravelTime)
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

                if (state.time > ttlAfterFalling)
                {
                    states.ExitState("Falling");
                    states.EnterState("Death");
                }
            }
            
        }


    }
}