using GBJAM9.Ecs;
using Gemserk.Leopotam.Ecs;
using Gemserk.Leopotam.Ecs.Controllers;
using UnityEngine;

public class SamuraiDogAIController : MonoBehaviour, IController
{
    public float chaseDistance;
    
    public float basicAttackDistance = 0.5f;
    public float specialAttackDistance;

    public float specialAttackChargeTime;
    
    public void OnUpdate(float dt, World world, int entity)
    {
        ref var playerInput = ref world.GetComponent<PlayerInputComponent>(entity);
        
        var position = world.GetComponent<PositionComponent>(entity);
        var playerComponent = world.GetComponent<PlayerComponent>(entity);
        
        ref var movementComponent = ref world.GetComponent<UnitMovementComponent>(entity);

        ref var states = ref world.GetComponent<StatesComponent>(entity);
        ref var control = ref world.GetComponent<UnitControlComponent>(entity);

        // if controllable by player, disable AI.
        if (!playerInput.disabled)
            return;
        
        var basicAttackTargeting = new TargetingParameters
        {
            player = playerComponent.player,
            position = position.value,
            range = basicAttackDistance
        };
        
        var specialAttackTargeting = new TargetingParameters
        {
            player = playerComponent.player,
            position = position.value,
            range = specialAttackDistance
        };

        // var targets = TargetingUtils.FindTargets(world, specialAttackTargeting);

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
        
        var basicAttackTargets = TargetingUtils.FindTargets(world, basicAttackTargeting);

        // if (states.HasState("BasicAttack"))
        // {
        //     var chaseTarget = chaseTargets[0];
        //     
        //     control.direction = (chaseTarget.position - position.value).normalized;
        //
        //     if (TargetingUtils.ValidateTarget(specialAttackTargeting, chaseTarget))
        //     {
        //         states.EnterState("ChargingSpecialAttack");
        //         states.ExitState("ChasingPlayer");
        //
        //         control.secondaryAction = true;
        //
        //         return;
        //     }
        //     
        //     return;
        // }

        if (states.HasState("DelayAfterAttack"))
        {
            var state = states.GetState("DelayAfterAttack");
            
            control.direction = Vector2.zero;
            control.mainAction = false;
            
            if (state.time > 1)
            {
                states.ExitState("DelayAfterAttack");
            }

            return;
        }
        
        if (basicAttackTargets.Count > 0)
        {
            // states.EnterState("BasicAttack");
            var basicAttackTarget = basicAttackTargets[0];
            control.direction = (basicAttackTarget.position - position.value).normalized;
            control.mainAction = true;
            
            // enter ai state recover from attack or delay attack or something? 
            states.EnterState("DelayAfterAttack");
            
            return;
        }
        
        var chaseTargets = TargetingUtils.FindTargets(world, new TargetingParameters
        {
            player = playerComponent.player,
            position = position.value,
            range = chaseDistance
        });
        
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