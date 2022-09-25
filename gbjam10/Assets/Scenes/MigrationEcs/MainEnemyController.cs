using GBJAM10.Ecs;
using Gemserk.Leopotam.Ecs;
using Gemserk.Leopotam.Ecs.Controllers;
using Gemserk.Leopotam.Ecs.Gameplay;
using UnityEngine;

public class MainEnemyController : ControllerBase
{
    private const string SpawnBombState = "SpawningBomb";
    
    public GameObject bombDefinition;

    public Vector2 spawnBombOffset = new Vector2(-1, 0);

    public override void OnUpdate(float dt)
    {
        ref var states = ref world.GetComponent<StatesComponent>(entity);
        ref var unitStateComponent = ref world.GetComponent<UnitStateComponent>(entity);
        ref var control = ref world.GetComponent<UnitControlComponent>(entity);
        
        var playerComponent = world.GetComponent<PlayerComponent>(entity);
        
        ref var abilities = ref world.GetComponent<AbilitiesComponent>(entity);
        var plantTrapAbility = abilities.GetAbility("PlantTrap");

        unitStateComponent.disableAutoUpdate = true;
        unitStateComponent.walking = false;
        control.direction.x = 1;
        
        var position = world.GetComponent<PositionComponent>(entity);

        if (states.HasState(SpawnBombState))
        {
            var state = states.GetState(SpawnBombState);
            if (state.time > plantTrapAbility.duration)
            {
                var bombEntity = world.CreateEntity(bombDefinition.GetInterface<IEntityDefinition>(), null);
                ref var bombPosition = ref world.GetComponent<PositionComponent>(bombEntity);
                
                ref var bombPlayerComponent = ref world.GetComponent<PlayerComponent>(bombEntity);
                bombPlayerComponent.player = playerComponent.player;
                
                bombPosition.value = position.value + spawnBombOffset;

                plantTrapAbility.isRunning = false;
                plantTrapAbility.cooldownCurrent = 0;
                
                unitStateComponent.attacking1 = false;
                states.ExitState(SpawnBombState);
            }

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