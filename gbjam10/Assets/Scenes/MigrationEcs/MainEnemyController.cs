using GBJAM10.Ecs;
using Gemserk.Leopotam.Ecs;
using Gemserk.Leopotam.Ecs.Controllers;
using Gemserk.Leopotam.Ecs.Gameplay;
using UnityEngine;

public class MainEnemyController : ControllerBase
{
    private const string SpawnBombState = "SpawningBomb";
    
    public GameObject bombDefinition;

    public float spawnBombCooldown;
    public float spawnBombDuration;
    
    public Vector2 spawnBombOffset = new Vector2(-1, 0);
    private float spawnBombTime;
    
    public override void OnUpdate(float dt)
    {
        ref var states = ref world.GetComponent<StatesComponent>(entity);
        ref var unitStateComponent = ref world.GetComponent<UnitStateComponent>(entity);
        ref var control = ref world.GetComponent<UnitControlComponent>(entity);
        
        var playerComponent = world.GetComponent<PlayerComponent>(entity);

        unitStateComponent.disableAutoUpdate = true;
        unitStateComponent.walking = false;
        control.direction.x = 1;
        
        var position = world.GetComponent<PositionComponent>(entity);

        if (states.HasState(SpawnBombState))
        {
            var state = states.GetState(SpawnBombState);
            if (state.time > spawnBombDuration)
            {
                var bombEntity = world.CreateEntity(bombDefinition.GetInterface<IEntityDefinition>(), null);
                ref var bombPosition = ref world.GetComponent<PositionComponent>(bombEntity);
                
                ref var bombPlayerComponent = ref world.GetComponent<PlayerComponent>(bombEntity);
                bombPlayerComponent.player = playerComponent.player;
                
                bombPosition.value = position.value + spawnBombOffset;
                spawnBombTime = 0;  
                
                unitStateComponent.attacking1 = false;
                states.ExitState(SpawnBombState);
            }

            return;
        }
        
        spawnBombTime += dt;

        if (spawnBombTime > spawnBombCooldown)
        {
            states.EnterState(SpawnBombState);
            // control.direction.x = 0;
            unitStateComponent.attacking1 = true;
            return;
        }
        
        unitStateComponent.walking = true;
    }
}