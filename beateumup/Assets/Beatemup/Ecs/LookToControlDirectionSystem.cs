using Gemserk.Leopotam.Ecs;
using Gemserk.Leopotam.Ecs.Gameplay;
using Leopotam.EcsLite;

namespace Beatemup.Ecs
{
    public class LookToControlDirectionSystem : BaseSystem, IEcsRunSystem
    {
        public void Run(EcsSystems systems)
        {
            // var gameData = world.sharedData.sharedData as SharedGameData;

            var controlComponents = world.GetComponents<ControlComponent>();
            var lookingDirectionComponents = world.GetComponents<LookingDirection>();
            
            // Update looking direction based on controls
            foreach (var entity in world.GetFilter<ControlComponent>().Inc<LookingDirection>().End())
            {
                var control = controlComponents.Get(entity);
                ref var lookingDirection = ref lookingDirectionComponents.Get(entity);

                if (control.direction.sqrMagnitude > 0f)
                {
                    lookingDirection.value = control.direction.normalized;
                }
            }
        }
    }
}
