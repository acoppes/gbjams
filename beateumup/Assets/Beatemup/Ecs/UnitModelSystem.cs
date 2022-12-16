using Beatemup.Models;
using Gemserk.Leopotam.Ecs;
using Gemserk.Leopotam.Ecs.Gameplay;
using Leopotam.EcsLite;
using UnityEngine;

namespace Beatemup.Ecs
{
    public class UnitModelSystem : BaseSystem, IEcsRunSystem, IEntityCreatedHandler, IEntityDestroyedHandler
    {
        public GamePerspectiveAsset gamePerspective;
        
        public void OnEntityCreated(Gemserk.Leopotam.Ecs.World world, Entity entity)
        {
            // create model if model component
            var models = world.GetComponents<UnitModelComponent>();
            if (models.Has(entity))
            {
                ref var model = ref models.Get(entity);
                var modelInstance = Instantiate(model.prefab);
                
                model.instance = modelInstance.GetComponent<Model>();
                model.instance.gameObject.SetActive(true);
                // model.subModel = model.instance.transform.Find("Model");
            }
        }

        public void OnEntityDestroyed(Gemserk.Leopotam.Ecs.World world, Entity entity)
        {
            // destroy model if model component
            var models = world.GetComponents<UnitModelComponent>();
            if (models.Has(entity))
            {
                ref var model = ref models.Get(entity);
                if (model.instance != null)
                {
                    Destroy(model.instance.gameObject);
                }
                model.instance = null;
            }
        }

        public void Run(EcsSystems systems)
        {
            var modelComponents = world.GetComponents<UnitModelComponent>();
            var positionComponents = world.GetComponents<PositionComponent>();
            
            var lookingDirectionComponents = world.GetComponents<LookingDirection>();
            
            foreach (var entity in world.GetFilter<UnitModelComponent>().End())
            {
                var modelComponent = modelComponents.Get(entity);

                if (modelComponent.instance.shadow != null)
                {
                    modelComponent.instance.shadow.enabled = modelComponent.hasShadow;
                    modelComponent.instance.shadow.transform.localScale = new Vector3(1, 
                        modelComponent.shadowPerspective, 1);
                }
                
                // disable by default
                if (modelComponent.instance.playerIndicator != null)
                {
                    modelComponent.instance.playerIndicator.enabled = false;
                }

                modelComponent.instance.model.color = modelComponent.color;
            }
            
            foreach (var entity in world.GetFilter<UnitModelComponent>().Inc<PositionComponent>().End())
            {
                ref var modelComponent = ref modelComponents.Get(entity);
                var positionComponent = positionComponents.Get(entity);

                var position = gamePerspective.ConvertFromWorld(positionComponent.value);

                modelComponent.instance.transform.position = new Vector3(position.x, position.y, 0);
                modelComponent.instance.model.transform.localPosition = new Vector3(0, position.z, 0);
            }

            foreach (var entity in world.GetFilter<UnitModelComponent>().Inc<LookingDirection>().End())
            {
                var modelComponent = modelComponents.Get(entity);
                var lookingDirection = lookingDirectionComponents.Get(entity);

                var modelInstance = modelComponent.instance;

                var scale = modelInstance.transform.localScale;

                if (modelComponent.rotation == UnitModelComponent.RotationType.FlipToLookingDirection)
                {
                    if (Mathf.Abs(lookingDirection.value.x) > 0)
                    {
                        scale.x = lookingDirection.value.x < 0 ? -1 : 1;
                    }
                    
                    modelInstance.transform.localScale = scale;
                }
                else if (modelComponent.rotation == UnitModelComponent.RotationType.Rotate)
                {
                    var angle = Mathf.Atan2(lookingDirection.value.z, lookingDirection.value.x) * Mathf.Rad2Deg;
                    var angleAxis = Quaternion.AngleAxis(angle, Vector3.forward);
                    var eulerAngles = angleAxis.eulerAngles;

                    // eulerAngles.x = eulerAngles.z * 0.75f;

                    modelComponent.instance.model.transform.localEulerAngles = eulerAngles;

                    if (modelComponent.hasShadow)
                    {
                        modelComponent.instance.shadow.transform.localEulerAngles = eulerAngles;
                    }
                }
            }
        }
    }
}