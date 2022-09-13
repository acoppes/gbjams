using System.Collections.Generic;
using GBJAM10.Ecs;
using Gemserk.Leopotam.Ecs;
using Gemserk.Leopotam.Ecs.Controllers;
using Gemserk.Leopotam.Ecs.Gameplay;
using MoonSharp.Interpreter;
using UnityEngine;

public class SamuraiDogAIController : ControllerBase
{
    public override void OnUpdate(float dt)
    {
        ref var playerInput = ref world.GetComponent<PlayerInputComponent>(entity);
        
        var position = world.GetComponent<PositionComponent>(entity);
        
        ref var movementComponent = ref world.GetComponent<UnitMovementComponent>(entity);

        ref var states = ref world.GetComponent<StatesComponent>(entity);
        ref var control = ref world.GetComponent<UnitControlComponent>(entity);
        ref var unitState = ref world.GetComponent<UnitStateComponent>(entity);

        ref var abilities = ref world.GetComponent<AbilitiesComponent>(entity);
        
        var chargeSpecialAttack = abilities.GetAbility("SecondaryAbility");
        
        // if controllable by player, disable AI.

        // var lua = new Lua();
        // lua["x"] = 10;
        // var x = (int) lua.DoString("x = x + 1`")[0];
        // Debug.Log(x);

        // var scriptCode = @"
        //     return x + 1
        // ";
        //
        // var script = new Script();
        //
        // // DynValue x = UserData.Create(5);
        // // Debug.Log(x.Number);
        //
        // script.Globals["x"] = 5;
        //
        // var result = script.DoString(scriptCode);
        // Debug.Log(result.Number);
        
        // DynValue res = Script.RunString(script);
        // return res.Number;
        
        
        // TODO: stop running stuff if death (controls)
        
        if (playerInput.disabled)
            return;

        if (unitState.healthState == HealthComponent.State.Death)
        {
            control.direction = Vector2.zero;
            return;
        }

        // var targets = TargetingUtils.FindTargets(world, specialAttackTargeting);

        var chaseTargets = abilities.GetTargeting("Chase").targets;

        // it is performing the special attack or recovering from the attack
        if (states.HasState("SpecialAttack") || states.HasState("SpecialAttackRecovery"))
        {
            return;
        }
        
        if (states.HasState("ChargingSpecialAttack"))
        {
            var state = states.GetState("ChargingSpecialAttack");

            if (chaseTargets.Count > 0)
            {
                var chaseTarget = chaseTargets[0];
                control.direction = (chaseTarget.position - position.value).normalized;
            }
            
            if (state.time > chargeSpecialAttack.duration)
            {
                control.secondaryAction = false;
                states.ExitState("ChargingSpecialAttack");
            }
            
            return;
        }
        
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
        
        if (chaseTargets.Count > 0 && chargeSpecialAttack.isReady)
        {
            var chaseTarget = chaseTargets[0];
            
            if (TargetingUtils.ValidateTarget(abilities.GetTargeting("SpecialAttack").parameters, chaseTarget))
            {
                states.EnterState("ChargingSpecialAttack");
                states.ExitState("ChasingPlayer");

                control.direction = Vector2.zero;
                control.secondaryAction = true;

                return;
            }
        }

        var basicAttackTargets = abilities.GetTargeting("MainAbility").targets;

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

        if (states.HasState("ChasingPlayer"))
        {
            if (chaseTargets.Count == 0)
            {
                states.ExitState("ChasingPlayer");
                return;
            }

            var chaseTarget = chaseTargets[0];
            control.direction = (chaseTarget.position - position.value).normalized;

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