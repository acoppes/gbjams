using Gemserk.Leopotam.Ecs;
using Gemserk.Leopotam.Ecs.Gameplay;
using Leopotam.EcsLite;
using UnityEngine;

namespace Beatemup.Ecs
{
    public class DebugHitBoxSystem : BaseSystem, IEcsRunSystem, IEntityCreatedHandler, IEntityDestroyedHandler
    {
        public GameObject debugHitBoxPrefab;
        public GameObject debugHurtBoxPrefab;
        public GameObject debugDepthBoxPrefab;

        public void OnEntityCreated(World world, Entity entity)
        {
            if (world.HasComponent<HitBoxComponent>(entity))
            {
                ref var hitBox = ref world.GetComponent<HitBoxComponent>(entity);
                
                hitBox.debugHitBox = GameObject.Instantiate(debugHitBoxPrefab);
                hitBox.debugHitBox.SetActive(true);
                hitBox.debugHitBox.transform.localScale = new Vector3(0, 0, 1);
                
                hitBox.debugHurtBox = GameObject.Instantiate(debugHurtBoxPrefab);
                hitBox.debugHurtBox.SetActive(true);
                hitBox.debugHurtBox.transform.localScale = new Vector3(0, 0, 1);
                
                hitBox.debugDepthBox = GameObject.Instantiate(debugDepthBoxPrefab);
                hitBox.debugDepthBox.SetActive(true);
                hitBox.debugDepthBox.transform.localScale = new Vector3(0, 0, 1);
            }
        }
        
        public void OnEntityDestroyed(World world, Entity entity)
        {
            if (world.HasComponent<HitBoxComponent>(entity))
            {
                ref var hitBox = ref world.GetComponent<HitBoxComponent>(entity);

                if (hitBox.debugHitBox != null)
                {
                    GameObject.DestroyImmediate(hitBox.debugHitBox);
                }

                if (hitBox.debugHurtBox != null)
                {
                    GameObject.DestroyImmediate(hitBox.debugHurtBox);
                }
                
                if (hitBox.debugDepthBox != null)
                {
                    GameObject.DestroyImmediate(hitBox.debugDepthBox);
                }
                
                hitBox.debugHitBox = null;
                hitBox.debugHurtBox = null;
                hitBox.debugDepthBox = null;
            }
        }
        
        public void Run(EcsSystems systems)
        {
            var hitBoxComponents = world.GetComponents<HitBoxComponent>();
            var positionComponents = world.GetComponents<PositionComponent>();
            
            foreach (var entity in world.GetFilter<HitBoxComponent>().End())
            {
                var hitBox = hitBoxComponents.Get(entity);
                
                hitBox.debugHitBox.transform.position = hitBox.hit.position + hitBox.hit.offset;
                hitBox.debugHitBox.transform.localScale = new Vector3(hitBox.hit.size.x, hitBox.hit.size.y, 1);
                
                hitBox.debugHurtBox.transform.position = hitBox.hurt.position + hitBox.hurt.offset;
                hitBox.debugHurtBox.transform.localScale = new Vector3(hitBox.hurt.size.x, hitBox.hurt.size.y, 1);
            }
            
            foreach (var entity in world.GetFilter<HitBoxComponent>().Inc<PositionComponent>().End())
            {
                var hitBox = hitBoxComponents.Get(entity);
                var position = positionComponents.Get(entity);
                
                hitBox.debugDepthBox.transform.position = new Vector3(position.value.x, position.value.y, 0);
                hitBox.debugDepthBox.transform.localScale = new Vector3(hitBox.hurt.size.x, hitBox.depth * 2.0f, 1);
            }
            
        }
    }
}