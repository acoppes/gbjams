using System.Collections.Generic;
using GBJAM10.Ecs;
using Gemserk.Leopotam.Ecs;
using Gemserk.Leopotam.Ecs.Controllers;
using Gemserk.Leopotam.Ecs.Gameplay;
using UnityEngine;

public class MainEnemyController : ControllerBase
{
    private const string SpawnBombState = "SpawningBomb";
    private const string SwitchingPositionState = "SwitchingPosition";
    
    public List<GameObject> plantDefinitions;

    private float switchPositionDestinationY;
    
    public Vector2 spawnBombOffset = new Vector2(-1, 0);

    public float switchPositionsRandomCooldown = 0.5f;
    public float spawnBombRandomCooldown = 0.5f;

    public override void OnUpdate(float dt)
    {
        ref var states = ref world.GetComponent<StatesComponent>(entity);
        ref var unitStateComponent = ref world.GetComponent<UnitStateComponent>(entity);
        ref var control = ref world.GetComponent<UnitControlComponent>(entity);
        
        var playerComponent = world.GetComponent<PlayerComponent>(entity);
        
        ref var abilities = ref world.GetComponent<AbilitiesComponent>(entity);
        var plantTrapAbility = abilities.GetAbility("PlantTrap");
        
        var switchPositionAbility = abilities.GetAbility("SwitchPosition");

        unitStateComponent.disableAutoUpdate = true;
        unitStateComponent.walking = false;
        
        control.direction.x = 1;
        control.direction.y = 0;
        
        var position = world.GetComponent<PositionComponent>(entity);
        
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
        
        unitStateComponent.walking = true;
    }
}