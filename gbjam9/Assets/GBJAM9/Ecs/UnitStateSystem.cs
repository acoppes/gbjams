using Gemserk.Leopotam.Ecs;
using Leopotam.EcsLite;

namespace GBJAM9.Ecs
{
    public class UnitStateSystem : BaseSystem, IEcsRunSystem
    {
        public void Run(EcsSystems systems)
        {
            var inputs = world.GetComponents<UnitInputComponent>();
            var states = world.GetComponents<UnitStateComponent>();

            foreach (var entity in world
                         .GetFilter<UnitStateComponent>()
                         .Inc<UnitInputComponent>()
                         .End())
            { 
                ref var unitStateComponent = ref states.Get(entity);
                var inputComponent = inputs.Get(entity);

                unitStateComponent.walking =
                    !inputComponent.disabled && inputComponent.movementDirection.sqrMagnitude > 0;
            }
        }
    }
}