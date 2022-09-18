using Gemserk.Leopotam.Ecs;
using Gemserk.Leopotam.Ecs.Gameplay;
using Leopotam.EcsLite;

namespace GBJAM10.Ecs
{
    public class TargetExtraSystem : BaseSystem, IEcsRunSystem
    {
        public void Run(EcsSystems systems)
        {
            var targets = world.GetComponents<TargetComponent>();
            var lookingDirections = world.GetComponents<LookingDirection>();
            
            foreach (var entity in world.GetFilter<TargetComponent>().Inc<LookingDirection>().End())
            {
                ref var targetComponent = ref targets.Get(entity);
                var lookingDirection = lookingDirections.Get(entity);

                ref var target = ref targetComponent.target;

                if (target.extra == null)
                {
                    target.extra = new TargetExtra();
                }
                
                var targetExtra = target.extra as TargetExtra;
                targetExtra.lookingDirection = lookingDirection.value;
            }
        }
    }
}