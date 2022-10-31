using Gemserk.Leopotam.Ecs;
using Gemserk.Leopotam.Ecs.Gameplay;
using Leopotam.EcsLite;

namespace Beatemup.Ecs
{
    public class UnitStateSystem : BaseSystem, IEcsRunSystem
    {
        public void Run(EcsSystems systems)
        {
            var controlComponents = world.GetComponents<ControlComponent>();
            var movements = world.GetComponents<UnitMovementComponent>();
            var states = world.GetComponents<ModelStateComponent>();
            var healths = world.GetComponents<HealthComponent>();

            foreach (var entity in world
                         .GetFilter<ModelStateComponent>()
                         .Inc<ControlComponent>()
                         .Inc<UnitMovementComponent>()
                         .End())
            { 
                ref var unitStateComponent = ref states.Get(entity);
                var controlComponent = controlComponents.Get(entity);
                var movement = movements.Get(entity);
                
                if (!unitStateComponent.disableAutoUpdate)
                {
                    unitStateComponent.walking = !movement.disabled && controlComponent.direction.sqrMagnitude > 0;
                    unitStateComponent.up = controlComponent.direction.y > 0;
                }
            }
            
            foreach (var entity in world
                         .GetFilter<ModelStateComponent>()
                         .Inc<HealthComponent>()
                         .End())
            { 
                ref var unitStateComponent = ref states.Get(entity);
                var healthComponent = healths.Get(entity);
                ref var triggers = ref unitStateComponent.stateTriggers;

                triggers.hit = false;
                
                if (healthComponent.pendingDamages.Count > 0)
                {
                    triggers.hit = true;
                }
            }
        }
    }
}