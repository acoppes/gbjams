using Beatemup.Models;
using Gemserk.Leopotam.Ecs;
using Leopotam.EcsLite;
using UnityEngine;

namespace Beatemup.Ecs
{
    public class HurtBoxColliderSystem : BaseSystem, IEcsRunSystem, IEntityCreatedHandler, IEntityDestroyedHandler
    {
        public void OnEntityCreated(Gemserk.Leopotam.Ecs.World world, Entity entity)
        {
            var hitBoxes = world.GetComponents<HitBoxComponent>();
            if (hitBoxes.Has(entity))
            {
                ref var hitBox = ref hitBoxes.Get(entity);

                var instance = new GameObject("HurtBox");
                instance.layer = LayerMask.NameToLayer("HurtBox");

                hitBox.instance = instance.AddComponent<ColliderEntityReference>();
                
                hitBox.instance.boxCollider2D = instance.AddComponent<BoxCollider2D>();
                hitBox.instance.boxCollider2D.size = hitBox.hurt.size;
                hitBox.instance.boxCollider2D.isTrigger = true;

                hitBox.instance.entity = entity;
            }
        }

        public void OnEntityDestroyed(Gemserk.Leopotam.Ecs.World world, Entity entity)
        {
            var hitBoxes = world.GetComponents<HitBoxComponent>();
            if (hitBoxes.Has(entity))
            {
                ref var hitBox = ref hitBoxes.Get(entity);
                if (hitBox.instance != null)
                {
                    GameObject.DestroyImmediate(hitBox.instance.gameObject);
                }
                hitBox.instance = null;
            }
        }

        public void Run(EcsSystems systems)
        {
            var hitBoxes = world.GetComponents<HitBoxComponent>();
            // var positionComponents = world.GetComponents<PositionComponent>();
            
            foreach (var entity in world.GetFilter<HitBoxComponent>().End())
            {
                ref var hitBox = ref hitBoxes.Get(entity);
                // var positionComponent = positionComponents.Get(entity);
                
                hitBox.instance.transform.position = hitBox.hurt.position + hitBox.hurt.offset;
                hitBox.instance.boxCollider2D.size = hitBox.hurt.size;
            }
        }
    }
}