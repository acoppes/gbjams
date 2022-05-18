using Gemserk.Leopotam.Ecs;
using Leopotam.EcsLite;

namespace GBJAM9.Ecs
{
    public class ModelSystem : BaseSystem, IEcsRunSystem, IFixedUpdateSystem, IEcsInitSystem
    {
        public void Init(EcsSystems systems)
        {
            world.onEntityCreated += OnEntityCreated;
            world.onEntityDestroyed += OnEntityDestroyed;
        }
        
        private void OnEntityCreated(Gemserk.Leopotam.Ecs.World world, int entity)
        {
            // create model if model component
            var models = world.GetComponents<UnitModelComponent>();
            if (models.Has(entity))
            {
                ref var model = ref models.Get(entity);
                model.instance =  Instantiate(model.prefab);
            }
        }

        private void OnEntityDestroyed(Gemserk.Leopotam.Ecs.World world, int entity)
        {
            // destroy model if model component
            var models = world.GetComponents<UnitModelComponent>();
            if (models.Has(entity))
            {
                ref var model = ref models.Get(entity);
                if (model.instance != null)
                {
                    Destroy(model.instance);
                }
                model.instance = null;
            }
        }

        public void Run(EcsSystems systems)
        {
            var models = world.GetComponents<UnitModelComponent>();
            var positions = world.GetComponents<PositionComponent>();

            // foreach (var entity in world.GetFilter<UnitModelComponent>().End())
            // {
            //     ref var modelComponent = ref models.Get(entity);
            //
            //     if (modelComponent.prefab != null && modelComponent.instance == null)
            //     {
            //         modelComponent.instance = Instantiate(modelComponent.prefab);
            //     }
            // }
            
            foreach (var entity in world.GetFilter<UnitModelComponent>().Inc<PositionComponent>().End())
            {
                ref var modelComponent = ref models.Get(entity);
                ref var positionComponent = ref positions.Get(entity);

                modelComponent.instance.transform.position = positionComponent.value;
            }
        }


    }
}