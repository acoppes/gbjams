using GBJAM10.Definitions;
using GBJAM10.Ecs;
using Gemserk.Leopotam.Ecs;
using Gemserk.Leopotam.Ecs.Controllers;
using Gemserk.Leopotam.Ecs.Gameplay;
using UnityEngine;
using UnityEngine.Serialization;

namespace GBJAM10.Controllers
{
    public class CharacterController : ControllerBase, IInit, IEntityDestroyed
    {
        private const string StateJumping = "Jumping";
        private const string StateFalling = "Falling";
        private const string StatePickingTrap = "PickingTrap";
        private const string StateSuperAttack = "SuperAttack";
    
        // Read this kind of things from configuration
        public float jumpMaxHeight;
        public float jumpSpeed = 1;
        public float fallSpeed = 1;

        public float slowExtraSpeed = -3;
        public float fastExtraSpeed = 3;

        public float autoAttackDelayAfterSuperAttack;

        [FormerlySerializedAs("bulletDefinition")] 
        public GameObject defaultBulletDefinition;

        public GameObject superBulletDefinition;
    
        private GameObject currentBulletDefinition;

        private Entity autoAttackBullet = Entity.NullEntity;
    
        public void OnInit()
        {
            currentBulletDefinition = defaultBulletDefinition;
        }
    
    
        public void OnEntityDestroyed(Entity e)
        {
            if (e == autoAttackBullet)
            {
                autoAttackBullet = Entity.NullEntity;
            }
        }

        private Entity FireBullet(GameObject bulletDefinition)
        {
            var playerComponent = world.GetComponent<PlayerComponent>(entity);
            var lookingDirection = world.GetComponent<LookingDirection>(entity);
            var position = world.GetComponent<PositionComponent>(entity);
        
            var model = world.GetComponent<UnitModelComponent>(entity);
            var attachBulletPosition = model.instance.transform.FindInHierarchy("Attach_Bullet").position;
            
            var bulletEntity = world.CreateEntity(bulletDefinition.GetInterface<IEntityDefinition>(), null);
            ref var bulletPosition = ref world.GetComponent<PositionComponent>(bulletEntity);
            
            ref var bulletPlayerComponent = ref world.GetComponent<PlayerComponent>(bulletEntity);
            bulletPlayerComponent.player = playerComponent.player;
            
            bulletPosition.value = attachBulletPosition;

            return bulletEntity;
        }

        public override void OnUpdate(float dt)
        {
            // if (world.HasComponent<PlayerInputComponent>(entity))
            //     return;
        
            ref var playerInput = ref world.GetComponent<PlayerInputComponent>(entity);
            ref var jumpComponent = ref world.GetComponent<JumpComponent>(entity);
            ref var playerComponent = ref world.GetComponent<PlayerComponent>(entity);
        
            playerInput.disabled = true;
        
            ref var movementComponent = ref world.GetComponent<UnitMovementComponent>(entity);
        
            ref var unitState = ref world.GetComponent<UnitStateComponent>(entity);
            unitState.disableAutoUpdate = true;
            unitState.walking = false;
        
            ref var states = ref world.GetComponent<StatesComponent>(entity);
            ref var control = ref world.GetComponent<UnitControlComponent>(entity);

            ref var abilities = ref world.GetComponent<AbilitiesComponent>(entity);
            var pickTrapAbility = abilities.GetAbility("PickTrap");
        
            var autoAttackAbility = abilities.GetAbility("AutoAttack");
            var superAttackAbility = abilities.GetAbility("SuperAttack");

            var position = world.GetComponent<PositionComponent>(entity);
            var lookingDirection = world.GetComponent<LookingDirection>(entity);
        
            var model = world.GetComponent<UnitModelComponent>(entity);
        
            control.direction.x = 1;
        
            if (playerInput.keyMap != null)
            {
                control.direction.y = playerInput.keyMap.direction.y;
            
                control.mainAction = playerInput.keyMap.button1Pressed;
                control.secondaryAction = playerInput.keyMap.button2Pressed;

                // if (control.mainAction)
                // {
                //     Debug.Log("JUST PRESSED");
                // }

                movementComponent.extraSpeed = 0;
            
                if (playerInput.keyMap.direction.x > 0)
                {
                    movementComponent.extraSpeed += fastExtraSpeed;
                } else if (playerInput.keyMap.direction.x < 0)
                {
                    movementComponent.extraSpeed += slowExtraSpeed;
                }
            }

            if (states.HasState(StatePickingTrap))
            {
                movementComponent.extraSpeed += slowExtraSpeed;
            
                // control.direction.x = 0;
            
                control.direction.y = 0;

                var stopAbility = pickTrapAbility.isComplete;

                var pickTrapsTargeting = abilities.GetTargeting("PickTrap");
                if (pickTrapsTargeting.targets.Count > 0)
                {
                    var target = pickTrapsTargeting.targets[0];
                    var unitTypeComponent = world.GetComponent<UnitTypeComponent>(target.entity);

                    if ((unitTypeComponent.type & (int) UnitDefinition.UnitType.Pickup) != 0)
                    {
                        ref var targetHealth = ref world.GetComponent<HealthComponent>(target.entity);
                        targetHealth.deathRequest = true;
                    
                        // pick special attack 
                    
                        // change normal state!! 
                        currentBulletDefinition = null;
                        states.EnterState(StateSuperAttack);

                        superAttackAbility.cooldownCurrent = 0;
                        
                        if (model.instance != null)
                        {
                            var sfxTransform = model.instance.transform.FindInHierarchy("Sfx_PickTrap");
                            if (sfxTransform != null)
                            {
                                sfxTransform.GetComponent<AudioSource>().Play();
                            }
                        }
                    }
                
                    // kill trap before damage
                    stopAbility = true;
                }

                if (stopAbility)
                {
                    unitState.attacking1 = false;
                
                    pickTrapAbility.isRunning = false;
                    states.ExitState(StatePickingTrap);
                }
            
                return;
            }

            // movementComponent.movingDirection = new Vector2(1, movementComponent.movingDirection.y);
            if (states.HasState(StateFalling))
            {
                // var state = states.GetState(StateFalling);
            
                control.direction.y = 0;
                jumpComponent.y -= dt * fallSpeed;
            
                if (jumpComponent.y <= 0)
                {
                    unitState.dashing = false;
                    states.ExitState(StateFalling);
                    jumpComponent.y = 0;
                }
            
                return;
            }

            // if (states.HasState(StateJumping))
            // {
            //     // var state = states.GetState(StateJumping);
            //
            //     control.direction.y = 0;
            //
            //     jumpComponent.y += dt * jumpSpeed;
            //
            //     if (jumpComponent.y >= jumpMaxHeight || !control.secondaryAction)
            //     {
            //         unitState.dashing = false;
            //         states.ExitState(StateJumping);
            //         states.EnterState(StateFalling);
            //     }
            //
            //     return;
            // }
        
            if (!states.HasState(StateSuperAttack))
            {
                if (control.mainAction && pickTrapAbility.isCooldownReady)
                {
                    pickTrapAbility.isRunning = true;

                    unitState.walking = false;

                    // control.direction.x = 0;

                    unitState.attacking1 = true;
                    states.EnterState(StatePickingTrap);

                    return;
                }
            }
            else
            {
                if (control.mainAction && superAttackAbility.isCooldownReady)
                {
                    FireBullet(superBulletDefinition);
                    states.ExitState(StateSuperAttack);
                    currentBulletDefinition = defaultBulletDefinition;

                    autoAttackAbility.cooldownCurrent = -autoAttackDelayAfterSuperAttack;
                    pickTrapAbility.cooldownCurrent = 0;
                }
            }

            // if (control.secondaryAction)
            // {
            //     // start jumping 
            //     jumpComponent.y = 0;
            //     unitState.dashing = true;
            //     states.EnterState(StateJumping);
            //     return;
            // }
        
            if (autoAttackAbility.isCooldownReady && currentBulletDefinition != null && autoAttackBullet == Entity.NullEntity)
            {
                autoAttackBullet = FireBullet(currentBulletDefinition);
                autoAttackAbility.cooldownCurrent = 0;
            }

            if (movementComponent.totalSpeed > 0)
            {
                unitState.walking = true;    
            }
        }

    }
}