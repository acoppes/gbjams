using GBJAM10.Ecs;
using Gemserk.Leopotam.Ecs;
using Gemserk.Leopotam.Ecs.Controllers;
using Gemserk.Leopotam.Ecs.Gameplay;
using UnityEngine;

public class MainEnemyController : ControllerBase
{
    public GameObject bombDefinition;

    public float spawnBombCooldown;
    public Vector2 spawnBombOffset = new Vector2(-1, 0);
    private float spawnBombTime;
    
    public override void OnUpdate(float dt)
    {
        ref var states = ref world.GetComponent<StatesComponent>(entity);
        ref var control = ref world.GetComponent<UnitControlComponent>(entity);

        var position = world.GetComponent<PositionComponent>(entity);
        
        control.direction.x = 1;

        spawnBombTime += dt;

        if (spawnBombTime > spawnBombCooldown)
        {
            var bombEntity = world.CreateEntity(bombDefinition.GetInterface<IEntityDefinition>(), null);
            ref var bombPosition = ref world.GetComponent<PositionComponent>(bombEntity);
            bombPosition.value = position.value + spawnBombOffset;
            spawnBombTime = 0;
        }
    }
}