using Beatemup.Models;
using Gemserk.Leopotam.Ecs;
using Gemserk.Leopotam.Ecs.Gameplay;
using Leopotam.EcsLite;
using UnityEngine;

namespace Beatemup.Ecs
{
    public class HurtBoxColliderSystem : BaseSystem, IEcsRunSystem, IEntityCreatedHandler, IEntityDestroyedHandler
    {
        public void OnEntityCreated(Gemserk.Leopotam.Ecs.World world, Entity entity)
        {
            var hurtBoxes = world.GetComponents<HurtBoxComponent>();
            if (hurtBoxes.Has(entity))
            {
                ref var hurtBoxComponent = ref hurtBoxes.Get(entity);

                var instance = new GameObject("HurtBox");
                instance.layer = LayerMask.NameToLayer("HurtBox");

                hurtBoxComponent.instance = instance.AddComponent<ColliderEntityReference>();
                
                hurtBoxComponent.instance.boxCollider2D = instance.AddComponent<BoxCollider2D>();
                hurtBoxComponent.instance.boxCollider2D.size = hurtBoxComponent.size;
                hurtBoxComponent.instance.boxCollider2D.isTrigger = true;

                hurtBoxComponent.instance.entity = entity;
            }
        }

        public void OnEntityDestroyed(Gemserk.Leopotam.Ecs.World world, Entity entity)
        {
            var hurtBoxes = world.GetComponents<HurtBoxComponent>();
            if (hurtBoxes.Has(entity))
            {
                ref var hurtBoxComponent = ref hurtBoxes.Get(entity);
                if (hurtBoxComponent.instance != null)
                {
                    GameObject.DestroyImmediate(hurtBoxComponent.instance.gameObject);
                }
                hurtBoxComponent.instance = null;
            }
        }

        public void Run(EcsSystems systems)
        {
            var hurtBoxes = world.GetComponents<HurtBoxComponent>();
            var positionComponents = world.GetComponents<PositionComponent>();
            
            foreach (var entity in world.GetFilter<HurtBoxComponent>().Inc<PositionComponent>().End())
            {
                ref var hurtBoxComponent = ref hurtBoxes.Get(entity);
                var positionComponent = positionComponents.Get(entity);
                
                hurtBoxComponent.instance.transform.position = positionComponent.value;
                hurtBoxComponent.instance.boxCollider2D.size = hurtBoxComponent.size;
            }
        }
    }
}