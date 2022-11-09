using Gemserk.Leopotam.Ecs;
using Gemserk.Leopotam.Ecs.Gameplay;
using Leopotam.EcsLite;
using UnityEngine;

namespace Beatemup.Ecs
{
    public class HitVfxSystem : BaseSystem, IEcsRunSystem
    {
        public GameObject hitDefinition;
        
        public void Run(EcsSystems systems)
        {
            if (hitDefinition == null)
            {
                return;
            }
            
            var hitComponents = world.GetComponents<HitComponent>();
            var positionComponents = world.GetComponents<PositionComponent>();
            var hurtBoxComponents = world.GetComponents<HurtBoxComponent>();
            
            foreach (var entity in world.GetFilter<HitComponent>().Inc<PositionComponent>().Inc<HurtBoxComponent>().End())
            {
                ref var hitComponent = ref hitComponents.Get(entity);

                if (hitComponent.hits == 0)
                {
                    continue;
                }

                for (int i = 0; i < hitComponent.hits; i++)
                {
                    var position = positionComponents.Get(entity);
                    var hurtBox = hurtBoxComponents.Get(entity);

                    var hitEntity = world.CreateEntity(hitDefinition.GetInterface<IEntityDefinition>());
                    ref var hitPosition = ref world.GetComponent<PositionComponent>(hitEntity);

                    hitPosition.value = position.value;
                    hitPosition.value.x += UnityEngine.Random.Range(-hurtBox.size.x, hurtBox.size.x) * 0.5f;
                    hitPosition.value.z += UnityEngine.Random.Range(0, hurtBox.size.y);
                    hitPosition.value.y = position.value.y - 0.01f;
                }
            }
        }
    }
}