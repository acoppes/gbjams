using Beatemup.Models;
using Gemserk.Leopotam.Ecs;
using Leopotam.EcsLite;
using UnityEngine;
using Vertx.Debugging;

namespace Beatemup.Ecs
{
    public class HurtBoxColliderSystem : BaseSystem, IEcsRunSystem, IEntityCreatedHandler, IEntityDestroyedHandler
    {
        public GamePerspectiveAsset gamePerspective;
        
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
                boxCollider.size = hitBox.hurt.size;
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
                
                hurtBoxColliderComponent.targetReference.transform.position = hitBox.hurt.position3d;
                hurtBoxColliderComponent.collider.enabled = hitBox.hurt.size.sqrMagnitude > Mathf.Epsilon;
                hurtBoxColliderComponent.collider.size = hitBox.hurt.size;

                var position = gamePerspective.ConvertFromWorld(hitBox.hurt.position3d);
                var size = hitBox.hurt.size;

                // position.y *= gamePerspective.gamePerspectiveY;
                // size.y *= gamePerspective.gamePerspectiveY;
                
                D.raw(new Shape.Box(position + hitBox.hurt.offset, size * 0.5f), Color.green);
                // D.raw(new Shape.Text(position + hitBox.hurt.offset, "HURT"), Color.green, Color.black);
            }
        }
    }
}