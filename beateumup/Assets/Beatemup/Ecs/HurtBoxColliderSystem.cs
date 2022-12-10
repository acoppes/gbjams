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
                
                var targetReference = instance.AddComponent<TargetReference>();
                
                var boxCollider = instance.AddComponent<BoxCollider>();
                boxCollider.size = new Vector3(hitBox.hurt.size.x, hitBox.hurt.size.y, hitBox.hurt.depth);
                boxCollider.isTrigger = true;

                world.AddComponent(entity, new HurtBoxColliderComponent()
                {
                    collider = boxCollider,
                    targetReference = targetReference
                });
            }
        }

        public void OnEntityDestroyed(Gemserk.Leopotam.Ecs.World world, Entity entity)
        {
            var hurtBoxColliders = world.GetComponents<HurtBoxColliderComponent>();
            if (hurtBoxColliders.Has(entity))
            {
                ref var hurtBoxCollider = ref hurtBoxColliders.Get(entity);
                
                if (hurtBoxCollider.targetReference != null)
                {
                    GameObject.DestroyImmediate(hurtBoxCollider.targetReference.gameObject);
                }
                
                hurtBoxCollider.targetReference = null;
                hurtBoxCollider.collider = null;
            }
        }

        public void Run(EcsSystems systems)
        {
            var hitBoxes = world.GetComponents<HitBoxComponent>();
            var hurtBoxColliders = world.GetComponents<HurtBoxColliderComponent>();
            var targetComponents = world.GetComponents<TargetComponent>();
            
            foreach (var entity in world.GetFilter<HurtBoxColliderComponent>().Inc<TargetComponent>().End())
            {
                ref var hurtBoxColliderComponent = ref hurtBoxColliders.Get(entity);
                var targetComponent = targetComponents.Get(entity);

                hurtBoxColliderComponent.targetReference.target = targetComponent.target;
            }
            
            foreach (var entity in world.GetFilter<HurtBoxColliderComponent>().Inc<HitBoxComponent>().End())
            {
                var hitBox = hitBoxes.Get(entity);
                ref var hurtBoxColliderComponent = ref hurtBoxColliders.Get(entity);
                
                hurtBoxColliderComponent.targetReference.transform.position = hitBox.hurt.position + hitBox.hurt.offset;
                hurtBoxColliderComponent.collider.enabled = hitBox.hurt.size.SqrMagnitude() > Mathf.Epsilon;
                hurtBoxColliderComponent.collider.size = new Vector3(hitBox.hurt.size.x, hitBox.hurt.size.y, hitBox.hurt.depth);
            }
        }
    }
}