using GBJAM9.Ecs;
using Gemserk.Leopotam.Ecs;
using Gemserk.Leopotam.Ecs.Controllers;
using UnityEngine;

public class SamuraiDogAIController : MonoBehaviour, IController
{
    public float chaseDistance;
    public float specialAttackDistance;

    public float specialAttackChargeTime;
    
    public void OnUpdate(float dt, World world, int entity)
    {
        ref var playerInput = ref world.GetComponent<PlayerInputComponent>(entity);
        var position = world.GetComponent<PositionComponent>(entity);
        
        ref var movementComponent = ref world.GetComponent<UnitMovementComponent>(entity);
        
        ref var unitState = ref world.GetComponent<UnitStateComponent>(entity);
        
        ref var states = ref world.GetComponent<StatesComponent>(entity);
        ref var control = ref world.GetComponent<UnitControlComponent>(entity);

        ref var abilities = ref world.GetComponent<AbilitiesComponent>(entity);
        
        var lookingDirection = world.GetComponent<LookingDirection>(entity);
        
        var attack = abilities.Get("Attack");

        playerInput.disabled = true;
        
        // take control of the character...
        
        // find targets, if found and ability ready, start charging...
        
        // if inside range, start charging attack

        var mainCharacter = world.sharedData.singletonByNameEntities["Main_Character"];
        var targetPosition = world.GetComponent<PositionComponent>(mainCharacter);

        var distanceToPlayer = Vector2.Distance(position.value, targetPosition.value);

        // it is performing the special attack or recovering from the attack
        if (states.HasState("SpecialAttack") || states.HasState("SpecialAttackRecovery"))
        {
            return;
        }

        if (states.HasState("ChargingSpecialAttack"))
        {
            var state = states.GetState("ChargingSpecialAttack");

            // control.direction = (targetPosition.value - position.value).normalized;
            
            if (state.time > specialAttackChargeTime)
            {
                control.secondaryAction = false;
                states.ExitState("ChargingSpecialAttack");
                return;
            }
        }
        
        if (states.HasState("ChasingPlayer"))
        {
            control.direction = (targetPosition.value - position.value).normalized;

            if (distanceToPlayer < specialAttackDistance)
            {
                states.EnterState("ChargingSpecialAttack");
                states.ExitState("ChasingPlayer");

                control.secondaryAction = true;

                return;
            }
            
            return;
        }
        
        if (!movementComponent.disabled && distanceToPlayer < chaseDistance)
        {
            states.EnterState("ChasingPlayer");
            return;
        }
        

        // control.secondaryAction = true;
        // control.direction = new Vector2(-1, -1);
    }
}