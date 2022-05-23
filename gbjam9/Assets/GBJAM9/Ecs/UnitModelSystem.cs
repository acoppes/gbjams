using Gemserk.Leopotam.Ecs;
using Leopotam.EcsLite;
using UnityEngine;

namespace GBJAM9.Ecs
{
    public class UnitModelSystem : BaseSystem, IEcsRunSystem, IEntityCreatedHandler, IEntityDestroyedHandler
    {
        public void OnEntityCreated(Gemserk.Leopotam.Ecs.World world, int entity)
        {
            // create model if model component
            var models = world.GetComponents<UnitModelComponent>();
            if (models.Has(entity))
            {
                ref var model = ref models.Get(entity);
                model.instance =  Instantiate(model.prefab);
            }
        }

        public void OnEntityDestroyed(Gemserk.Leopotam.Ecs.World world, int entity)
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
            var modelComponents = world.GetComponents<UnitModelComponent>();
            var positionComponents = world.GetComponents<PositionComponent>();
            var lookingDirectionComponents = world.GetComponents<LookingDirection>();
            
            foreach (var entity in world.GetFilter<UnitModelComponent>().Inc<PositionComponent>().End())
            {
                ref var modelComponent = ref modelComponents.Get(entity);
                var positionComponent = positionComponents.Get(entity);

                modelComponent.instance.transform.position = positionComponent.value;
            }
            
            foreach (var entity in world.GetFilter<UnitModelComponent>().Inc<LookingDirection>().End())
            {
                var modelComponent = modelComponents.Get(entity);
                var lookingDirection = lookingDirectionComponents.Get(entity);

                var modelInstance = modelComponent.instance;

                var scale = modelInstance.transform.localScale;

                if (!modelComponent.rotateToDirection)
                {
                    if (Mathf.Abs(lookingDirection.value.x) > 0)
                    {
                        scale.x = lookingDirection.value.x < 0 ? -1 : 1;
                    }
                    
                    modelInstance.transform.localScale = scale;
                }
                else
                {
                    var angle = Mathf.Atan2(lookingDirection.value.y, lookingDirection.value.x) * Mathf.Rad2Deg;
                    modelInstance.transform.Find("Model").rotation = Quaternion.AngleAxis(angle, Vector3.forward);
                }
            }
        }
    }
}