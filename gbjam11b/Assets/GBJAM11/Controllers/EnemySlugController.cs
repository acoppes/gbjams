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
            return true;
        }

        public void OnInterrupt(Entity entity, IActiveController activeController)
        {
            ExitAttack(entity.world, entity);
        }
        
        public void OnUpdate(World world, Entity entity, float dt)
        {
            ref var abilities = ref entity.Get<AbilitiesComponent>();
            ref var states = ref entity.Get<StatesComponent>();
            ref var movement = ref entity.Get<MovementComponent>();
            var position = entity.Get<PositionComponent>();
            ref var activeController = ref entity.Get<ActiveControllerComponent>();
            ref var animations = ref entity.Get<AnimationComponent>();
            
            var chaseAbility = abilities.GetAbilityNoNullCheck("Chase");
            var attackAbility = abilities.GetAbilityNoNullCheck("Attack");

            attackAbility.CalculateTargets(world);

            var canAttack = attackAbility.hasTargets && attackAbility.isReady;
            
            if (states.TryGetState("Attack", out var attackState))
            {
                if (animations.IsPlaying("Attack") && animations.isCompleted)
                {
                    // perform damage

                    var abilityTarget = attackAbility.abilityTargets[0];

                    if (abilityTarget.valid)
                    {
                        abilityTarget.target.entity.Get<HealthComponent>().damages.Add(new DamageData()
                        {
                            value = 1
                        });
                    }
                    
                    ExitAttack(world, entity);
                    return;
                }

                return;
            }
            
            if (states.TryGetState("Chase", out var chaseState))
            {
                // follow target
                var abilityTarget = chaseAbility.abilityTargets[0];

                //if (!chaseAbility.IsValidTarget(abilityTarget.target))
                if (!abilityTarget.valid || canAttack)
                {
                    ExitChase(world, entity);
                    return;
                }
                
                movement.movingDirection = (abilityTarget.position - position.value).normalized;
                return;
            }

            if (canAttack && activeController.CanInterrupt(entity, this))
            {
                EnterAttack(world, entity);
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
 
        private void EnterAttack(World world, Entity entity)
        {
            ref var states = ref entity.Get<StatesComponent>();
            ref var abilities = ref entity.Get<AbilitiesComponent>();
            ref var animations = ref entity.Get<AnimationComponent>();
            ref var activeController = ref entity.Get<ActiveControllerComponent>();
            ref var movement = ref entity.Get<MovementComponent>();
            
            var ability = abilities.GetAbilityNoNullCheck("Attack");
            ability.targetsLocked = true;
            ability.Start();
            
            movement.movingDirection = Vector2.zero;
            
            animations.Play("Attack", 0);
            states.EnterState("Attack");
            activeController.TakeControl(entity, this);
        }

        private void ExitAttack(World world, Entity entity)
        {
            ref var states = ref entity.Get<StatesComponent>();
            ref var abilities = ref entity.Get<AbilitiesComponent>();
            ref var movement = ref entity.Get<MovementComponent>();
            ref var activeController = ref entity.Get<ActiveControllerComponent>();
            
            var ability = abilities.GetAbilityNoNullCheck("Attack");
            ability.targetsLocked = false;
            ability.Stop(Ability.StopType.Completed);
            ability.abilityTargets.Clear();
            
            activeController.ReleaseControl(this);
            states.ExitState("Attack");
        }
    }
}