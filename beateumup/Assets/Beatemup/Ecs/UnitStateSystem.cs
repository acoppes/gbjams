using Gemserk.Leopotam.Ecs;
using Gemserk.Leopotam.Ecs.Gameplay;
using Leopotam.EcsLite;

namespace Beatemup.Ecs
{
    public class UnitStateSystem : BaseSystem, IEcsRunSystem
    {
        public void Run(EcsSystems systems)
        {
            var inputs = world.GetComponents<UnitControlComponent>();
            var movements = world.GetComponents<UnitMovementComponent>();
            var states = world.GetComponents<UnitStateComponent>();
            var healths = world.GetComponents<HealthComponent>();

            foreach (var entity in world
                         .GetFilter<UnitStateComponent>()
                         .Inc<UnitControlComponent>()
                         .Inc<UnitMovementComponent>()
                         .End())
            { 
                ref var unitStateComponent = ref states.Get(entity);
                var inputComponent = inputs.Get(entity);
                var movement = movements.Get(entity);
                
                if (!unitStateComponent.disableAutoUpdate)
                {
                    unitStateComponent.walking = !movement.disabled && inputComponent.direction.sqrMagnitude > 0;
                }
            }
            
            foreach (var entity in world
                         .GetFilter<UnitStateComponent>()
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

                unitStateComponent.healthState = healthComponent.state;
            }
        }
    }
}