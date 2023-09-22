using Game.Components;
using Game.Components.Abilities;
using Game.Controllers;
using Game.Utilities;
using GBJAM11.Extensions;
using Gemserk.Leopotam.Ecs;
using Gemserk.Leopotam.Ecs.Components;
using Gemserk.Leopotam.Ecs.Controllers;
using Gemserk.Leopotam.Ecs.Events;
using UnityEngine;

namespace GBJAM11.Controllers
{
    public class EnemySlugController : ControllerBase, IUpdate, IActiveController
    {
        public bool CanBeInterrupted(Entity entity, IActiveController activeController)
        {
            throw new System.NotImplementedException();
        }

        public void OnInterrupt(Entity entity, IActiveController activeController)
        {
            throw new System.NotImplementedException();
        }
        
        public void OnUpdate(World world, Entity entity, float dt)
        {
            ref var abilities = ref entity.Get<AbilitiesComponent>();
            ref var states = ref entity.Get<StatesComponent>();
            var chaseAbility = abilities.GetAbilityNoNullCheck("Chase");
            ref var movement = ref entity.Get<MovementComponent>();
            var position = entity.Get<PositionComponent>();
            
            if (states.TryGetState("Chase", out var chaseState))
            {
                // follow target
                var abilityTarget = chaseAbility.abilityTargets[0];

                //if (!chaseAbility.IsValidTarget(abilityTarget.target))
                if (!abilityTarget.valid)
                {
                    ExitChase(world, entity);
                    return;
                }
                
                movement.movingDirection = (abilityTarget.position - position.value).normalized;
                return;
            }
            
            if (chaseAbility.CalculateTargets(world))
            {
                EnterChase(world, entity);
                return;
            }
        }

        private void EnterChase(World world, Entity entity)
        {
            ref var states = ref entity.Get<StatesComponent>();
            ref var abilities = ref entity.Get<AbilitiesComponent>();
            
            var chaseAbility = abilities.GetAbilityNoNullCheck("Chase");
            chaseAbility.targetsLocked = true;
            chaseAbility.Start();
            
            states.EnterState("Chase");
        }

        private void ExitChase(World world, Entity entity)
        {
            ref var states = ref entity.Get<StatesComponent>();
            ref var abilities = ref entity.Get<AbilitiesComponent>();
            ref var movement = ref entity.Get<MovementComponent>();
            
            var chaseAbility = abilities.GetAbilityNoNullCheck("Chase");
            chaseAbility.targetsLocked = false;
            chaseAbility.Stop(Ability.StopType.Completed);
            chaseAbility.abilityTargets.Clear();
            
            movement.movingDirection = Vector2.zero;
            
            states.ExitState("Chase");
        }
 
    }
}