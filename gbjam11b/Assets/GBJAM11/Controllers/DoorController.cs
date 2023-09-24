using Game.Components;
using Game.Components.Abilities;
using GBJAM11.Components;
using Gemserk.Leopotam.Ecs;
using Gemserk.Leopotam.Ecs.Components;
using Gemserk.Leopotam.Ecs.Controllers;
using Gemserk.Leopotam.Ecs.Events;

namespace GBJAM11.Controllers
{
    public class DoorController : ControllerBase, IUpdate, IInit
    {
        public void OnInit(World world, Entity entity)
        {
            var doorComponent = entity.Get<DoorComponent>();
            if (doorComponent.startsOpen)
            {
                EnterOpen(world, entity);
            }
            else
            {
                EnterClosed(world, entity);
            }
        }
        
        public void OnUpdate(World world, Entity entity, float dt)
        {
            // on open, play open anim and disable collider
            
            ref var abilities = ref entity.Get<AbilitiesComponent>();
            ref var states = ref entity.Get<StatesComponent>();
            ref var animations = ref entity.Get<AnimationComponent>();
            ref var doorComponent = ref entity.Get<DoorComponent>();
            
            var openAbility = abilities.GetAbilityNoNullCheck("Open");
            var closeAbility = abilities.GetAbilityNoNullCheck("Close");

            if (states.TryGetState("Opening", out var openState))
            {
                if (animations.isCompleted)
                {
                    ExitOpening(world, entity);
                    EnterOpen(world, entity);
                }

                return;
            }
            
            if (states.TryGetState("Closing", out var closingState))
            {
                if (animations.isCompleted)
                {
                    ExitClosing(world, entity);
                    EnterClosed(world, entity);
                }

                return;
            }
            
            
            if (openAbility.pendingExecution)
            {
                if (doorComponent.isClosed)
                {
                    EnterOpening(world, entity);
                    return;
                }
            }
            
            if (closeAbility.pendingExecution)
            {
                if (doorComponent.isOpen)
                {
                    EnterClosing(world, entity);
                    return;
                }
            }
        }
        
        private void EnterOpening(World world, Entity entity)
        {
            ref var states = ref entity.Get<StatesComponent>();
            ref var abilities = ref entity.Get<AbilitiesComponent>();
            ref var animations = ref entity.Get<AnimationComponent>();
            
            ref var doorComponent = ref entity.Get<DoorComponent>();
            doorComponent.isClosed = false;
            doorComponent.isOpen = false;
            
            var ability = abilities.GetAbilityNoNullCheck("Open");
            ability.Start();
            animations.Play("Opening", 0);
            
            states.EnterState("Opening");
        }

        private void ExitOpening(World world, Entity entity)
        {
            ref var states = ref entity.Get<StatesComponent>();
            ref var abilities = ref entity.Get<AbilitiesComponent>();
            
            var ability = abilities.GetAbilityNoNullCheck("Open");
            ability.Stop(Ability.StopType.Completed);
            
            states.ExitState("Opening");
        }
        
        private void EnterClosing(World world, Entity entity)
        {
            ref var states = ref entity.Get<StatesComponent>();
            ref var abilities = ref entity.Get<AbilitiesComponent>();
            ref var animations = ref entity.Get<AnimationComponent>();
            
            var ability = abilities.GetAbilityNoNullCheck("Close");
            ability.Start();
            animations.Play("Close", 0);
            
            states.EnterState("Closing");

            entity.Get<Physics2dComponent>().disableCollisions = false;
        }

        private void ExitClosing(World world, Entity entity)
        {
            ref var states = ref entity.Get<StatesComponent>();
            ref var abilities = ref entity.Get<AbilitiesComponent>();
            
            var ability = abilities.GetAbilityNoNullCheck("Close");
            ability.Stop(Ability.StopType.Completed);
            
            states.ExitState("Closing");
        }
        
        private void EnterOpen(World world, Entity entity)
        {
            ref var animations = ref entity.Get<AnimationComponent>();
            ref var doorComponent = ref entity.Get<DoorComponent>();

            doorComponent.isOpen = true;
            doorComponent.isClosed = false;
            
            animations.Play("Open", 0);
            entity.Get<Physics2dComponent>().disableCollisions = true;
        }
        
        private void EnterClosed(World world, Entity entity)
        {
            ref var animations = ref entity.Get<AnimationComponent>();
            ref var doorComponent = ref entity.Get<DoorComponent>();

            doorComponent.isClosed = true;
            doorComponent.isOpen = false;
            
            animations.Play("Closed", 0);
            entity.Get<Physics2dComponent>().disableCollisions = false;
        }


    }
}