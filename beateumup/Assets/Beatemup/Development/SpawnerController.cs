using System.Collections.Generic;
using Beatemup;
using Beatemup.Ecs;
using Gemserk.Leopotam.Ecs;
using Gemserk.Leopotam.Ecs.Gameplay;
using Gemserk.Leopotam.Gameplay.Controllers;
using Gemserk.Leopotam.Gameplay.Events;
using UnityEngine;
using TargetingUtils = Beatemup.Ecs.TargetingUtils;

public class SpawnerController : ControllerBase, IInit
{
    public GameObject enemyDefinition;
    public GameObject enemyParameters;
    
    public List<BoxCollider2D> spawnAreas = new List<BoxCollider2D>();

    public int wavesToIncrementSpawns = 3;
    
    public int spawnsPerWave = 1;

    private int spawnedWaves = 0;

    public float waveBaseDuration = 5;
    public float waveIncrementDuration = 5;

    private float currentWaveDuration;
    
    public void OnInit()
    {

    }
    
    public override void OnUpdate(float dt)
    {
        var player = world.GetComponent<PlayerComponent>(entity);

        currentWaveDuration -= dt;
        
        // var query = new Query()
        //     .CheckName("Enemy_Soldier")
        //     .CheckPlayer(1);
        
        // if there are still enemies...

        var targets = TargetingUtils.GetTargets(world, new TargetingUtils.RuntimeTargetingParameters
        {
            player = player.player,
            checkAreaType = TargetingUtils.RuntimeTargetingParameters.CheckAreaType.Nothing,
            playerAllianceType = TargetingUtils.PlayerAllianceType.Allies,
            name = "Enemy_Soldier",
            aliveType = HitPointsComponent.AliveType.Alive
        });
        
        // if (world.Query(query))
        // {
        //     return;
        // }

        if (targets.Count > 0 && currentWaveDuration > 0)
        {
            return;
        }

        var spawnAreaIndex = UnityEngine.Random.Range(0, spawnAreas.Count);

        for (var i = 0; i < spawnsPerWave; i++)
        {
            var enemyEntity = world.CreateEntity(enemyDefinition.GetComponent<IEntityDefinition>(),
                enemyParameters.GetComponentsInChildren<IEntityInstanceParameter>());
        
            ref var enemyPosition = ref world.GetComponent<PositionComponent>(enemyEntity);
        
            var spawnArea = spawnAreas[spawnAreaIndex];

            enemyPosition.value = new Vector3(UnityEngine.Random.Range(spawnArea.bounds.min.x, spawnArea.bounds.max.x), 
                UnityEngine.Random.Range(spawnArea.bounds.min.y, spawnArea.bounds.max.y), 0);
            
            ref var instancePlayer = ref world.GetComponent<PlayerComponent>(enemyEntity);

            // player.player = enemyPosition.value.x < 0 ? 0 : 1;
            instancePlayer.player = player.player;

            spawnAreaIndex = (spawnAreaIndex + 1) % spawnAreas.Count;
        }

        currentWaveDuration = waveBaseDuration + waveIncrementDuration * (spawnsPerWave - 1);
        
        spawnedWaves++;

        if (spawnedWaves % wavesToIncrementSpawns == 0)
        {
            spawnsPerWave++;
        }
    }

}
