using Game.Components;
using Game.Components.Abilities;
using Gemserk.Leopotam.Ecs;
using Gemserk.Leopotam.Ecs.Components;
using Gemserk.Leopotam.Ecs.Controllers;
using Gemserk.Leopotam.Ecs.Events;

namespace GBJAM11.Controllers
{
    public class DoorController : ControllerBase, IUpdate
    {
        public void OnUpdate(World world, Entity entity, float dt)
        {
            // on open, play open anim and disable collider
            
            ref var abilities = ref entity.Get<AbilitiesComponent>();
            ref var states = ref entity.Get<StatesComponent>();
            ref var animations = ref entity.Get<AnimationComponent>();
            
            var openAbility = abilities.GetAbilityNoNullCheck("Open");

            if (states.TryGetState("Opening", out var openState))
            {
                if (animations.isCompleted)
                {
                    ExitOpen(world, entity);
                }

                return;
            }
            

            if (openAbility.pendingExecution)
            {
                EnterOpen(world, entity);
                return;
            }
        }
        
        private void EnterOpen(World world, Entity entity)
        {
            ref var states = ref entity.Get<StatesComponent>();
            ref var abilities = ref entity.Get<AbilitiesComponent>();
            ref var animations = ref entity.Get<AnimationComponent>();
            
            var ability = abilities.GetAbilityNoNullCheck("Open");
            ability.Start();
            animations.Play("Open", 0);
            
            states.EnterState("Opening");
            states.EnterState("Open");

            entity.Get<Physics2dComponent>().disableCollisions = true;
        }

        private void ExitOpen(World world, Entity entity)
        {
            ref var states = ref entity.Get<StatesComponent>();
            ref var abilities = ref entity.Get<AbilitiesComponent>();
            
            var ability = abilities.GetAbilityNoNullCheck("Open");
            ability.Stop(Ability.StopType.Completed);
            
            states.ExitState("Opening");
        }
    }
}