using Gemserk.Leopotam.Ecs;
using Gemserk.Leopotam.Ecs.Gameplay;
using Leopotam.EcsLite;

namespace Beatemup.Ecs
{
    public class TargetSystem : BaseSystem, IEcsRunSystem
    {
        public void Run(EcsSystems systems)
        {
            var targetComponents = world.GetComponents<TargetComponent>();
            var playerComponents = world.GetComponents<PlayerComponent>();
            var hitBoxComponents = world.GetComponents<HitBoxComponent>();
            var positions = world.GetComponents<PositionComponent>();
            
            foreach (var entity in world.GetFilter<TargetComponent>().Inc<PositionComponent>().End())
            {
                ref var targetComponent = ref targetComponents.Get(entity);
                var positionComponent = positions.Get(entity);

                ref var target = ref targetComponent.target;
                target.entity = entity;
                target.position = positionComponent.value;
            }
            
            foreach (var entity in world.GetFilter<TargetComponent>().Inc<PlayerComponent>().End())
            {
                ref var targetComponent = ref targetComponents.Get(entity);
                var playerComponent = playerComponents.Get(entity);

                targetComponent.target.player = playerComponent.player;
            }
            
            foreach (var entity in world.GetFilter<TargetComponent>().Inc<HitBoxComponent>().End())
            {
                ref var targetComponent = ref targetComponents.Get(entity);
                var hitBoxComponent = hitBoxComponents.Get(entity);

                targetComponent.target.hurtBox = hitBoxComponent.hurt;
            }
        }
    }
}