using System.Collections;
using System.Collections.Generic;
using Gemserk.Leopotam.Ecs;
using Gemserk.Leopotam.Ecs.Gameplay;
using Gemserk.Leopotam.Gameplay.Controllers;
using Gemserk.Leopotam.Gameplay.Events;
using UnityEngine;

public class SpawnerController : ControllerBase, IInit
{
    public GameObject enemyDefinition;
    public GameObject enemyParameters;
    
    public BoxCollider2D spawnArea;
    
    
    public void OnInit()
    {

    }
    
    public override void OnUpdate(float dt)
    {
        var enemyInstance = world.GetEntityByName("Enemy_Character");

        if (enemyInstance != Entity.NullEntity)
        {
            return;
        }
        
        var enemyEntity = world.CreateEntity(enemyDefinition.GetComponent<IEntityDefinition>(),
            enemyParameters.GetComponentsInChildren<IEntityInstanceParameter>());
        
        ref var enemyPosition = ref world.GetComponent<PositionComponent>(enemyEntity);

        enemyPosition.value = new Vector3(UnityEngine.Random.Range(spawnArea.bounds.min.x, spawnArea.bounds.max.x), 
            UnityEngine.Random.Range(spawnArea.bounds.min.y, spawnArea.bounds.max.y), 0);
            
        ref var instancePlayer = ref world.GetComponent<PlayerComponent>(enemyEntity);
        ref var player = ref world.GetComponent<PlayerComponent>(enemyEntity);
        // player.player = enemyPosition.value.x < 0 ? 0 : 1;
        instancePlayer.player = player.player;
        
    }

}
