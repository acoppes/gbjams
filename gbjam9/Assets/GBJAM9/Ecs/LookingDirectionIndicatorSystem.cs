using Gemserk.Leopotam.Ecs;
using Leopotam.EcsLite;
using UnityEngine;

namespace GBJAM9.Ecs
{
    public class LookingDirectionIndicatorSystem : BaseSystem, IEcsRunSystem, IEntityCreatedHandler, IEntityDestroyedHandler
    {
        [SerializeField]
        protected GameObject indicatorPrefab;
        
        public void OnEntityCreated(Gemserk.Leopotam.Ecs.World world, int entity)
        {
            var lookingDirectionComponents = world.GetComponents<LookingDirection>();
            if (lookingDirectionComponents.Has(entity))
            {
                var lookingDirection = lookingDirectionComponents.Get(entity);
                
                if (!lookingDirection.disableIndicator)
                {
                    var indicators = world.GetComponents<LookingDirectionIndicator>();
                    ref var indicatorComponent = ref indicators.Add(entity);
                    indicatorComponent.instance = GameObject.Instantiate(indicatorPrefab);
                }
            }
        }

        public void OnEntityDestroyed(Gemserk.Leopotam.Ecs.World world, int entity)
        {
            var indicators = world.GetComponents<LookingDirectionIndicator>();
            if (indicators.Has(entity))
            {
                ref var indicatorComponent = ref indicators.Get(entity);
                if (indicatorComponent.instance != null)
                {
                    GameObject.Destroy(indicatorComponent.instance);
                }
                indicatorComponent.instance = null;
            }
        }
        
        public void Run(EcsSystems systems)
        {
            var indicators = world.GetComponents<LookingDirectionIndicator>();
            var lookingDirectionComponents = world.GetComponents<LookingDirection>();
            var positions = world.GetComponents<PositionComponent>();
            // var states = world.GetComponents<UnitStateComponent>();

            foreach (var entity in world.GetFilter<LookingDirectionIndicator>().Inc<LookingDirection>().End())
            {
                var indicatorComponent = indicators.Get(entity);
                var lookingDirection = lookingDirectionComponents.Get(entity);

                var indicatorInstance = indicatorComponent.instance;
                var pivot = indicatorInstance.transform.Find("Pivot");

                var eulerAngles = pivot.localEulerAngles;
                eulerAngles.z = Vector2.SignedAngle(Vector2.right, lookingDirection.value);
                pivot.localEulerAngles = eulerAngles;
            }
            
            foreach (var entity in world.GetFilter<LookingDirectionIndicator>().Inc<PositionComponent>().End())
            {
                var indicatorComponent = indicators.Get(entity);
                var positionComponent = positions.Get(entity);

                var indicatorInstance = indicatorComponent.instance;
                indicatorInstance.transform.position = positionComponent.value;
            }
            
            // indicator could be another entity instead of being only the model and use common model stuff.
            // foreach (var entity in world.GetFilter<LookingDirectionIndicator>().Inc<UnitStateComponent>().End())
            // {
            //     var indicatorComponent = indicators.Get(entity);
            //     var state = states.Get(entity);
            //
            //     var indicatorInstance = indicatorComponent.instance;
            //     indicatorInstance.layer = state.isDeath ? LayerMask.NameToLayer("Hidden") : LayerMask.NameToLayer("Default");
            // }
        }


    }
}