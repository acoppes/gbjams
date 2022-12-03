using Gemserk.Leopotam.Ecs;
using Gemserk.Leopotam.Ecs.Gameplay;
using Leopotam.EcsLite;
using UnityEngine;

namespace Beatemup.Ecs
{
    public class HitVfxSystem : BaseSystem, IEcsRunSystem
    {
        public float maxRandomDelay = 0.25f;
        
        public GameObject hitDefinition;
        
        public void Run(EcsSystems systems)
        {
            if (hitDefinition == null)
            {
                return;
            }
            
            var hitComponents = world.GetComponents<HitPointsComponent>();
            var positionComponents = world.GetComponents<PositionComponent>();
            var hitBoxComponents = world.GetComponents<HitBoxComponent>();
            
            foreach (var entity in world.GetFilter<HitPointsComponent>().Inc<PositionComponent>().Inc<HitBoxComponent>().End())
            {
                ref var hitComponent = ref hitComponents.Get(entity);

                if (hitComponent.hits.Count == 0)
                {
                    continue;
                }

                for (int i = 0; i < hitComponent.hits.Count; i++)
                {
                    // var hitData = hitComponent.hits[i];
                    
                    var position = positionComponents.Get(entity);
                    var hitBox = hitBoxComponents.Get(entity);

                    var hitVfxEntity = world.CreateEntity(hitDefinition.GetInterface<IEntityDefinition>());
                    ref var hitVfxPosition = ref world.GetComponent<PositionComponent>(hitVfxEntity);

                    hitVfxPosition.value = position.value;
                    hitVfxPosition.value.x += UnityEngine.Random.Range(-hitBox.hurt.size.x, hitBox.hurt.size.x) * 0.5f;
                    hitVfxPosition.value.z += UnityEngine.Random.Range(0, hitBox.hurt.size.y);
                    hitVfxPosition.value.y = position.value.y - 0.01f;
                    
                    ref var hitVfxComponent = ref world.GetComponent<VfxComponent>(hitVfxEntity);
                    hitVfxComponent.delay = UnityEngine.Random.Range(0.0f, maxRandomDelay);
                }
            }
        }
    }
}