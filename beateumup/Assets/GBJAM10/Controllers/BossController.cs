using System.Collections.Generic;
using GBJAM10.Definitions;
using GBJAM10.Ecs;
using Gemserk.Leopotam.Ecs;
using Gemserk.Leopotam.Ecs.Controllers;
using Gemserk.Leopotam.Ecs.Gameplay;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

namespace GBJAM10.Controllers
{
    public class BossController : ControllerBase
    {
        private const string SpawnBombState = "SpawningBomb";
        private const string SwitchingPositionState = "SwitchingPosition";
        private const string BurstAttackState = "BurstAttack";
        
        public List<GameObject> plantDefinitions;

        private float switchPositionDestinationY;
    
        public Vector2 spawnBombOffset = new Vector2(-1, 0);

        public float switchPositionsRandomCooldown = 0.5f;
        public float spawnBombRandomCooldown = 0.5f;

        public GameObject burstAttackBulletDefinition;
        public int burstAttackMinBullets = 3;
        public int burstAttackMaxBullets = 5;

        private int burstAttackPendingBullets = 0;

        public float damageNear = 1;
        
        private Entity FireBullet(GameObject bulletDefinition)
        {
            var playerComponent = world.GetComponent<PlayerComponent>(entity);
        
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
            ref var states = ref world.GetComponent<StatesComponent>(entity);
            ref var unitStateComponent = ref world.GetComponent<UnitStateComponent>(entity);
            ref var control = ref world.GetComponent<UnitControlComponent>(entity);
        
            var playerComponent = world.GetComponent<PlayerComponent>(entity);
        
            ref var abilities = ref world.GetComponent<AbilitiesComponent>(entity);
            
            var plantTrapAbility = abilities.GetAbility("PlantTrap");
            var switchPositionAbility = abilities.GetAbility("SwitchPosition");
            var burstAttackAbility = abilities.GetAbility("BurstAttack");

            unitStateComponent.disableAutoUpdate = true;
            unitStateComponent.walking = false;
        
            control.direction.x = 1;
            control.direction.y = 0;
        
            var position = world.GetComponent<PositionComponent>(entity);

            var damageNearAbility = abilities.GetAbility("DamageNear");
            var damageNearTargeting = abilities.GetTargeting("DamageNear");

            if (damageNearAbility.isCooldownReady)
            {
                foreach (var target in damageNearTargeting.targets)
                {
                    if (target.entity == Entity.NullEntity)
                        continue;

                    if (world.HasComponent<UnitTypeComponent>(target.entity))
                    {
                        var unitTypeComponent = world.GetComponent<UnitTypeComponent>(target.entity);
                        if ((unitTypeComponent.type & (int)UnitDefinition.UnitType.Unit) == 0)
                        {
                            continue;
                        }
                    }

                    ref var targetHealth = ref world.GetComponent<HealthComponent>(target.entity);
                    targetHealth.pendingDamages.Add(new Damage
                    {
                        value = damageNear
                    });
                    
                    // reset cooldown only if damage player
                    damageNearAbility.cooldownCurrent = 0;
                }
            }
        
            if (states.HasState(BurstAttackState))
            {
                var state = states.GetState(BurstAttackState);
            
                unitStateComponent.walking = true;

                // fire bullets!!
                
                if (burstAttackAbility.isComplete)
                {
                    FireBullet(burstAttackBulletDefinition);
                    
                    burstAttackPendingBullets--;
                    
                    if (burstAttackPendingBullets <= 0)
                    {
                        states.ExitState(BurstAttackState);
                        burstAttackAbility.isRunning = false;
                        burstAttackAbility.cooldownCurrent =
                            UnityEngine.Random.Range(-switchPositionsRandomCooldown, 0);
                    }
                    else
                    {
                        burstAttackAbility.runningTime = 0;
                    }
                }
            }
            
            if (states.HasState(SwitchingPositionState))
            {
                var state = states.GetState(SwitchingPositionState);
            
                unitStateComponent.walking = true;

                control.direction.y = Mathf.Sign(switchPositionDestinationY - position.value.y);

                if (Mathf.Abs(switchPositionDestinationY - position.value.y) < 0.1f || 
                    state.time > switchPositionAbility.duration)
                {
                    switchPositionAbility.isRunning = false;
                    switchPositionAbility.cooldownCurrent = UnityEngine.Random.Range(-switchPositionsRandomCooldown, 0);
                    states.ExitState(SwitchingPositionState);
                }
            
                return;
            }

            if (states.HasState(SpawnBombState))
            {
                var state = states.GetState(SpawnBombState);
                if (state.time > plantTrapAbility.duration)
                {
                    var plantDefinition = plantDefinitions[UnityEngine.Random.Range(0, plantDefinitions.Count)];
                    var plantEntity = world.CreateEntity(plantDefinition.GetInterface<IEntityDefinition>(), null);
                    ref var plantPosition = ref world.GetComponent<PositionComponent>(plantEntity);
                
                    ref var plantPlayer = ref world.GetComponent<PlayerComponent>(plantEntity);
                    plantPlayer.player = playerComponent.player;
                
                    plantPosition.value = position.value + spawnBombOffset;

                    plantTrapAbility.isRunning = false;
                    plantTrapAbility.cooldownCurrent = UnityEngine.Random.Range(-spawnBombRandomCooldown, 0);
                
                    unitStateComponent.attacking1 = false;
                    states.ExitState(SpawnBombState);
                }

                return;
            }

            if (switchPositionAbility.isCooldownReady)
            {
                switchPositionDestinationY = UnityEngine.Random.Range(-3.0f, 3.0f);
            
                switchPositionAbility.isRunning = true;
                unitStateComponent.walking = true;
                states.EnterState(SwitchingPositionState);
                return;
            }
        
            if (plantTrapAbility.isCooldownReady)
            {
                plantTrapAbility.isRunning = true;
                states.EnterState(SpawnBombState);
                // control.direction.x = 0;
                unitStateComponent.attacking1 = true;
                return;
            }

            if (!states.HasState(BurstAttackState))
            {
                if (burstAttackAbility.isCooldownReady)
                {
                    burstAttackPendingBullets = UnityEngine.Random.Range(burstAttackMinBullets, burstAttackMaxBullets);
                    burstAttackAbility.isRunning = true;
                    states.EnterState(BurstAttackState);
                    return;
                }
            }
            
            unitStateComponent.walking = true;
        }
    }
}