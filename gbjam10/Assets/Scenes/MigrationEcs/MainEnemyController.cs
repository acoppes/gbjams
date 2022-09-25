using GBJAM10.Ecs;
using Gemserk.Leopotam.Ecs;
using Gemserk.Leopotam.Ecs.Controllers;
using Gemserk.Leopotam.Ecs.Gameplay;
using UnityEngine;

public class MainEnemyController : ControllerBase
{
    private const string SpawnBombState = "SpawningBomb";
    private const string SwitchingPositionState = "SwitchingPosition";
    
    public GameObject bombDefinition;

    private float switchPositionDestinationY;
    
    public Vector2 spawnBombOffset = new Vector2(-1, 0);

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
                switchPositionAbility.cooldownCurrent = 0;
                states.ExitState(SwitchingPositionState);
            }
            
            return;
        }

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

        if (switchPositionAbility.isCooldownReady)
        {
            switchPositionDestinationY = UnityEngine.Random.Range(-3.0f, 3.0f);
            
            Debug.Log($"DestinationY: {switchPositionDestinationY}");
            
            // switchPositionDestinationY = position.value.y + UnityEngine.Random.Range(1.0f, 3.0f) * 
            //     (UnityEngine.Random.Range(0, 1) < 0.5f ? -1.0f : 1.0f);
            
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