using Gemserk.Leopotam.Ecs;
using Leopotam.EcsLite;

namespace GBJAM9.Ecs
{
    public class ProjectileSystem : BaseSystem, IEcsRunSystem
    {
        public void Run(EcsSystems systems)
        {
            var positions = world.GetComponents<PositionComponent>();
            var lookingDirections = world.GetComponents<LookingDirection>();

            var projectileComponents = world.GetComponents<ProjectileComponent>();
            var unitControlComponents = world.GetComponents<UnitControlComponent>();
            
            // update ability position
            foreach (var entity in world.GetFilter<ProjectileComponent>()
                         .Inc<PositionComponent>()
                         .Inc<LookingDirection>().End())
            {
                ref var projectileComponent = ref projectileComponents.Get(entity);

                if (projectileComponent.started)
                {
                    continue;
                }
                
                ref var position = ref positions.Get(entity);
                position.value = projectileComponent.startPosition;
                
                ref var lookingDirection = ref lookingDirections.Get(entity);
                lookingDirection.value = projectileComponent.startDirection;

                projectileComponent.started = true;
            }
            
            foreach (var entity in world.GetFilter<ProjectileComponent>()
                         .Inc<UnitControlComponent>().End())
            {
                var projectileComponent = projectileComponents.Get(entity);
                ref var unitControlComponent = ref unitControlComponents.Get(entity);

                unitControlComponent.direction = projectileComponent.startDirection;
            }
        }
    }
}