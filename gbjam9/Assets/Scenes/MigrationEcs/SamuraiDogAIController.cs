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
        var playerComponent = world.GetComponent<PlayerComponent>(entity);
        
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

        var specialAttackTargeting = new TargetingParameters
        {
            player = playerComponent.player,
            position = position.value,
            range = specialAttackDistance
        };

        // var targets = TargetingUtils.FindTargets(world, specialAttackTargeting);

        var chaseTargets = TargetingUtils.FindTargets(world, new TargetingParameters
        {
            player = playerComponent.player,
            position = position.value,
            range = chaseDistance,
            extraValidation = delegate(TargetingParameters parameters, Target target)
            {
                if (target.extra is not TargetExtra targetExtra)
                {
                    return false;
                }
                
                var direction = (target.position - parameters.position).normalized;
                
                if (Vector2.Angle(targetExtra.lookingDirection, direction) > 45)
                {
                    return false;
                }
                
                return true;
            }
        });
        
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
            if (chaseTargets.Count == 0)
            {
                states.ExitState("ChasingPlayer");
                return;
            }

            var chaseTarget = chaseTargets[0];
            
            control.direction = (chaseTarget.position - position.value).normalized;

            if (TargetingUtils.ValidateTarget(specialAttackTargeting, chaseTarget))
            {
                states.EnterState("ChargingSpecialAttack");
                states.ExitState("ChasingPlayer");

                control.secondaryAction = true;

                return;
            }
            
            return;
        }
        
        if (chaseTargets.Count > 0)
        {
            if (!movementComponent.disabled)
            {
                states.EnterState("ChasingPlayer");
                return;
            }
        }

        // control.secondaryAction = true;
        // control.direction = new Vector2(-1, -1);
    }
}