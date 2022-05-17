using Gemserk.Leopotam.Ecs;
using Leopotam.EcsLite;

namespace GBJAM9.Ecs
{
    public class ModelSystem : BaseSystem, IEcsRunSystem, IFixedUpdateSystem
    {
        public void Run(EcsSystems systems)
        {
            var models = world.GetComponents<UnitModelComponent>();
            var positions = world.GetComponents<PositionComponent>();

            foreach (var entity in world.GetFilter<UnitModelComponent>().End())
            {
                ref var modelComponent = ref models.Get(entity);

                if (modelComponent.prefab != null && modelComponent.instance == null)
                {
                    modelComponent.instance = Instantiate(modelComponent.prefab);
                }
            }
            
            foreach (var entity in world.GetFilter<UnitModelComponent>().Inc<PositionComponent>().End())
            {
                ref var modelComponent = ref models.Get(entity);
                ref var positionComponent = ref positions.Get(entity);

                modelComponent.instance.transform.position = positionComponent.value;
            }
        }
    }
}